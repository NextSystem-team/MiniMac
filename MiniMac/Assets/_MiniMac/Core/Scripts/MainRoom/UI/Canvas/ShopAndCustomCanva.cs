using UnityEngine;
using UnityEngine.UI;

public class ShopAndCustomCanva : MonoBehaviour
{
    [SerializeField] private RectTransform customPanel;

    [SerializeField] private Button shopButton;
    [SerializeField] private Button customButton;

    public void ShowCustomPanel()
    {
        customPanel.gameObject.SetActive(true);

        customPanel.anchoredPosition = new(0f, customPanel.anchoredPosition.y);

        shopButton.interactable = true;
        customButton.interactable = false;
    }

    public void CloseCustomPanel()
    {
        customPanel.gameObject.SetActive(false);

        customPanel.anchoredPosition = new(1600f, customPanel.anchoredPosition.y);

        shopButton.interactable = false;
        customButton.interactable = true;
    }
}
