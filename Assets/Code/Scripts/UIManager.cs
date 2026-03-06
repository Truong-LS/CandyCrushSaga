using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Cốt lõi của Singleton: Tạo một cổng kết nối toàn cục
    public static UIManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;

    [Header("Game Stats")]
    private int score = 0;
    public int maxMoves = 20; // Số lượt đi ban đầu
    private int currentMoves;

    void Awake()
    {
        // Thiết lập Singleton: Đảm bảo chỉ có 1 UIManager duy nhất tồn tại
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Xóa bản sao nếu lỡ tạo dư
        }
    }

    void Start()
    {
        // Khởi tạo thông số khi bắt đầu màn chơi
        currentMoves = maxMoves;
        UpdateUI();
    }

    // Hàm này được gọi từ BoardManager khi kẹo nổ
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // Hàm này được gọi từ BoardManager khi người chơi vuốt kẹo thành công
    public void UseMove()
    {
        currentMoves--;
        UpdateUI();

        // Kiểm tra điều kiện thua
        if (currentMoves <= 0)
        {
            GameOver();
        }
    }

    // Cập nhật lại các dòng chữ trên màn hình
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        if (movesText != null)
        {
            movesText.text = "Moves: " + currentMoves.ToString();
        }
    }

    private void GameOver()
    {
        Debug.Log("Hết lượt đi! GAME OVER!");
        // TODO: Mở bảng thông báo Thua cuộc ở đây (Ta sẽ làm sau)
    }
}