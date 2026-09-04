using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int dotsConnected;
    private float timePassed;
    [SerializeField] private Text timer;

    private void Update()
    {
        timePassed += Time.deltaTime;

        timer.text = DisplayTime(timePassed);

        if (dotsConnected >= 2)
        {
            Time.timeScale = 1;

            if (!GameManager.Instance.hasPlayedMiniGame)
            {
                GameManager.Instance.money += 50;
                GameManager.Instance.playerScore += 50;
                GameManager.Instance.hasPlayedMiniGame = true;
            }
            else
            {
                GameManager.Instance.playerScore += 5;
                GameManager.Instance.money += 10;
            }
            
            GameManager.Instance.IncrementPetQuestionChance(40);

            SceneManager.LoadScene("MainRoomScene");
        }
    }

    private string DisplayTime(float time)
    {
        int seconds;
        int minutes;
        int hours;

        seconds = Mathf.FloorToInt(time % 60);
        minutes = Mathf.FloorToInt(time % 3600 / 60);
        hours = Mathf.FloorToInt(time / 3600);

        string timer;

        if (hours <= 0)
        {
            timer = minutes.ToString("0") + ":" + seconds.ToString("00");
        }
        else
        {
            timer = hours.ToString() + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        return timer;
    }
}
