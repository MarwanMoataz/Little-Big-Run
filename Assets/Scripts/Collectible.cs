using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool isCollected = false;
    private Transform magnetTarget;
    private float magnetSpeed = 0f;
    public float rotationSpeed = 50f;
    private void Update()
    {
        RotateCollectible();

        // Move toward the player if the magnet is active
        if (magnetTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, magnetTarget.position, magnetSpeed * Time.deltaTime);
        }
    }
    private void RotateCollectible()
    {
        // Rotate the collectible around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect(other.transform);
        }
    }

    public void EnableMagnet(Transform playerTransform, float speed)
    {
        magnetTarget = playerTransform;
        magnetSpeed = speed;
    }

    public void DisableMagnet()
    {
        magnetTarget = null;
        magnetSpeed = 0f;
    }

    private void Collect(Transform playerTransform)
    {
        isCollected = true;

        GameManager.instance.UpdateCollectibles(1);

        int totalCollectibles = PlayerPrefs.GetInt("TotalCollectibles", 0);
        totalCollectibles++;
        PlayerPrefs.SetInt("TotalCollectibles", totalCollectibles);
        PlayerPrefs.Save();

        gameObject.SetActive(false);

        AudioManager.instance.PlayNextCollectibleSound();
    }
}
