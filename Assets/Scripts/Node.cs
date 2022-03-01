using UnityEngine;

public class Node
{
    public int gCost, hCost;
    public bool obstacle;
    public Color color { get; set; }
    public Tile tile;
    public int GridX, GridY;
    public Node parent;
    
    public Node(bool _obstacle, Tile _tile, int _gridX, int _gridY)
    {
        obstacle = _obstacle;
        GridX = _gridX;
        GridY = _gridY;
        tile = _tile;
        //color = _color;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }

    }
    

    public void SetObstacle(bool isOb)
    {
        obstacle = isOb;
    }
}