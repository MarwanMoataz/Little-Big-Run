using UnityEngine;
using TMPro;

public class TotalCollectiblesDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalCollectiblesText; // Reference to UI TextMeshPro

    private void Start()
    {
        if (totalCollectiblesText == null)
        {
            Debug.LogError("TotalCollectiblesText is not assigned in the Inspector.");
            return;
        }

        // Get total collectibles from PlayerPrefs
        int totalCollectibles = PlayerPrefs.GetInt("TotalCollectibles", 0);

        // Display total collectibles
        totalCollectiblesText.text = $"Total Orbs: {totalCollectibles}";
    }
}
