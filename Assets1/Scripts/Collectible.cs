using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Update the collectible count in GameManager
            GameManager.instance.UpdateCollectibles(1);

            // Save the total collectible count globally
            int totalCollectibles = PlayerPrefs.GetInt("TotalCollectibles", 0);
            totalCollectibles++;
            PlayerPrefs.SetInt("TotalCollectibles", totalCollectibles);
            PlayerPrefs.Save();

            // Hide the collectible
            gameObject.SetActive(false);

            // Play the collectible sound
            AudioManager.instance.PlayNextCollectibleSound();
        }
    }

}
