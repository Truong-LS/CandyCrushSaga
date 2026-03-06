using UnityEngine;

public class InputManager : MonoBehaviour
{
    private BoardManager boardManager;

    [Header("Swipe Settings")]
    public float swipeResist = 0.5f; // Khoảng cách tối thiểu để ghi nhận là 1 cú vuốt (tránh click nhầm)

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Candy currentCandy; // Viên kẹo đang được bấm giữ

    void Awake()
    {
        boardManager = GetComponent<BoardManager>();
    }

    void Update()
    {
        // TÙY CHỌN: Nếu BoardManager đang xử lý kẹo rơi hoặc đang tính điểm, chặn không cho vuốt
        // if (boardManager.currentState != GameState.Playing) return;

        if (Input.GetMouseButtonDown(0))
        {
            CalculateFirstTouch();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CalculateFinalTouch();
        }
    }

    private void CalculateFirstTouch()
    {
        // Lấy tọa độ chuột/ngón tay trên màn hình và chuyển sang hệ tọa độ của game (World Point)
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Bắn tia Raycast 2D tại vị trí chạm để xem có trúng viên kẹo nào không
        RaycastHit2D hit = Physics2D.Raycast(firstTouchPosition, Vector2.zero);
        if (hit.collider != null)
        {
            currentCandy = hit.collider.GetComponent<Candy>();

            // Làm mờ viên kẹo để báo hiệu đang chọn
            if (currentCandy != null)
            {
                currentCandy.SetSelected(true);
            }
        }
    }

    private void CalculateFinalTouch()
    {
        if (currentCandy != null)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Tính khoảng cách vuốt. Nếu vuốt đủ dài thì mới tính là Swap
            float swipeDistance = Vector2.Distance(firstTouchPosition, finalTouchPosition);
            if (swipeDistance > swipeResist)
            {
                CalculateAngleAndDirection();
            }

            // Trả lại màu gốc cho viên kẹo
            currentCandy.SetSelected(false);
            currentCandy = null; // Xóa tham chiếu
        }
    }

    private void CalculateAngleAndDirection()
    {
        // Tính góc vuốt bằng hàm Atan2 (trả về giá trị từ -180 đến 180 độ)
        float swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;

        Vector2Int swipeDirection = Vector2Int.zero;

        // Chuyển đổi góc vuốt thành Hướng (Vector2Int)
        if (swipeAngle > -45 && swipeAngle <= 45)
        {
            swipeDirection = Vector2Int.right; // Vuốt phải
        }
        else if (swipeAngle > 45 && swipeAngle <= 135)
        {
            swipeDirection = Vector2Int.up;    // Vuốt lên
        }
        else if (swipeAngle > 135 || swipeAngle <= -135)
        {
            swipeDirection = Vector2Int.left;  // Vuốt trái
        }
        else if (swipeAngle < -45 && swipeAngle >= -135)
        {
            swipeDirection = Vector2Int.down;  // Vuốt xuống
        }

        // Truyền kẹo đang chọn và hướng vuốt sang cho BoardManager xử lý logic
        if (swipeDirection != Vector2Int.zero)
        {
            boardManager.SwapCandies(currentCandy, swipeDirection);
        }
    }
}
