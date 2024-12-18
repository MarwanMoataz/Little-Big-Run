using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<ShopItem> shopItems; // Assign ScriptableObjects in Inspector
    public Transform shopContent; // Reference to the Scroll View's Content object
    public GameObject shopItemPrefab; // Assign the ShopItemUI prefab in the Inspector
    public TMPro.TextMeshProUGUI collectiblesText; // UI text to show player's collectibles

    private int playerCollectibles;

    private void Start()
    {
        playerCollectibles = PlayerPrefs.GetInt("TotalCollectibles", 0);
        UpdateCollectiblesUI();
        Debug.Log(shopContent); // Ensure it is not null
        Debug.Log(shopItemPrefab); // Ensure it is not null
        Debug.Log(collectiblesText); // Ensure it is not null

        
        // Populate the shop with items
        foreach (var item in shopItems)
        {
            CreateShopItemUI(item);
        }
    }

    private void UpdateCollectiblesUI()
    {
        collectiblesText.text = $"Orbs: {playerCollectibles}";
    }

    private void CreateShopItemUI(ShopItem item)
    {
        GameObject itemUI = Instantiate(shopItemPrefab, shopContent);
        itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;
        itemUI.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().text = item.price.ToString();
        itemUI.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;

        Button buyButton = itemUI.transform.Find("BuyButton").GetComponent<Button>();
        buyButton.onClick.AddListener(() => AttemptPurchase(item, itemUI));

        int unlocked = PlayerPrefs.GetInt($"Unlocked_{item.itemName}", 0);
        if (unlocked == 1)
        {
            itemUI.transform.Find("BuyButton").GetComponent<Button>().interactable = false;
        }
    }


    private void AttemptPurchase(ShopItem item, GameObject itemUI)
    {
        if (playerCollectibles >= item.price)
        {
            playerCollectibles -= item.price;
            PlayerPrefs.SetInt("TotalCollectibles", playerCollectibles);
            PlayerPrefs.Save();
            UpdateCollectiblesUI();

            Debug.Log($"Purchased {item.itemName}!");
            itemUI.transform.Find("BuyButton").GetComponent<Button>().interactable = false;

            // Unlock the item for future selection
            PlayerPrefs.SetInt($"Unlocked_{item.itemName}", 1);
        }
        else
        {
            Debug.Log($"Not enough collectibles to buy {item.itemName}.");
        }
    }
}
