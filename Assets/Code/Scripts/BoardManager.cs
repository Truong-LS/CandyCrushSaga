using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public GameObject[] candyPrefabs;

    public GameObject[,] board;

    //private int score = 0;

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

    //void SpawnCandy(int x, int y)
    //{
    //    int randomIndex = Random.Range(0, candyPrefabs.Length);

    //    GameObject candy = Instantiate(
    //        candyPrefabs[randomIndex],
    //        new Vector3(x, y, 0),
    //        Quaternion.identity
    //    );

    //    Candy candyScript = candy.GetComponent<Candy>();
    //    candyScript.x = x;
    //    candyScript.y = y;
    //    candyScript.candyType = randomIndex;

    //    board[x, y] = candy;
    //}
    void SpawnCandy(int x, int y)
    {
        int randomIndex = Random.Range(0, candyPrefabs.Length);

        // Tránh match ngang
        if (x >= 2)
        {
            int leftType1 = board[x - 1, y]?.GetComponent<Candy>()?.candyType ?? -1;
            int leftType2 = board[x - 2, y]?.GetComponent<Candy>()?.candyType ?? -1;

            if (leftType1 == randomIndex && leftType2 == randomIndex)
            {
                randomIndex = (randomIndex + 1) % candyPrefabs.Length;
            }
        }

        // Tránh match dọc
        if (y >= 2)
        {
            int downType1 = board[x, y - 1]?.GetComponent<Candy>()?.candyType ?? -1;
            int downType2 = board[x, y - 2]?.GetComponent<Candy>()?.candyType ?? -1;

            if (downType1 == randomIndex && downType2 == randomIndex)
            {
                randomIndex = (randomIndex + 1) % candyPrefabs.Length;
            }
        }

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

    //public void CheckMatches()
    //{
    //    bool anyMatch = false;

    //    // Check ngang
    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width - 2; x++)
    //        {
    //            if (board[x, y] != null &&
    //                board[x + 1, y] != null &&
    //                board[x + 2, y] != null)
    //            {
    //                int type = board[x, y].GetComponent<Candy>().candyType;

    //                if (board[x + 1, y].GetComponent<Candy>().candyType == type &&
    //                    board[x + 2, y].GetComponent<Candy>().candyType == type)
    //                {
    //                    DestroyCandy(x, y);
    //                    DestroyCandy(x + 1, y);
    //                    DestroyCandy(x + 2, y);
    //                    anyMatch = true;
    //                }
    //            }
    //        }
    //    }

    //    // Check dọc
    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height - 2; y++)
    //        {
    //            if (board[x, y] != null &&
    //                board[x, y + 1] != null &&
    //                board[x, y + 2] != null)
    //            {
    //                int type = board[x, y].GetComponent<Candy>().candyType;

    //                if (board[x, y + 1].GetComponent<Candy>().candyType == type &&
    //                    board[x, y + 2].GetComponent<Candy>().candyType == type)
    //                {
    //                    DestroyCandy(x, y);
    //                    DestroyCandy(x, y + 1);
    //                    DestroyCandy(x, y + 2);
    //                    anyMatch = true;
    //                }
    //            }
    //        }
    //    }

    //    if (anyMatch)
    //    {
    //        CollapseBoard();
    //        SpawnNewCandies();
    //        CheckMatches(); // combo tiếp
    //    }
    //}
    //public void CheckMatches()
    //{
    //    List<Vector2Int> matchedPositions = new List<Vector2Int>();

    //    // CHECK NGANG
    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width - 2; x++)
    //        {
    //            if (board[x, y] != null &&
    //                board[x + 1, y] != null &&
    //                board[x + 2, y] != null)
    //            {
    //                int type = board[x, y].GetComponent<Candy>().candyType;

    //                if (board[x + 1, y].GetComponent<Candy>().candyType == type &&
    //                    board[x + 2, y].GetComponent<Candy>().candyType == type)
    //                {
    //                    matchedPositions.Add(new Vector2Int(x, y));
    //                    matchedPositions.Add(new Vector2Int(x + 1, y));
    //                    matchedPositions.Add(new Vector2Int(x + 2, y));
    //                }
    //            }
    //        }
    //    }

    //    // CHECK DỌC
    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height - 2; y++)
    //        {
    //            if (board[x, y] != null &&
    //                board[x, y + 1] != null &&
    //                board[x, y + 2] != null)
    //            {
    //                int type = board[x, y].GetComponent<Candy>().candyType;

    //                if (board[x, y + 1].GetComponent<Candy>().candyType == type &&
    //                    board[x, y + 2].GetComponent<Candy>().candyType == type)
    //                {
    //                    matchedPositions.Add(new Vector2Int(x, y));
    //                    matchedPositions.Add(new Vector2Int(x, y + 1));
    //                    matchedPositions.Add(new Vector2Int(x, y + 2));
    //                }
    //            }
    //        }
    //    }

    //    // Loại bỏ trùng
    //    HashSet<Vector2Int> uniqueMatches = new HashSet<Vector2Int>(matchedPositions);

    //    if (uniqueMatches.Count > 0)
    //    {
    //        foreach (var pos in uniqueMatches)
    //        {
    //            DestroyCandy(pos.x, pos.y);
    //        }

    //        CollapseBoard();
    //        SpawnNewCandies();

    //        Invoke(nameof(CheckMatches), 0.2f); // delay nhẹ để nhìn đẹp hơn
    //    }
    //}

    void DestroyCandy(int x, int y)
    {
        if (board[x, y] != null)
        {
            Destroy(board[x, y]);
            board[x, y] = null;

            UIManager.Instance.AddScore(10);
        }
    }

    public int CheckMatches(bool isPlayerMove = false)
    {
        List<Vector2Int> matchedPositions = new List<Vector2Int>();

        // CHECK NGANG
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
                        matchedPositions.Add(new Vector2Int(x, y));
                        matchedPositions.Add(new Vector2Int(x + 1, y));
                        matchedPositions.Add(new Vector2Int(x + 2, y));
                    }
                }
            }
        }

        // CHECK DỌC
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
                        matchedPositions.Add(new Vector2Int(x, y));
                        matchedPositions.Add(new Vector2Int(x, y + 1));
                        matchedPositions.Add(new Vector2Int(x, y + 2));
                    }
                }
            }
        }

        HashSet<Vector2Int> uniqueMatches = new HashSet<Vector2Int>(matchedPositions);

        if (uniqueMatches.Count > 0)
        {
            // 🔥 Trừ move chỉ khi là lượt của người chơi
            if (isPlayerMove)
            {
                UIManager.Instance.UseMove();
            }

            foreach (var pos in uniqueMatches)
            {
                DestroyCandy(pos.x, pos.y);
            }

            CollapseBoard();
            SpawnNewCandies();

            // Combo không trừ move
            Invoke(nameof(CheckMatches), 0.2f);

            return uniqueMatches.Count;
        }

        return 0;
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