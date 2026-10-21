using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
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

    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private Transform roomCameraPosition;
    [SerializeField] private Transform roomCenterTracker;
    [SerializeField] private Transform shopCameraPosition;
    [SerializeField] private CinemachineCamera shopSlotCamera;
    [SerializeField] private CinemachineBrain camBrain;

    private Canvas mainCanvas;

    void Awake()
    {
        mainCanvas = GetComponent<Canvas>();
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
        StartCoroutine(OpenShopCoroutine());
    }

    public void CloseShopCanva()
    {
        shopSlotCamera.gameObject.SetActive(false);
        shopSlotCamera.Priority = 0;
        
        shopContainer.anchoredPosition = new(shopContainer.anchoredPosition.x, finalHeight);
        shopContainer.DOAnchorPosY(startHeight, easeTime).SetEase(Ease.InBack, 0.8f)
            .OnComplete(() => { 
                shopCanva.SetActive(false);  
            });

        StartCoroutine(AnimateCloseShopCanva());
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

    private IEnumerator OpenShopCoroutine()
    {
        shopCanva.SetActive(true);

        mainCanvas.enabled = false;
        mainCamera.transform.SetPositionAndRotation(shopCameraPosition.position, shopCameraPosition.rotation);
        mainCamera.Follow = null;

        yield return new WaitForSeconds(0.3f);

        shopSlotCamera.gameObject.SetActive(true);
        shopSlotCamera.Priority = 2;

        shopContainer.anchoredPosition = new(shopContainer.anchoredPosition.x, startHeight);

        StartCoroutine(AnimateOpenShopCanva());
    }

    private IEnumerator AnimateOpenShopCanva()
    {
        yield return null;

        yield return new WaitWhile(() => camBrain.IsBlending);

        shopContainer.DOAnchorPosY(finalHeight, easeTime).SetEase(Ease.OutBounce);
    }

    private IEnumerator AnimateCloseShopCanva()
    {
        yield return null;

        yield return new WaitWhile(() => camBrain.IsBlending);
        yield return new WaitForSeconds(0.3f);

        mainCamera.transform.SetPositionAndRotation(roomCameraPosition.position, roomCameraPosition.rotation); 
        mainCanvas.enabled = true; 
        mainCamera.Follow = roomCenterTracker;
    }
}
