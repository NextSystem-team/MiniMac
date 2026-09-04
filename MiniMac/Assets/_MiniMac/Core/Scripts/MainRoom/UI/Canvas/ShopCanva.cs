using UnityEngine;
using UnityEngine.UI;

public class ShopCanva : MonoBehaviour
{
    [SerializeField] private ShopItemListSO shopItemListSO;
    [SerializeField] private GameObject shopItemButtonPrefab;
    [SerializeField] private Transform shopItemButtonParent;
    [SerializeField] private ShopItemDescription itemDescription;

    void Start()
    {
        PopulateShopItems();
    }

    public void PopulateShopItems()
    {
        foreach (Transform child in shopItemButtonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem shopItem in shopItemListSO.shopItems)
        {
            if (!GameManager.Instance.CheckIfHasHat(shopItem.itemID))
            {
                GameObject buttonObj = Instantiate(shopItemButtonPrefab, shopItemButtonParent);
                ShopItemButton button = buttonObj.GetComponent<ShopItemButton>();
                button.shopItemSO = shopItem;
                button.itemImage.sprite = shopItem.itemIcon;

                buttonObj.GetComponent<Button>().onClick.AddListener(() => ShowItemDescription(shopItem));
            }
        }
    }

    private void ShowItemDescription(ShopItem shopItem)
    {
        itemDescription.SetDescription(shopItem);
    }
}
