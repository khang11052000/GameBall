using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private Vector2 pos;
 
    public void Init(Vector2 _pos, bool isOffset) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        pos = _pos;
    }
 
    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
    
    private void OnMouseUp()
    {
        if (GridManager.GetInstance().startPos == new Vector2(-1, -1))
        {
            if (GridManager.GetInstance().GetBallAtPosition(pos) != null)
            {
                GridManager.GetInstance().startPos = pos;
            }
        }
        else
        {
            if (GridManager.GetInstance().GetBallAtPosition(pos) != null)
            {
                return;
            }
            
            GridManager.GetInstance().endPos = pos;

            GridManager.GetInstance().MoveBallToNewPos(GridManager.GetInstance().startPos, GridManager.GetInstance().endPos);

            GridManager.GetInstance().startPos = new Vector2(-1, -1);
            GridManager.GetInstance().endPos = new Vector2(-1, -1);
        }


    }
}