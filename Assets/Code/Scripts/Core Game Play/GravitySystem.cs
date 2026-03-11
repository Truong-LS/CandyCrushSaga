using System.Collections;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    private BoardManager boardManager;

    [Header("Gravity Settings")]
    public float fallDelay = 0.4f; // Thời gian chờ kẹo rơi xong trước khi sinh kẹo mới

    void Awake()
    {
        boardManager = GetComponent<BoardManager>();
    }

    // Hàm này được BoardManager gọi sau khi đã xóa (Destroy) các viên kẹo match
    public void StartGravity()
    {
        StartCoroutine(DropCandiesRoutine());
    }

    private IEnumerator DropCandiesRoutine()
    {
        bool hasFallen = false;

        // 1. QUÉT TỪNG CỘT TỪ DƯỚI LÊN TRÊN ĐỂ KÉO KẸO XUỐNG
        for (int x = 0; x < boardManager.width; x++)
        {
            int nullCount = 0; // Đếm số ô trống trong cột hiện tại

            for (int y = 0; y < boardManager.height; y++)
            {
                if (boardManager.allCandies[x, y] == null)
                {
                    nullCount++; // Gặp ô trống -> Tăng biến đếm
                }
                else if (nullCount > 0)
                {
                    // Nếu gặp ô có kẹo VÀ bên dưới nó có ô trống -> Kéo nó xuống
                    Candy currentCandy = boardManager.allCandies[x, y];

                    // Xóa kẹo ở vị trí cũ trong mảng
                    boardManager.allCandies[x, y] = null;

                    // Chuyển kẹo đến vị trí mới trong mảng (y hiện tại trừ đi số ô trống)
                    boardManager.allCandies[x, y - nullCount] = currentCandy;

                    // Cập nhật tọa độ logic cho viên kẹo (Candy.cs sẽ tự động Lerp nó rơi xuống)
                    currentCandy.UpdatePosition(x, y - nullCount);

                    hasFallen = true;
                }
            }
        }

        // Nếu có kẹo rơi, chờ một chút để animation chạy xong
        if (hasFallen)
        {
            yield return new WaitForSeconds(fallDelay);

            // --- VỊ TRÍ 1: CHÈN ÂM THANH RƠI Ở ĐÂY ---
            // Phát âm thanh khi các viên kẹo cũ vừa rơi lấp đầy chỗ trống
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayLandingSound();
            }
        }

        // 2. SINH KẸO MỚI LẤP ĐẦY BẢNG
        StartCoroutine(FillBoardRoutine());
    }

    private IEnumerator FillBoardRoutine()
    {
        bool hasSpawned = false;

        for (int x = 0; x < boardManager.width; x++)
        {
            for (int y = 0; y < boardManager.height; y++)
            {
                // Tìm những ô còn trống (thường là ở các hàng trên cùng sau khi kẹo đã rơi)
                if (boardManager.allCandies[x, y] == null)
                {
                    // Mẹo: Sinh kẹo ở vị trí cao hơn bảng (ví dụ y + 2) để nó có hiệu ứng "rơi từ ngoài màn hình vào"
                    Vector2 spawnPosition = new Vector2(x, boardManager.height + 2);

                    int randomType = Random.Range(0, boardManager.candyPrefabs.Length);
                    GameObject newCandyObj = Instantiate(boardManager.candyPrefabs[randomType], spawnPosition, Quaternion.identity);
                    newCandyObj.transform.parent = boardManager.transform;

                    Candy newCandy = newCandyObj.GetComponent<Candy>();

                    // Khởi tạo kẹo mới với tọa độ đích là (x, y)
                    newCandy.Init(x, y, randomType);

                    boardManager.allCandies[x, y] = newCandy;
                    hasSpawned = true;
                }
            }
        }

        // Nếu có kẹo mới sinh ra, chờ nó rơi xuống đúng vị trí
        if (hasSpawned)
        {
            yield return new WaitForSeconds(fallDelay);

            //// --- VỊ TRÍ 2: CHÈN ÂM THANH RƠI Ở ĐÂY ---
            //// Phát âm thanh khi các viên kẹo mới vừa sinh ra đã rơi chạm bảng
            //if (AudioManager.instance != null)
            //{
            //    AudioManager.instance.PlayLandingSound();
            //}
        }

        // 3. KẾT THÚC RƠI -> BÁO BOARD MANAGER KIỂM TRA COMBO LIÊN HOÀN
        boardManager.CheckMatchesAsync();
    }
}