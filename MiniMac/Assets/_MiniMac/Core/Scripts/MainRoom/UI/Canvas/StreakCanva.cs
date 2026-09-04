using UnityEngine;

public class StreakCanva : MonoBehaviour
{
    [SerializeField] private GameObject streakScreen;

    void Start()
    {
        OpenStreakScreen();
    }

    public void OpenStreakScreen()
    {

        if (GameManager.Instance.hasAnsweredQuestion && GameManager.Instance.hasPattedPet && GameManager.Instance.hasPlayedMiniGame && 
            !ReportManager.Instance.hasMadeDailyRoutine)
        {
            ReportManager.Instance.hasMadeDailyRoutine = true;  
            streakScreen.SetActive(true);
        }
    }

    public void CloseStreakScreen()
    {
        streakScreen.SetActive(false);
    }
}
