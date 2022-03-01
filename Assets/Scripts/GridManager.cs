using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;

    private int maxBall = 0;
    
    public GameOverScreen GameOverScreen;

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Ball _ballPrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;
    private Dictionary<Vector2, Ball> _ball;

    public Node[,] Grid;
    public List<Node> path;

    public Pathfinding2D finder;

    public Vector2 startPos;
    public Vector2 endPos;

    void Start()
    {
        instance = this;
        GenerateGrid();
        GenerateBall();


        startPos = new Vector2(-1, -1);
        endPos = new Vector2(-1, -1);
    }

    public static GridManager GetInstance()
    {
        return instance;
    }

    public void ClearHighlight()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Grid[x, y].tile.SetHighlight(false);
            }
        }
    }

    void GenerateGrid()
    {
        Grid = new Node[_width, _height];


        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //var colorBall = Ball.
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(new Vector2(x, y), isOffset);

                Grid[x, y] = new Node(false, spawnedTile, x, y);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 + 0.2f, -10);
    }

    void GenerateBall()
    {
        _ball = new Dictionary<Vector2, Ball>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var randomBall = Random.Range(0, 6);
                if (randomBall == 3)
                {
                    maxBall++;
                    var spawnedBall = Instantiate(_ballPrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedBall.name = $"Ball {x} {y}";

                    var color = spawnedBall.Init(new Vector2(x, y));

                    Grid[x, y].SetObstacle(true);
                    Grid[x, y].color = color;


                    _ball[new Vector2(x, y)] = spawnedBall;
                }
            }
        }
    }

    public void GenerateBallNextTurn()
    {
        int count = 0;
        while (true)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (Grid[x, y].obstacle)
                        continue;

                    var randomBall = Random.Range(0, 20);
                    if (count < 3)
                    {
                        if (randomBall == 3)
                        {
                            // Debug.Log(new Vector2(x, y));
                            maxBall++;
                            var spawnedBall = Instantiate(_ballPrefab, new Vector3(x, y), Quaternion.identity);
                            spawnedBall.name = $"Ball New {x} {y}";

                            var color = spawnedBall.Init(new Vector2(x, y));

                            Grid[x, y].SetObstacle(true);
                            Grid[x, y].color = color;


                            _ball[new Vector2(x, y)] = spawnedBall;
                            count++;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public Ball GetBallAtPosition(Vector2 pos)
    {
        if (_ball.TryGetValue(pos, out var ball)) return ball;
        return null;
    }

    public void MoveBallToNewPos(Vector2 from, Vector2 to)
    {
        if (_ball.TryGetValue(from, out var ball))
        {
            ball.transform.position = new Vector3(to.x, to.y);
            _ball.Add(to, ball);
            _ball.Remove(from);

            // TODO: set obstacle here
            Grid[(int) from.x, (int) from.y].SetObstacle(false);
            Grid[(int) to.x, (int) to.y].SetObstacle(true);
        }
    }


    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        //checks and adds top neighbor
        if (node.GridX >= 0 && node.GridX < _width && node.GridY + 1 >= 0 && node.GridY + 1 < _height)
            neighbors.Add(Grid[node.GridX, node.GridY + 1]);

        //checks and adds bottom neighbor
        if (node.GridX >= 0 && node.GridX < _width && node.GridY - 1 >= 0 && node.GridY - 1 < _height)
            neighbors.Add(Grid[node.GridX, node.GridY - 1]);

        //checks and adds right neighbor
        if (node.GridX + 1 >= 0 && node.GridX + 1 < _width && node.GridY >= 0 && node.GridY < _height)
            neighbors.Add(Grid[node.GridX + 1, node.GridY]);

        //checks and adds left neighbor
        if (node.GridX - 1 >= 0 && node.GridX - 1 < _width && node.GridY >= 0 && node.GridY < _height)
            neighbors.Add(Grid[node.GridX - 1, node.GridY]);
        return neighbors;
    }

    public bool IsInside(int i, int j)
    {
        return (i >= 0 && i < _width && j >= 0 && j < _height);
    }

    public List<Node> CheckLine(int iCenter, int jCenter)
    {
        
        List<Node> lines = new List<Node>();


        int[] u = {0, 1, 1, 1};
        int[] v = {1, 0, -1, 1};
        int i, j, k, t, count;
        count = 0;

        for (t = 0; t < 4; t++)
        {
            k = 0;
            i = iCenter;
            j = jCenter;
            while (true)
            {
                i += u[t];
                j += v[t];
                if (!IsInside(i, j))
                    break;
                if (Grid[i, j] != Grid[iCenter, jCenter])
                    break;
                k++;
            }

            i = iCenter;
            j = jCenter;
            while (true)
            {
                i -= u[t];
                j -= v[t];
                if (!IsInside(i, j))
                    break;
                if (Grid[i, j].color != Grid[iCenter, jCenter].color)
                    break;
                k++;
            }

            k++;
            if (k >= 5)
            {
                while (k-- > 0)
                {
                    i += u[t];
                    j += v[t];
                    if (i != iCenter || j != jCenter)
                    {
                        // lines.point[count].x = i;
                        // lines.point[count].y = j;

                        lines.Add(Grid[i, j]);

                        count++;
                    }
                }
            }
        }
        //Debug.Log(Grid[iCenter, jCenter].color);
        //Debug.Log($"Line: {line.GridX} {line.GridY}");

        if (count > 0)
        {
            lines.Add(Grid[iCenter, jCenter]);
            //Debug.Log("hihi");
            
        }

        return lines;
    }

    private void Update()
    {
        if (maxBall > 32)
        {
            GameOverScreen.Setup();
        }
        //Debug.Log(maxBall);
    }


    public void DestroyBall(int x, int y)
    {
        Destroy(_ball[new Vector2(x, y)].gameObject);
    }
}