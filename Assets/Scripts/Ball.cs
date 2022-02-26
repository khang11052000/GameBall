using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color _redColor, _bluColor, _yellowColor;
    [SerializeField] private SpriteRenderer _renderer;

    public void Init()
    {
        int randomColor = Random.Range(0, 3);
        if (randomColor == 0)
        {
            _renderer.color = _bluColor;
        }
        else if (randomColor == 1)
        {
            _renderer.color = _redColor;
        }
        else
        {
            _renderer.color = _yellowColor;
        }
    }
}
