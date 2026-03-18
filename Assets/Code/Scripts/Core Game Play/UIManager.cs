using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tạo một cấu trúc dữ liệu để lưu Mục Tiêu
[System.Serializable]
public class LevelGoal
{
    public int candyType;       // Mã ID của kẹo cần thu thập (0: Đỏ, 1: Xanh...)
    public int targetAmount;    // Số lượng cần ăn
    public int currentAmount;   // Số lượng đã ăn được
    public TextMeshProUGUI uiText; // Tham chiếu đến Text hiển thị số lượng còn lại trên UI
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI movesText;
    // Bỏ scoreText đi vì ta không dùng tính điểm nữa

    [Header("Level Goals")]
    public List<LevelGoal> goals; // Danh sách mục tiêu

    [Header("Game Stats")]
    public int maxMoves = 20;
    [HideInInspector] public int currentMoves; // Chuyển thành public để BoardManager truy cập được

    [Header("Scene Transition")]
    public string winSceneName = "WinScene";   // Tên scene Thắng (bạn có thể đổi trên Inspector)
    public string loseSceneName = "LoseScene"; // Tên scene Thua (bạn có thể đổi trên Inspector)
    public float delayBeforeTransition = 1.5f; // Thời gian chờ trước khi chuyển cảnh

    // Biến chặn để không chạy logic khi game đã kết thúc
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentMoves = maxMoves;
        UpdateMovesUI();
        InitGoalsUI();
    }

    // Khởi tạo Text cho các mục tiêu
    private void InitGoalsUI()
    {
        foreach (LevelGoal goal in goals)
        {
            UpdateGoalUI(goal);
        }
    }

    // --- LOGIC THU THẬP KẸO ---
    public void CollectCandy(int type)
    {
        if (isGameOver) return;

        // Báo ra Console mỗi khi có 1 viên kẹo bất kỳ bị vỡ
        Debug.Log("💥 Kẹo vừa nổ có ID màu là: " + type);

        foreach (LevelGoal goal in goals)
        {
            if (goal.candyType == type)
            {
                Debug.Log("✅ Nổ TRÚNG kẹo mục tiêu! Bắt đầu trừ số...");

                if (goal.currentAmount < goal.targetAmount)
                {
                    goal.currentAmount++;
                    UpdateGoalUI(goal);
                    CheckWinCondition();
                }
                break;
            }
        }
    }

    // --- LOGIC LƯỢT ĐI ---
    public void UseMove()
    {
        if (isGameOver) return;
        currentMoves--;
        UpdateMovesUI();
    }

    // --- ĐIỀU KIỆN THẮNG ---
    private void CheckWinCondition()
    {
        bool hasWon = true;

        foreach (LevelGoal goal in goals)
        {
            if (goal.currentAmount < goal.targetAmount)
            {
                hasWon = false;
                break;
            }
        }

        if (hasWon)
        {
            isGameOver = true;
            Debug.Log("🎉 CHIẾN THẮNG! Đang chuyển cảnh...");
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayWinSound();
            }
            // Gọi hàm chờ rồi chuyển scene
            StartCoroutine(LoadSceneAfterDelay(winSceneName));
        }
    }

    // --- ĐIỀU KIỆN THUA ---
    public void CheckLossCondition()
    {
        if (isGameOver) return;

        if (currentMoves <= 0)
        {
            isGameOver = true;
            Debug.Log("💀 THUA CUỘC! Đang chuyển cảnh...");
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayLoseSound();
            }
            // Gọi hàm chờ rồi chuyển scene
            StartCoroutine(LoadSceneAfterDelay(loseSceneName));
        }
    }

    // --- COROUTINE CHỜ CHUYỂN SCENE ---
    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        // Chờ một khoảng thời gian (VD: 1.5 giây) để người chơi kịp nhìn thấy kẹo nổ xong
        yield return new WaitForSeconds(delayBeforeTransition);

        // Tải Scene mới
        SceneManager.LoadScene(sceneName);
    }

    // --- CẬP NHẬT GIAO DIỆN ---
    private void UpdateMovesUI()
    {
        if (movesText != null) movesText.text = "Moves: " + currentMoves.ToString();
    }

    private void UpdateGoalUI(LevelGoal goal)
    {
        if (goal.uiText != null)
        {
            // Tính số kẹo còn lại
            int leftToCollect = goal.targetAmount - goal.currentAmount;

            // Đảm bảo số không bao giờ bị âm (nếu ăn lố thì hiện 0)
            leftToCollect = Mathf.Max(0, leftToCollect);

            // Hiển thị lên UI
            goal.uiText.text = leftToCollect.ToString();

            // THÊM ĐÚNG DÒNG NÀY: Ép Unity vẽ lại con số ngay lập tức!
            goal.uiText.ForceMeshUpdate();
        }
    }
}