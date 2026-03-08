using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Playing, Processing }

public class BoardManager : MonoBehaviour
{
    [Header("Board Dimensions")]
    public int width = 8;
    public int height = 8;

    [Header("Prefabs")]
    public GameObject[] candyPrefabs;
    public GameObject colorBombPrefab;

    // Mảng 2 chiều lưu trữ kẹo
    public Candy[,] allCandies;
    public GameState currentState = GameState.Playing;

    [Header("Timing")]
    public float swapTime = 0.3f; // Thời gian chờ kẹo đổi chỗ (Lerp) xong mới check match

    // Tham chiếu đến các hệ thống khác (Vệ tinh)
    [HideInInspector] public MatchFinder matchFinder;
    [HideInInspector] public GravitySystem gravitySystem;
    [HideInInspector] public SpecialCandyHandler specialHandler;

    void Awake()
    {
        // Khởi tạo các tham chiếu
        matchFinder = GetComponent<MatchFinder>();
        gravitySystem = GetComponent<GravitySystem>();
        specialHandler = GetComponent<SpecialCandyHandler>();

        allCandies = new Candy[width, height];
    }

    void Start()
    {
        SetUpBoard();
    }

    // ================= KHỞI TẠO BẢNG =================

    void SetUpBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int randomType = Random.Range(0, candyPrefabs.Length);

                // Thuật toán chống tạo Match ngay lúc mới sinh bảng
                while (MatchesAt(x, y, randomType))
                {
                    randomType = Random.Range(0, candyPrefabs.Length);
                }

