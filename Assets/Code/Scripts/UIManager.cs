using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI targetText;

    private int score = 0;
    private int moves = 30;
    private int target = 500;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate UIManager destroyed: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("UIManager Awake: " + gameObject.name);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("UIManager score = " + score);
        UpdateUI();
    }

    public void UseMove()
    {
        moves--;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        movesText.text = "Moves: " + moves;
        targetText.text = "Target: " + target;
    }
}