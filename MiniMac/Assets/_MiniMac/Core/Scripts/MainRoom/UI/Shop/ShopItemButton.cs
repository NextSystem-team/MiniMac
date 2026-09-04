using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public ShopItem shopItemSO;
    public Image itemImage;

    void Awake()
    {
        itemImage.sprite = shopItemSO.itemIcon;
    }
}
