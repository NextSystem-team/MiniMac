using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainRoomCanva : MonoBehaviour
{
    [SerializeField] private GameObject rankCanva;
    [SerializeField] private GameObject shopCanva;
    [SerializeField] private RectTransform shopContainer;

    [SerializeField] private float easeTime = 0.3f;
    private float startHeight = 2500f;
    private float finalHeight = 0f;

    [SerializeField] private TextMeshProUGUI reportTextUI;
    [SerializeField] private TextMeshProUGUI buttonMoneyDisplay;
    [SerializeField] private TextMeshProUGUI shopMoneyDisplay;
    [SerializeField] private Text rankScoreDisplay;

    [SerializeField] private GameObject reportPanel;

    void Start()
    {
        
    }

    void Update()
    {
        buttonMoneyDisplay.text = GameManager.Instance.money.ToString();
        shopMoneyDisplay.text = GameManager.Instance.money.ToString();
        rankScoreDisplay.text = GameManager.Instance.playerScore.ToString();
    }

    public void OpenRankCanva()
    {
        rankCanva.SetActive(true);
    }

    public void CloseRankCanva()
    {
        rankCanva.SetActive(false);
    }

    public void OpenShopCanva()
    {
        shopCanva.SetActive(true);

        shopContainer.anchoredPosition = new(shopContainer.anchoredPosition.x, startHeight);

        shopContainer.DOAnchorPosY(finalHeight, easeTime).SetEase(Ease.OutBounce);
    }

    public void CloseShopCanva()
    {
        shopContainer.anchoredPosition = new(shopContainer.anchoredPosition.x, finalHeight);

        shopContainer.DOAnchorPosY(startHeight, easeTime).SetEase(Ease.InBack, 0.8f)
            .OnComplete(() => { shopCanva.SetActive(false); });
    }

    public void ChangeToMinigameScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MiniGameScene");
    }   

    public void OpenReportPanel()
    {
        reportPanel.SetActive(true);
    }

    public void CloseReportPanel()
    {
        reportPanel.SetActive(false);
    }

    public void UpdateReportScreen()
    {
        ReportManager.Instance.UpdateReportScreen(reportTextUI);
    }
}
