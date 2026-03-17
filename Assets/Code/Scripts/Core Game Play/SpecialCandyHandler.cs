using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialCandyHandler : MonoBehaviour
{
    private BoardManager boardManager;
    public bool flag = false;

    void Awake()
    {
        boardManager = GetComponent<BoardManager>();
    }

    /// <summary>
    /// Hàm này được BoardManager gọi SAU KHI MatchFinder tìm ra list kẹo, 
    /// và TRƯỚC KHI các viên kẹo bị Destroy.
    /// </summary>
    /// <param name="matchedCandies">Danh sách kẹo tạo thành chuỗi</param>
    /// <param name="swipeDirection">Hướng vuốt của người chơi (để biết tạo sọc ngang hay dọc)</param>
    /// <param name="swipedCandy">Viên kẹo người chơi vừa chạm vào (để ưu tiên sinh kẹo ở đây)</param>
    public void CheckAndSpawnSpecialCandy(List<Candy> matchedCandies, Vector2Int swipeDirection, Candy swipedCandy)
    {
        // Nếu số lượng match nhỏ hơn 4, chắc chắn không có kẹo đặc biệt
        if (matchedCandies.Count < 4) return;

        // Ưu tiên sinh kẹo đặc biệt tại vị trí người chơi vừa vuốt. 
        // Nếu người chơi không vuốt (kẹo tự rơi tạo combo), chọn ngẫu nhiên viên đầu tiên trong list.
        Candy spawnTarget = (swipedCandy != null && matchedCandies.Contains(swipedCandy)) ? swipedCandy : matchedCandies[0];

        int spawnX = spawnTarget.xIndex;
        int spawnY = spawnTarget.yIndex;
        int colorType = spawnTarget.candyType;

        // 1. KIỂM TRA COLOR BOMB (Kẹo Sô-cô-la 5 viên thẳng hàng) hoặc WRAPPED CANDY (Chữ L, T)
        if (matchedCandies.Count >= 5)
        {
            if (IsStraightLine(matchedCandies))
            {
                // 5 viên thẳng hàng -> Sinh Color Bomb
                CreateSpecialCandy(spawnX, spawnY, colorType, SpecialType.ColorBomb);
            }
            else
            {
                // Từ 5 viên trở lên nhưng không thẳng hàng -> Chắc chắn là giao điểm hình L hoặc T
                // (Vì MatchFinder chỉ tìm theo hàng ngang và dọc, để ra hình gãy khúc thì phải có giao điểm)
                CreateSpecialCandy(spawnX, spawnY, colorType, SpecialType.Wrapped);
            }
            return; // Xử lý xong kẹo cấp cao thì thoát
        }

        // 2. KIỂM TRA STRIPED CANDY (Kẹo sọc 4 viên)
        if (matchedCandies.Count == 4)
        {
            // Nếu người chơi vuốt sang Trái/Phải (x != 0) -> Tạo kẹo sọc Ngang
            // Nếu vuốt Lên/Xuống (y != 0) -> Tạo kẹo sọc Dọc
            // Nếu không có hướng vuốt (Combo tự rơi) -> Random ngang hoặc dọc

            SpecialType stripedType = SpecialType.Horizontal;

            if (swipeDirection.x != 0)
            {
                stripedType = SpecialType.Horizontal;
            }
            else if (swipeDirection.y != 0)
            {
                stripedType = SpecialType.Vertical;
            }
            else
            {
                stripedType = (Random.value > 0.5f) ? SpecialType.Horizontal : SpecialType.Vertical;
            }

            CreateSpecialCandy(spawnX, spawnY, colorType, stripedType);
            flag = true;
        }
    }

    // Hàm phụ trợ: Kiểm tra xem danh sách kẹo có nằm trên cùng 1 hàng ngang hoặc dọc không
    private bool IsStraightLine(List<Candy> candies)
    {
        bool sameX = true;
        bool sameY = true;

        int firstX = candies[0].xIndex;
        int firstY = candies[0].yIndex;

        foreach (Candy candy in candies)
        {
            if (candy.xIndex != firstX) sameX = false;
            if (candy.yIndex != firstY) sameY = false;
        }

        // Nếu tất cả cùng x hoặc tất cả cùng y -> Thẳng hàng
        return sameX || sameY;
    }

    // Hàm ra lệnh cho BoardManager sinh kẹo đặc biệt
    private void CreateSpecialCandy(int x, int y, int colorType, SpecialType specialType)
    {
        Debug.Log($"[SpecialCandyHandler] Yêu cầu sinh kẹo: {specialType} tại ({x}, {y})");

        // Lưu ý: Ở đây ta gọi một hàm của BoardManager để nó biến viên kẹo đang chuẩn bị xóa 
        // thành kẹo đặc biệt, hoặc đánh dấu ô (x,y) này để tý nữa không xóa nó mà "nâng cấp" nó lên.

        boardManager.MarkForSpecialUpgrade(x, y, colorType, specialType);
    }

    public List<Candy> GetCandiesAffectedBySpecials(List<Candy> initialMatches)
    {
        // HashSet tự động ngăn chặn việc nổ trùng một viên kẹo nhiều lần
        HashSet<Candy> candiesToDestroy = new HashSet<Candy>(initialMatches);
        Queue<Candy> checkQueue = new Queue<Candy>(initialMatches);

        while (checkQueue.Count > 0)
        {
            Candy currentCandy = checkQueue.Dequeue();

            // 1. KẸO SỌC NGANG
            if (currentCandy.specialType == SpecialType.Horizontal)
            {
                for (int x = 0; x < boardManager.width; x++)
                {
                    Candy c = boardManager.allCandies[x, currentCandy.yIndex];
                    if (c != null && !candiesToDestroy.Contains(c))
                    {
                        candiesToDestroy.Add(c);
                        checkQueue.Enqueue(c); // Đưa vào Queue để kiểm tra nổ dây chuyền
                    }
                }
            }
            // 2. KẸO SỌC DỌC
            else if (currentCandy.specialType == SpecialType.Vertical)
            {
                for (int y = 0; y < boardManager.height; y++)
                {
                    Candy c = boardManager.allCandies[currentCandy.xIndex, y];
                    if (c != null && !candiesToDestroy.Contains(c))
                    {
                        candiesToDestroy.Add(c);
                        checkQueue.Enqueue(c);
                    }
                }
            }
            // 3. KẸO GÓI (WRAPPED) - Nổ vùng 3x3
            else if (currentCandy.specialType == SpecialType.Wrapped)
            {
                for (int x = currentCandy.xIndex - 1; x <= currentCandy.xIndex + 1; x++)
                {
                    for (int y = currentCandy.yIndex - 1; y <= currentCandy.yIndex + 1; y++)
                    {
                        // Kiểm tra xem có bị tràn viền bảng (Out of bounds) không
                        if (x >= 0 && x < boardManager.width && y >= 0 && y < boardManager.height)
                        {
                            Candy c = boardManager.allCandies[x, y];
                            if (c != null && !candiesToDestroy.Contains(c))
                            {
                                candiesToDestroy.Add(c);
                                checkQueue.Enqueue(c);
                            }
                        }
                    }
                }
            }
        }

        return candiesToDestroy.ToList();
    }
}
