using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool _gamePlaying;
    void Start()
    {
        BeginGame();
    }

    public void BeginGame()
    {
        _gamePlaying = true;
        
        TimeController.instance.BeginTimer();
    }
}
