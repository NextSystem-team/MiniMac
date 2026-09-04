using UnityEngine;
using UnityEngine.UI;

public class ShopItemDescription : MonoBehaviour
{
    [SerializeField] private GameObject infoContainerTop;
    [SerializeField] private GameObject infoContainerBottom;

    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;
    [SerializeField] private Text itemPrice;

    public ShopItem currentShopItem;

    public void SetDescription(ShopItem shopItem)
    {
        itemIcon.sprite = shopItem.itemIcon;
        itemName.text = shopItem.itemName;
        itemDescription.text = shopItem.itemDescription;
        itemPrice.text = shopItem.itemPrice.ToString();

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

            transform.parent.GetComponent<ShopCanva>().PopulateShopItems();
        }
        else
        {
            Debug.Log("Not enough money to buy this item.");
        }
    }
}
