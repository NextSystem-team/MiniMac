using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemID;
    public string itemName;

    [TextArea(2, 3)]
    public string itemDescription;

    public Sprite itemRendererSprite;
    public Sprite itemIcon;
    public int itemPrice;
}

[CreateAssetMenu(fileName = "ShopItemListSO", menuName = "Scriptable Objects/Shop Item List")]
public class ShopItemListSO : ScriptableObject
{
    public List<ShopItem> shopItems;
}
