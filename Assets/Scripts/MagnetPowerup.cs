using System.Collections;
using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    public float magnetDuration = 5f; // How long the magnet effect lasts
    public float attractionRadius = 10f; // Range of the magnet effect
    public float attractionSpeed = 5f; // Speed of collectible attraction

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ActivateMagnet(other.transform));
        }
    }

    private IEnumerator ActivateMagnet(Transform player)
    {
        GameManager.instance.ActivateMagnet(attractionRadius, attractionSpeed, player);

        // Hide the power-up during activation
        gameObject.SetActive(false);

        // Wait for the magnet duration to end
        yield return new WaitForSeconds(magnetDuration);

        // Deactivate the magnet effect
        GameManager.instance.DeactivateMagnet();

    }

}
