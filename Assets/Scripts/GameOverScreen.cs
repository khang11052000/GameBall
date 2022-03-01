using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButtoon()
    {
        Application.Quit();
    }
}
