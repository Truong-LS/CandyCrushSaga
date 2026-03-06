using UnityEngine;

public enum SpecialType { None, Horizontal, Vertical, Wrapped, ColorBomb }

public class Candy : MonoBehaviour
{
    public SpecialType specialType = SpecialType.None;

    [Header("Candy Data")]
    public int xIndex;
    public int yIndex;
    public int candyType;
    public bool isMatched = false;

    [Header("Special Sprites (Kéo thả ảnh vào đây)")]
    public Sprite horizontalSprite;
    public Sprite verticalSprite;
    public Sprite wrappedSprite;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    private Vector2 targetPosition;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Hàm này được BoardManager gọi khi vừa sinh viên kẹo ra
    public void Init(int x, int y, int type)
    {
        xIndex = x;
        yIndex = y;
        candyType = type;

        // Đặt vị trí ban đầu
        transform.position = new Vector2(x, y);
        targetPosition = transform.position;
    }

    void Update()
    {
        // Liên tục kiểm tra vị trí đích
        targetPosition = new Vector2(xIndex, yIndex);

        // Nếu vị trí hiện tại khác vị trí đích -> Di chuyển mượt mà (Lerp) tới đích
        if (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Đảm bảo kẹo nằm chính xác ở tọa độ nguyên khi đã đến nơi
            transform.position = targetPosition;
        }
    }

    // Hàm cập nhật tọa độ logic (Được gọi từ BoardManager khi Swap hoặc rơi xuống)
    public void UpdatePosition(int newX, int newY)
    {
        xIndex = newX;
        yIndex = newY;
        // Việc di chuyển hình ảnh sẽ được tự động lo bởi hàm Update() bên trên
    }

    // THÊM HÀM NÀY: Để đổi hình ảnh khi thành kẹo đặc biệt
    public void UpgradeToSpecial(SpecialType newSpecialType)
    {
        specialType = newSpecialType;

        if (spriteRenderer != null)
        {
            switch (specialType)
            {
                case SpecialType.Horizontal:
                    if (horizontalSprite != null) spriteRenderer.sprite = horizontalSprite;
                    break;
                case SpecialType.Vertical:
                    if (verticalSprite != null) spriteRenderer.sprite = verticalSprite;
                    break;
                case SpecialType.Wrapped:
                    if (wrappedSprite != null) spriteRenderer.sprite = wrappedSprite;
                    break;
            }
        }
    }

    // Hàm đổi màu khi người chơi chạm vào (Được gọi từ InputManager)
    public void SetSelected(bool isSelected)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isSelected ? Color.gray : originalColor;
        }
    }
}