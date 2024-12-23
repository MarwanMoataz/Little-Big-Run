using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;        // UI Text for score
    public TextMeshProUGUI collectiblesText; // UI Text for collectibles

    private int sessionScore = 0;           // Session-based score
    private int sessionCollectibles = 0;    // Session-based collectible count
    private float elapsedTime = 0f;         // Elapsed time for time-based scoring
    private GameObject currentCharacter;    // Current character instance
    public Transform playerSpawnPoint;      // Spawn point for the player
    private bool magnetActive = false;
    private float magnetRange = 0f;
    private float magnetSpeed = 0f;

    private Transform playerTransform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        string defaultCharacter = "Sackboy";
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Sackboy")))
        {
            PlayerPrefs.SetString("Sackboy", defaultCharacter);
            PlayerPrefs.Save();
        }
        // Initialize session collectibles and score display
        UpdateScoreUI();
        UpdateCollectiblesUI();
        LoadCharacter(); // Load the selected character

    }

    private void Update()
    {
        // Increment score based on time elapsed
        elapsedTime += Time.deltaTime;
        sessionScore = Mathf.FloorToInt(elapsedTime);
        UpdateScoreUI();
        if (magnetActive && playerTransform != null)
        {
            ApplyMagnetEffect();
        }
    }

    public void UpdateScore(int value)
    {
        // Update session score
        sessionScore += value;
        UpdateScoreUI();
    }

   public void UpdateCollectibles(int value)
{
           // Debug.Log($"Updating Collectibles: Current = {sessionCollectibles}, Adding = {value}");

    sessionCollectibles += value;

    // Update global total collectibles
    int totalCollectibles = PlayerPrefs.GetInt("TotalCollectibles", 0) ;
    PlayerPrefs.SetInt("TotalCollectibles", totalCollectibles);
    PlayerPrefs.Save();

    // Update UI
    UpdateCollectiblesUI();
}

    public int GetSessionScore()
    {
        return sessionScore;
    }

    public int GetSessionCollectibles()
    {
        return sessionCollectibles;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {sessionScore}";
        }
    }

    private void UpdateCollectiblesUI()
    {
        if (collectiblesText != null)
        {
            collectiblesText.text = $"Collectibles: {sessionCollectibles}";
        }
    }

    private void LoadCharacter()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "Sackboy");
        ShopItem selectedCharacter = FindCharacterByName(selectedCharacterName);

        if (selectedCharacter != null)
        {
            if (currentCharacter != null)
            {
                Destroy(currentCharacter);
            }

            currentCharacter = Instantiate(selectedCharacter.model, playerSpawnPoint.position, playerSpawnPoint.rotation);
            currentCharacter.SetActive(true); // Ensure the character is active
        }
        else
        {
            Debug.LogError("No character selected or found!");
        }
    }



    public void ChangeCharacter(ShopItem newCharacter)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(newCharacter.model, playerSpawnPoint.position, playerSpawnPoint.rotation);
        currentCharacter.SetActive(true); // Ensure the character is active

        PlayerPrefs.SetString("SelectedCharacter", newCharacter.itemName);
        PlayerPrefs.Save();

        Debug.Log($"Changed to {newCharacter.itemName}!");
    }


    private ShopItem FindCharacterByName(string name)
    {
        // Assuming `CharacterManager` script has a method to get characters
        CharacterManager characterManager = FindObjectOfType<CharacterManager>();
        return characterManager.characters.Find(c => c.itemName == name);
    }
    public void ActivateMagnet(float range, float speed, Transform player)
    {
        magnetActive = true;
        magnetRange = range;
        magnetSpeed = speed;
        playerTransform = player;

        Collider[] collectibles = Physics.OverlapSphere(player.position, magnetRange, LayerMask.GetMask("Collectible"));
        foreach (var collectible in collectibles)
        {
            collectible.GetComponent<Collectible>().EnableMagnet(player, magnetSpeed);
        }

        Debug.Log("Magnet Power-Up Activated!");

        // Start deactivation timer
        StartCoroutine(MagnetTimer(5f)); // Example: 5 seconds duration
    }

    private IEnumerator MagnetTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        DeactivateMagnet();
    }


    private void ApplyMagnetEffect()
    {
        if (!magnetActive || playerTransform == null) return;

        Collider[] collectibles = Physics.OverlapSphere(playerTransform.position, magnetRange, LayerMask.GetMask("Collectible"));
        foreach (var collectibleCollider in collectibles)
        {
            Collectible collectibleScript = collectibleCollider.GetComponent<Collectible>();
            if (collectibleScript != null && !collectibleScript.IsMagnetActive())
            {
                collectibleScript.EnableMagnet(playerTransform, magnetSpeed);
            }
        }
    }




    public void DeactivateMagnet()
    {
        magnetActive = false;
        playerTransform = null; // Clear the player transform

        // Disable magnet effect for all collectibles
        Collectible[] allCollectibles = FindObjectsOfType<Collectible>();
        foreach (var collectible in allCollectibles)
        {
            collectible.DisableMagnet();
        }

        Debug.Log("Magnet Power-Up Deactivated!");
    }

}

