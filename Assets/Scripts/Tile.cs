using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private Vector2 pos;

    public bool nextTurn = false;
 
    public void Init(Vector2 _pos, bool isOffset) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        pos = _pos;
    }

    private void Update()
    {
        if (nextTurn == true)
        {
            GridManager.GetInstance().GenerateBallNextTurn();
            nextTurn = false;
            // Debug.Log("1");
        }
    }

    void OnMouseEnter() {
        //_highlight.SetActive(true);

        if (GridManager.GetInstance().startPos == new Vector2(-1, -1))
        {
            return;
        }
        
        // clear path
        GridManager.GetInstance().ClearHighlight();

        bool hasPath = GridManager.GetInstance().finder.FindPath(GridManager.GetInstance().startPos, pos);

        if (!hasPath)
        {
            return;
        }
        
        foreach (var node in GridManager.GetInstance().path)
        {
            node.tile.SetHighlight(true);
        }
    }
 
    void OnMouseExit()
    {
        // _highlight.SetActive(false);
    }

    public void SetHighlight(bool val)
    {
        _highlight.SetActive(val);
    }
    
    private void OnMouseUp()
    {
        // First click
        if (GridManager.GetInstance().startPos == new Vector2(-1, -1))
        {
            if (GridManager.GetInstance().GetBallAtPosition(pos) != null)
            {
                GridManager.GetInstance().startPos = pos;
                // Debug.Log(GridManager.GetInstance().GetBallAtPosition(pos).name);
            }
        }
        else
        {
            // Second click
            if (GridManager.GetInstance().GetBallAtPosition(pos) != null)
            {
                return;
            }



            bool hasPath = GridManager.GetInstance().finder.FindPath(GridManager.GetInstance().startPos, pos);
                
            if (hasPath)
            {
                GridManager.GetInstance().endPos = pos;

                GridManager.GetInstance().MoveBallToNewPos(GridManager.GetInstance().startPos, GridManager.GetInstance().endPos);


                var lines = GridManager.GetInstance().CheckLine((int) pos.x, (int) pos.y);
                foreach (var line in lines)
                {
                 
                    
                    Debug.Log($"Line: {line.GridX} {line.GridY}");

                    // GridManager.GetInstance().DestroyBall((int) line.GridX, (int) line.GridY);
                }
                

                GridManager.GetInstance().startPos = new Vector2(-1, -1);
                GridManager.GetInstance().endPos = new Vector2(-1, -1);
                
                GridManager.GetInstance().ClearHighlight();

                nextTurn = true;
            }
        }


    }

    
}