using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Ball _ballPrefab;
 
    [SerializeField] private Transform _cam;
 
    private Dictionary<Vector2, Tile> _tiles;
    private Dictionary<Vector2, Ball> _ball;
 
    void Start() {
        GenerateGrid();
        GenerateBall();
    }
 
    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
 
 
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
                    
                    spawnedBall.Init();
 
 
                    _ball[new Vector2(x, y)] = spawnedBall;
                }
            }
        }
    }
 
    // public Tile GetTileAtPosition(Vector2 pos) {
    //     if (_tiles.TryGetValue(pos, out var tile)) return tile;
    //     return null;
    // }
}