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
        screenContainer = streakScreen.GetComponent<RectTransform>();

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

            group.DOFade(1f, 0.7f);
            screenContainer.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack, 0.8f);
        }
    }   

    public void CloseStreakScreen()
    {
        group.DOFade(0f, 0.5f);
        screenContainer.DOScale(new Vector3(0.25f, 0.25f, 0.25f), 0.4f).SetEase(Ease.InBack, 0.8f)
            .OnComplete(()=>{ streakScreen.SetActive(false); });
    }
}
