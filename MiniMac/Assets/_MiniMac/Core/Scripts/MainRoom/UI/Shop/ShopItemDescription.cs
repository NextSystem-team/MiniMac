using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDescription : MonoBehaviour
{
    [SerializeField] private GameObject infoContainerTop;
    [SerializeField] private GameObject infoContainerBottom;

    [SerializeField] private SpriteRenderer itemIcon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;
    [SerializeField] private TextMeshPro itemPrice;

    public ShopItem currentShopItem;

    public void SetDescription(ShopItem shopItem)
    {
        itemIcon.sprite = shopItem.itemRendererSprite;
        itemName.text = shopItem.itemName;
        itemDescription.text = shopItem.itemDescription;
        itemPrice.text = shopItem.itemPrice.ToString() + "$";

        currentShopItem = shopItem;

        infoContainerTop.SetActive(true);
        infoContainerBottom.SetActive(true);
    }

    public void BuyItem()
    {
        if (GameManager.Instance.money >= currentShopItem.itemPrice)
        {
            GameManager.Instance.money -= currentShopItem.itemPrice;
            GameManager.Instance.hatsObtained.Add(currentShopItem.itemID);
            
            infoContainerBottom.SetActive(false);
            infoContainerTop.SetActive(false);
            itemIcon.sprite = null;
            itemPrice.text = "";

            transform.parent.GetComponent<ShopCanva>().PopulateShopItems();
            AudioManager.Instance.PlaySFX(SFXType.UI_Item_Buy);
        }
        else
        {
            Debug.Log("Not enough money to buy this item.");
        }
    }
}
