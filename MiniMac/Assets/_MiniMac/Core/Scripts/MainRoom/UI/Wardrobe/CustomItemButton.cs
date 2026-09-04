using UnityEngine;
using UnityEngine.UI;

public class CustomItemButton : MonoBehaviour
{
    public ShopItem shopItemSO;
    public Image itemImage;

    void Awake()
    {
        if (shopItemSO != null)
        {
            itemImage.sprite = shopItemSO.itemIcon;
        }
    }
}
