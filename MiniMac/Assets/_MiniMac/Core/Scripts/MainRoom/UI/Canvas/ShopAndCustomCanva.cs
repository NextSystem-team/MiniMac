using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopAndCustomCanva : MonoBehaviour
{
    [SerializeField] private RectTransform customPanel;

    private float startPosition = 1600f;
    private float endPosition = 0f;
    [SerializeField] private float easeTime = 0.3f;

    [SerializeField] private Button shopButton;
    private SodaChangerButtonBehaviour shopChanger;
    [SerializeField] private Button customButton;
    private SodaChangerButtonBehaviour customChanger;

    void Start()
    {
        shopChanger = shopButton.GetComponent<SodaChangerButtonBehaviour>();
        customChanger = customButton.GetComponent<SodaChangerButtonBehaviour>();

        StartCoroutine(StartWithShopOpen());
    }

    public void ShowCustomPanel()
    {
        customPanel.gameObject.SetActive(true);

        customPanel.DOAnchorPosX(endPosition, easeTime).SetEase(Ease.OutSine);

        shopChanger.SwitchState(ButtonStates.notPressed);
        customChanger.SwitchState(ButtonStates.pressed);
    }

    public void CloseCustomPanel()
    {
        customPanel.DOAnchorPosX(startPosition, easeTime).SetEase(Ease.InSine)
            .OnComplete(() => { customPanel.gameObject.SetActive(false); });

        shopChanger.SwitchState(ButtonStates.pressed);
        customChanger.SwitchState(ButtonStates.notPressed);
    }

    private IEnumerator StartWithShopOpen()
    {
        yield return new WaitForEndOfFrame();
        shopChanger.SwitchState(ButtonStates.pressed);
        customChanger.SwitchState(ButtonStates.notPressed);
    }
}
