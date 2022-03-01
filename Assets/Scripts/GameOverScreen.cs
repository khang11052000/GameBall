using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    //public Text pointsText;
    public Text timerText;

    public void Setup()
    {
        gameObject.SetActive(true);
        TimeController.instance.EndTimer();
        //pointsText.text = score.ToString() + " POINTS";
        timerText.text = TimeController.instance.timeCounter.text;
    }
}
