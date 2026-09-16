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

        shopButton.interactable = true;
        shopChanger.SwitchState(ButtonStates.pressed);
        customButton.interactable = false;
        customChanger.SwitchState(ButtonStates.notPressed);
    }

    public void CloseCustomPanel()
    {
        customPanel.DOAnchorPosX(startPosition, easeTime).SetEase(Ease.InSine)
            .OnComplete(() => { customPanel.gameObject.SetActive(false); });

        shopButton.interactable = false;
        shopChanger.SwitchState(ButtonStates.notPressed);
        customButton.interactable = true;
        customChanger.SwitchState(ButtonStates.pressed);
    }

    private IEnumerator StartWithShopOpen()
    {
        yield return new WaitForEndOfFrame();
        shopButton.interactable = true;
        shopChanger.SwitchState(ButtonStates.pressed);
        customButton.interactable = false;
        customChanger.SwitchState(ButtonStates.notPressed);
    }
}
