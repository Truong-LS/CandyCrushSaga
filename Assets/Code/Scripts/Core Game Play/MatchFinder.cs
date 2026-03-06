using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private BoardManager boardManager;

    void Awake()
    {
        // Lấy Component BoardManager gắn cùng một GameObject (Grid)
        boardManager = GetComponent<BoardManager>();
    }

    /// <summary>
    /// Hàm này quét toàn bộ bảng và trả về danh sách các viên kẹo đã match
    /// </summary>
    public List<Candy> FindAllMatches()
    {
        // Sử dụng HashSet để tự động loại bỏ các viên kẹo bị trùng 
        // (Ví dụ: 1 viên kẹo nằm ở góc chữ L hoặc chữ T sẽ bị quét trúng 2 lần)
        HashSet<Candy> matchedCandies = new HashSet<Candy>();

        // 1. KIỂM TRA THEO CHIỀU NGANG
        for (int y = 0; y < boardManager.height; y++)
        {
            for (int x = 0; x < boardManager.width - 2; x++) // width - 2 để không bị lỗi tràn mảng (Out of bounds)
            {
                Candy candy1 = boardManager.allCandies[x, y];
                Candy candy2 = boardManager.allCandies[x + 1, y];
                Candy candy3 = boardManager.allCandies[x + 2, y];

                // Đảm bảo cả 3 ô đều có kẹo
                if (candy1 != null && candy2 != null && candy3 != null)
                {
                    // Nếu cùng loại kẹo
                    if (candy1.candyType == candy2.candyType && candy2.candyType == candy3.candyType)
                    {
                        candy1.isMatched = true;
                        candy2.isMatched = true;
                        candy3.isMatched = true;

                        matchedCandies.Add(candy1);
                        matchedCandies.Add(candy2);
                        matchedCandies.Add(candy3);
                    }
                }
            }
        }

        // 2. KIỂM TRA THEO CHIỀU DỌC
        for (int x = 0; x < boardManager.width; x++)
        {
            for (int y = 0; y < boardManager.height - 2; y++) // height - 2 để không bị lỗi tràn mảng
            {
                Candy candy1 = boardManager.allCandies[x, y];
                Candy candy2 = boardManager.allCandies[x, y + 1];
                Candy candy3 = boardManager.allCandies[x, y + 2];

                // Đảm bảo cả 3 ô đều có kẹo
                if (candy1 != null && candy2 != null && candy3 != null)
                {
                    // Nếu cùng loại kẹo
                    if (candy1.candyType == candy2.candyType && candy2.candyType == candy3.candyType)
                    {
                        candy1.isMatched = true;
                        candy2.isMatched = true;
                        candy3.isMatched = true;

                        matchedCandies.Add(candy1);
                        matchedCandies.Add(candy2);
                        matchedCandies.Add(candy3);
                    }
                }
            }
        }

        // Chuyển HashSet thành List và trả về cho BoardManager
        return matchedCandies.ToList();
    }
}
