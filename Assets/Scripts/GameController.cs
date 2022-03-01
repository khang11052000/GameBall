using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverScreen GameOverScreen;
    private int maxBall;
    public void GameOver()
    {
        GameOverScreen.Setup();
    }

    private void Update()
    {
        // if (GridManager.GetInstance().maxBall > 135)
        // {
        //     GameOver();
        // }
        
    }
}
