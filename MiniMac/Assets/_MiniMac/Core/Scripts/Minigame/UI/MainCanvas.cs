using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanva : MonoBehaviour
{
    [SerializeField] private LineManager lineManager;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private RectTransform introPanel;
    [SerializeField] private RectTransform congratsPanel;
    [SerializeField] private Image congratsImage;
    [SerializeField] private Sprite successTaskImage;

    private CanvasGroup pauseGroup;
    private CanvasGroup popUpGroup;
    private CanvasGroup introGroup;
    private CanvasGroup congratsGroup;

    void Start()
    {
        pauseGroup = pausePanel.GetComponent<CanvasGroup>();
        popUpGroup = popUpPanel.GetComponent<CanvasGroup>();
        introGroup = introPanel.GetComponent<CanvasGroup>();
        congratsGroup = congratsPanel.GetComponent<CanvasGroup>();

        StartCoroutine(StartMinigame());
    }

    public void TogglePausePanel()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        pausePanel.SetActive(!pausePanel.activeSelf);

        if (Time.timeScale == 0)
        {
            pausePanel.SetActive(true);
            pauseGroup.DOFade(1f, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
        }
        else
        {
            pauseGroup.DOFade(0f, 0.3f).SetEase(Ease.Linear).SetUpdate(true)
                .OnComplete(() => { pausePanel.SetActive(false); });
        }

        lineManager.canCreateLine = !lineManager.canCreateLine;
    }

    public void OpenIntroPanel()
    {
        popUpPanel.SetActive(true);
        introPanel.gameObject.SetActive(true);

        popUpGroup.DOFade(1f, 0.3f).SetEase(Ease.Linear);
        introGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);

        introPanel.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack);
    }

    public void CloseIntroPanel()
    {
        introPanel.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.4f).SetEase(Ease.InBack, 1.7f);
        introGroup.DOFade(1f, 0.45f).SetEase(Ease.Linear).OnComplete(() => { introPanel.gameObject.SetActive(false); });
        popUpGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() => { popUpPanel.SetActive(false); });
    }

    public void OpenCongratsPanel()
    {
        if (popUpPanel.activeSelf) return;

        popUpPanel.SetActive(true);
        congratsPanel.gameObject.SetActive(true);

        popUpGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
        congratsGroup.DOFade(1f, 0.6f).SetEase(Ease.Linear);

        congratsPanel.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                RectTransform imageRect = congratsImage.GetComponent<RectTransform>();

                congratsImage.sprite = successTaskImage;
                congratsImage.color = Color.green;

                imageRect.localScale = new(1.6f,1.6f,1.6f);
                imageRect.DOScale(Vector3.one, 0.8f).SetEase(Ease.InOutBack);
            });
    }

    public void CloseCongratsPanel()
    {
        congratsGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
        congratsPanel.DOScale(Vector3.one * 0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            DOTween.KillAll();
            SceneManager.LoadScene("MainRoomScene");
        });
    }

    private IEnumerator StartMinigame()
    {
        yield return new WaitForEndOfFrame();

        OpenIntroPanel();
    }
}
