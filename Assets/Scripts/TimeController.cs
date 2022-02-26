using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    public Text timeCounter;

    private TimeSpan _timePlaying;
    private bool _timerGoing;

    private float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter.text = "Time: 00 : 00 . 00";
        //_timerGoing = false;
        BeginTimer();
    }

    public void BeginTimer()
    {
        _timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        _timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (_timerGoing)
        {
            elapsedTime += Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + _timePlaying.ToString("mm' : 'ss' . 'ff");
            timeCounter.text = timePlayingStr;
        
            yield return null;
        }
    }
}