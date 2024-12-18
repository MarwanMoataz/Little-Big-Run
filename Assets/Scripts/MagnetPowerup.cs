using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(ActivateMagnet(other.gameObject));
        }
    }

    private IEnumerator ActivateMagnet(GameObject player)
    {
        GameManager.instance.ActivateMagnet(attractionRadius, attractionSpeed);

        // Hide the power-up during activation
        gameObject.SetActive(false);

        yield return new WaitForSeconds(magnetDuration);

        // Deactivate the magnet effect
        GameManager.instance.DeactivateMagnet();
    }
}
