using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public GameObject[] candyPrefabs;

    public GameObject[,] board;

    private int score = 0;

    void Start()
    {
        board = new GameObject[width, height];
        CreateBoard();
        Debug.Log("Board Created");
    }

    void CreateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnCandy(x, y);
            }
        }
    }

    void SpawnCandy(int x, int y)
    {
        int randomIndex = Random.Range(0, candyPrefabs.Length);

        GameObject candy = Instantiate(
            candyPrefabs[randomIndex],
            new Vector3(x, y, 0),
            Quaternion.identity
        );

        Candy candyScript = candy.GetComponent<Candy>();
        candyScript.x = x;
        candyScript.y = y;
        candyScript.candyType = randomIndex;

        board[x, y] = candy;
    }

    public void CheckMatches()
    {
        bool anyMatch = false;

        // Check ngang
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                if (board[x, y] != null &&
                    board[x + 1, y] != null &&
                    board[x + 2, y] != null)
                {
                    int type = board[x, y].GetComponent<Candy>().candyType;

                    if (board[x + 1, y].GetComponent<Candy>().candyType == type &&
                        board[x + 2, y].GetComponent<Candy>().candyType == type)
                    {
                        DestroyCandy(x, y);
                        DestroyCandy(x + 1, y);
                        DestroyCandy(x + 2, y);
                        anyMatch = true;
                    }
                }
            }
        }

        // Check dọc
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                if (board[x, y] != null &&
                    board[x, y + 1] != null &&
                    board[x, y + 2] != null)
                {
                    int type = board[x, y].GetComponent<Candy>().candyType;

                    if (board[x, y + 1].GetComponent<Candy>().candyType == type &&
                        board[x, y + 2].GetComponent<Candy>().candyType == type)
                    {
                        DestroyCandy(x, y);
                        DestroyCandy(x, y + 1);
                        DestroyCandy(x, y + 2);
                        anyMatch = true;
                    }
                }
            }
        }

        if (anyMatch)
        {
            CollapseBoard();
            SpawnNewCandies();
            CheckMatches(); // combo tiếp
        }
    }

    void DestroyCandy(int x, int y)
    {
        if (board[x, y] != null)
        {
            Destroy(board[x, y]);
            board[x, y] = null;

            score += 10;
            Debug.Log("Score: " + score);
        }
    }

    void CollapseBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                if (board[x, y] == null)
                {
                    for (int k = y + 1; k < height; k++)
                    {
                        if (board[x, k] != null)
                        {
                            board[x, y] = board[x, k];
                            board[x, k] = null;

                            board[x, y].transform.position = new Vector3(x, y, 0);

                            Candy candyScript = board[x, y].GetComponent<Candy>();
                            candyScript.y = y;

                            break;
                        }
                    }
                }
            }
        }
    }

    void SpawnNewCandies()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] == null)
                {
                    SpawnCandy(x, y);
                }
            }
        }
    }
}