using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string itemName;  // Name of the item
    public int price;        // Price of the item in collectibles
    public GameObject model; // 3D model of the item
    public Sprite icon;      // Icon for UI display
}
