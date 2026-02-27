using UnityEngine;

public class Candy : MonoBehaviour
{
    public int candyType;
    public int x;
    public int y;

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isSelected = false;

    private static Candy firstSelected;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        Debug.Log("Candy Ready: " + gameObject.name);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            if (hit != null && hit.gameObject == this.gameObject)
            {
                Debug.Log("Clicked: " + candyType);
                ToggleSelect();
            }
        }
    }

    void ToggleSelect()
    {
        if (firstSelected == null)
        {
            firstSelected = this;
            sr.color = Color.gray;
            isSelected = true;
        }
        else if (firstSelected == this)
        {
            sr.color = originalColor;
            isSelected = false;
            firstSelected = null;
        }
        else
        {
            if (IsAdjacent(firstSelected) && candyType != firstSelected.candyType)
            {
                SwapCandy(firstSelected);
                FindObjectOfType<BoardManager>().CheckMatches();
            }

            firstSelected.sr.color = firstSelected.originalColor;
            firstSelected.isSelected = false;
            firstSelected = null;
        }
    }

    bool IsAdjacent(Candy other)
    {
        return (Mathf.Abs(x - other.x) == 1 && y == other.y) ||
               (Mathf.Abs(y - other.y) == 1 && x == other.x);
    }

    void SwapCandy(Candy other)
    {
        BoardManager board = FindObjectOfType<BoardManager>();

        // đổi trong mảng board
        GameObject temp = board.board[x, y];
        board.board[x, y] = board.board[other.x, other.y];
        board.board[other.x, other.y] = temp;

        // đổi vị trí hiển thị
        Vector3 tempPos = other.transform.position;
        other.transform.position = transform.position;
        transform.position = tempPos;

        // đổi tọa độ logic
        int tempX = other.x;
        int tempY = other.y;

        other.x = x;
        other.y = y;

        x = tempX;
        y = tempY;

        Debug.Log("Swapped!");
    }
}