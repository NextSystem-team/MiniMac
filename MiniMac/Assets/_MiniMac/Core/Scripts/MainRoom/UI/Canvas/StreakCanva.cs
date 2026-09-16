using DG.Tweening;
using UnityEngine;

public class StreakCanva : MonoBehaviour
{
    [SerializeField] private GameObject streakScreen;
    
    private RectTransform screenContainer;
    private CanvasGroup group;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
        screenContainer = GetComponent<RectTransform>();

        group.alpha = 0f;
        screenContainer.localScale = new(0.25f, 0.25f, 0.25f);
        streakScreen.SetActive(false);

        OpenStreakScreen();
    }

    public void OpenStreakScreen()
    {

        if (GameManager.Instance.hasAnsweredQuestion && GameManager.Instance.hasPattedPet && GameManager.Instance.hasPlayedMiniGame &&
            !ReportManager.Instance.hasMadeDailyRoutine)
        {
            ReportManager.Instance.hasMadeDailyRoutine = true;

            streakScreen.SetActive(true);
            group.alpha = 0f;
            screenContainer.localScale = new(0.25f, 0.25f, 0.25f);

            group.DOFade(1f, 1f);
            screenContainer.DOScale(Vector3.one, 1.1f).SetEase(Ease.OutBack, 0.8f);
        }
    }

    public void CloseStreakScreen()
    {
        streakScreen.SetActive(false);
    }
}
