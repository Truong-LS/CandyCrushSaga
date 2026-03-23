using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Danh sách các nút bấm màn chơi")]
    // Tạo một mảng chứa tất cả các nút Level 1, Level 2, Level 3...
    public Button[] levelButtons;

    void Start()
    {
        // 1. Mở sổ ra xem người chơi đã mở khóa đến màn mấy (Mặc định là 1)
        int unlockedLevel = PlayerPrefs.GetInt("SavedLevel", 1);

        // 2. Quét qua toàn bộ danh sách nút bấm
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // Trong lập trình, mảng bắt đầu từ 0. 
            // Nên i = 0 là Nút bài 1, i = 1 là Nút bài 2...
            int levelNumber = i + 1;

            // Nếu số thứ tự của màn lớn hơn số màn đã mở khóa -> KHÓA NÚT
            if (levelNumber > unlockedLevel)
            {
                levelButtons[i].interactable = false; // Tắt tính năng bấm
            }
            // Nếu nhỏ hơn hoặc bằng -> MỞ KHÓA
            else
            {
                levelButtons[i].interactable = true;  // Bật tính năng bấm
            }
        }
    }
}
