using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Ball _ballPrefab;
 
    [SerializeField] private Transform _cam;
 
    private Dictionary<Vector2, Tile> _tiles;
    private Dictionary<Vector2, Ball> _ball;

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
 
    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(new Vector2(x,y),isOffset);
 
 
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
 
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 + 0.2f,-10);
    }

    void GenerateBall()
    {
        _ball = new Dictionary<Vector2, Ball>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++)
            {
                var randomBall = Random.Range(0, 6);
                if (randomBall == 3)
                {
                    var spawnedBall = Instantiate(_ballPrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedBall.name = $"Ball {x} {y}";
                    
                    spawnedBall.Init(new Vector2(x, y));
 
 
                    _ball[new Vector2(x, y)] = spawnedBall;
                }
            }
        }
    }
 
    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
    
    public Ball GetBallAtPosition(Vector2 pos) {
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
        }
    }
}