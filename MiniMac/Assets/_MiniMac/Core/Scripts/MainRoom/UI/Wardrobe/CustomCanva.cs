using UnityEngine;
using UnityEngine.UI;

public class CustomCanva : MonoBehaviour
{
    [SerializeField] private Image hatImage;
    [SerializeField] private SpriteRenderer hatRenderer;
    [SerializeField] private GameObject wardrobePanel;

    [SerializeField] private GameObject wardrobeItemPrefab;
    [SerializeField] private Transform wardrobeItemParent;

    [SerializeField] private ShopItemListSO shopItemListSO;

    private void Start()
    {
        hatImage.sprite = GameManager.Instance.currentHat != null ? GameManager.Instance.currentHat.itemIcon : null;
        hatImage.color = GameManager.Instance.currentHat.itemIcon != null ? Color.white : Color.clear;

        PopulateWardrobe();
    }

    private void PopulateWardrobe()
    {
        foreach (Transform child in wardrobeItemParent)
        {
            Destroy(child.gameObject);
        }

        // Dequip item button
        GameObject dequipButton = Instantiate(wardrobeItemPrefab, wardrobeItemParent);
        CustomItemButton dequipButtonItem = dequipButton.GetComponent<CustomItemButton>();
        dequipButtonItem.shopItemSO = null; 
        dequipButtonItem.itemImage.color = Color.clear; 
        dequipButton.GetComponent<Button>().onClick.AddListener(() => EquipHat(null));

        foreach (var item in shopItemListSO.shopItems)
        {
            if (GameManager.Instance.CheckIfHasHat(item.itemID))
            {
                GameObject wardrobeItem = Instantiate(wardrobeItemPrefab, wardrobeItemParent);
                CustomItemButton button = wardrobeItem.GetComponent<CustomItemButton>();
                button.shopItemSO = item;
                button.itemImage.sprite = item.itemIcon;

                button.GetComponent<Button>().onClick.AddListener(() => EquipHat(item));
            }
        }
    }

    private void EquipHat(ShopItem item)
    {
        if (item != null)
        {
            hatRenderer.sprite = item.itemRendererSprite;
            hatImage.sprite = item.itemIcon;
            hatImage.color = Color.white;
            GameManager.Instance.currentHat = item;
        }
        else
        {
            hatRenderer.sprite = null;
            hatImage.sprite = null;
            hatImage.color = Color.clear;
            GameManager.Instance.currentHat = null;
        }

        CloseWardrobePanel();
    }

    public void ShowWardrobePanel()
    {
        PopulateWardrobe();
        wardrobePanel.SetActive(true);
    }

    public void CloseWardrobePanel()
    {
        wardrobePanel.SetActive(false);
    }
}