                SpawnCandyAt(x, y, randomType);
            }
        }
    }

    // Hàm kiểm tra nhanh để tránh tạo match khi khởi tạo
    bool MatchesAt(int x, int y, int type)
    {
        if (x > 1 && allCandies[x - 1, y]?.candyType == type && allCandies[x - 2, y]?.candyType == type) return true;
        if (y > 1 && allCandies[x, y - 1]?.candyType == type && allCandies[x, y - 2]?.candyType == type) return true;
        return false;
    }

    void SpawnCandyAt(int x, int y, int type)
    {
        Vector2 tempPosition = new Vector2(x, y);
        GameObject candyObj = Instantiate(candyPrefabs[type], tempPosition, Quaternion.identity);
        candyObj.transform.parent = this.transform;
        candyObj.name = $"Candy ({x}, {y})";

        Candy candyScript = candyObj.GetComponent<Candy>();
        candyScript.Init(x, y, type);
        allCandies[x, y] = candyScript;
    }

    // ================= LUỒNG XỬ LÝ CHÍNH (Được gọi từ InputManager) =================

    public void SwapCandies(Candy currentCandy, Vector2Int swipeDirection)
    {
        // Chặn Game Over
        if (UIManager.Instance.isGameOver) return;

        // Không cho thao tác nếu game đang xử lý kẹo rơi hoặc tính điểm
        if (currentState != GameState.Playing) return;

        int targetX = currentCandy.xIndex + swipeDirection.x;
        int targetY = currentCandy.yIndex + swipeDirection.y;

        // Kiểm tra xem vị trí đổi có nằm ngoài bảng không
        if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height) return;

        Candy targetCandy = allCandies[targetX, targetY];
        if (targetCandy == null) return;

        // Khóa game, bắt đầu quá trình hoán đổi
        currentState = GameState.Processing;
        StartCoroutine(ProcessSwapRoutine(currentCandy, targetCandy, swipeDirection));
    }

    private IEnumerator ProcessSwapRoutine(Candy currentCandy, Candy targetCandy, Vector2Int swipeDirection)
    {
        DoSwap(currentCandy, targetCandy);
        yield return new WaitForSeconds(swapTime);

        // --- LOGIC MỚI: KIỂM TRA BOM MÀU TẠI ĐÂY ---
        bool isColorBombSwap = false;
        int targetColorToDestroy = -1;

        // Nếu viên mình cầm là Bom Màu
        if (currentCandy.specialType == SpecialType.ColorBomb)
        {
            isColorBombSwap = true;
            targetColorToDestroy = targetCandy.candyType;
        }
        // Nếu viên mình tráo đến là Bom Màu
        else if (targetCandy.specialType == SpecialType.ColorBomb)
        {
            isColorBombSwap = true;
            targetColorToDestroy = currentCandy.candyType;
        }

        if (isColorBombSwap)
        {
            UIManager.Instance.UseMove();

            // Gom tất cả kẹo cùng màu lại để nổ
            List<Candy> colorBombMatches = new List<Candy>();
            colorBombMatches.Add(currentCandy);
            colorBombMatches.Add(targetCandy);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (allCandies[x, y] != null && allCandies[x, y].candyType == targetColorToDestroy)
                    {
                        colorBombMatches.Add(allCandies[x, y]);
                    }
                }
            }

            // Đưa vào chu trình nổ bình thường (có tính cả nổ lan nếu Bom màu quét trúng Kẹo Sọc/Gói)
            ProcessMatches(colorBombMatches);
            yield break; // Kết thúc Coroutine tại đây
        }
        // --- KẾT THÚC LOGIC BOM MÀU ---

        // Nếu không phải Bom Màu thì kiểm tra nối 3 như bình thường (Code cũ của bạn)
        List<Candy> matchedCandies = matchFinder.FindAllMatches();

        if (matchedCandies.Count > 0)
        {
            UIManager.Instance.UseMove();
            specialHandler.CheckAndSpawnSpecialCandy(matchedCandies, swipeDirection, currentCandy);
            ProcessMatches(matchedCandies);
        }
        else
        {
            DoSwap(currentCandy, targetCandy);
            yield return new WaitForSeconds(swapTime);
            currentState = GameState.Playing;
        }
    }

    private void DoSwap(Candy candyA, Candy candyB)
    {
        // Lưu vị trí cũ
        int tempX = candyA.xIndex;
        int tempY = candyA.yIndex;

        // Đổi chỗ trong mảng allCandies
        allCandies[candyB.xIndex, candyB.yIndex] = candyA;
        allCandies[tempX, tempY] = candyB;

        // Cập nhật dữ liệu tọa độ vào viên kẹo (Candy.cs sẽ tự Lerp)
        candyA.UpdatePosition(candyB.xIndex, candyB.yIndex);
        candyB.UpdatePosition(tempX, tempY);
    }

    // ================= XỬ LÝ KẾT QUẢ VÀ CASCADE (COMBO TỰ RƠI) =================

    private void ProcessMatches(List<Candy> matchedCandies)
    {
        List<Candy> allCandiesToDestroy = specialHandler.GetCandiesAffectedBySpecials(matchedCandies);

        // XÓA DÒNG NÀY: UIManager.Instance.AddScore(allCandiesToDestroy.Count * 10);

        // Hủy các viên kẹo
        foreach (Candy candy in allCandiesToDestroy)
        {
            if (candy == null) continue;

            int x = candy.xIndex;
            int y = candy.yIndex;

            // --- THÊM DÒNG NÀY: Báo cho UIManager biết loại kẹo nào vừa nổ ---
            UIManager.Instance.CollectCandy(candy.candyType);

            if (allCandies[x, y] == candy)
            {
                allCandies[x, y] = null;
            }

            Destroy(candy.gameObject);
        }

        gravitySystem.StartGravity();
    }

    // Hàm này được GravitySystem gọi lại SAU KHI kẹo đã rơi và sinh mới xong
    public void CheckMatchesAsync()
    {
        StartCoroutine(CheckMatchesRoutine());
    }

    private IEnumerator CheckMatchesRoutine()
    {
        List<Candy> matchedCandies = matchFinder.FindAllMatches();

        if (matchedCandies.Count > 0)
        {
            yield return new WaitForSeconds(0.2f);
            specialHandler.CheckAndSpawnSpecialCandy(matchedCandies, Vector2Int.zero, null);
            ProcessMatches(matchedCandies);
        }
        else
        {
            // Bảng đã ổn định, không còn kẹo nổ
            currentState = GameState.Playing;

            // --- THÊM DÒNG NÀY: Kiểm tra xem có bị hết lượt không ---
            UIManager.Instance.CheckLossCondition();
        }
    }

    // ================= KẾT NỐI VỚI SPECIAL CANDY =================

    // Hàm này được SpecialCandyHandler gọi để biến 1 viên kẹo sắp bị xóa thành kẹo đặc biệt
    public void MarkForSpecialUpgrade(int x, int y, int colorType, SpecialType specialType)
    {
        // Xóa viên kẹo cũ đi
        if (allCandies[x, y] != null)
        {
            Destroy(allCandies[x, y].gameObject);
            allCandies[x, y] = null;
        }

        GameObject newCandyObj;

        // Nếu là Bom Màu -> Dùng Prefab Bom Màu
        if (specialType == SpecialType.ColorBomb)
        {
            newCandyObj = Instantiate(colorBombPrefab, new Vector2(x, y), Quaternion.identity);
            Candy candyScript = newCandyObj.GetComponent<Candy>();

            candyScript.Init(x, y, 99); // 99 là mã của Bom màu
            candyScript.specialType = SpecialType.ColorBomb;

            allCandies[x, y] = candyScript;
        }
        // Nếu là Sọc / Gói -> Dùng lại Prefab kẹo thường của màu đó và kêu nó Đổi Ảnh
        else
        {
            newCandyObj = Instantiate(candyPrefabs[colorType], new Vector2(x, y), Quaternion.identity);
            Candy candyScript = newCandyObj.GetComponent<Candy>();

            candyScript.Init(x, y, colorType);
            candyScript.UpgradeToSpecial(specialType); // Lệnh đổi ảnh ở đây!

            allCandies[x, y] = candyScript;
        }

        newCandyObj.transform.parent = this.transform;
        newCandyObj.name = $"Special Candy ({x}, {y})";
    }


}