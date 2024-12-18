using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For Button and Image
using TMPro;

public class CharacterManager : MonoBehaviour
{
    public List<ShopItem> characters; // Assign in Inspector (all character ScriptableObjects)
    public Transform characterDisplay; // Parent object to display selected character
    private GameObject currentCharacter;
    public Transform characterGrid; // Assign in Inspector
    public GameObject characterOptionPrefab;
    public static CharacterManager instance
    {
        get
        {
            if (_instance == null)
            {
                // Find the instance if it doesn't exist
                _instance = FindObjectOfType<CharacterManager>();

                // If an instance wasn't found, create one
                if (_instance == null)
                {
                    GameObject go = new GameObject("CharacterManager");
                    _instance = go.AddComponent<CharacterManager>();
                }
            }
            return _instance;
        }
    }
    private static CharacterManager _instance;


    private void Awake()
    {
        // Ensure only one instance exists
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        if (characters != null && characterGrid != null && characterDisplay != null && characterOptionPrefab != null)
        {
            ShowUnlockedCharacters();
            SetDefaultCharacter();
        }
        else
        {
            //Debug.LogError("One or more necessary references are not assigned.");
        }
    }

    private void ShowUnlockedCharacters()
    {
        foreach (var character in characters)
        {
            int unlocked = PlayerPrefs.GetInt($"Unlocked_{character.itemName}", 0);
            if (unlocked == 1)
            {
                CreateCharacterOption(character);
            }
        }
    }

    private void CreateCharacterOption(ShopItem character)
    {
        GameObject option = Instantiate(characterOptionPrefab, characterGrid);
        if (option != null)
        {
            option.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = character.itemName;
            option.transform.Find("ItemIcon").GetComponent<Image>().sprite = character.icon;
            Button selectCharacter = option.transform.Find("SelectButton").GetComponent<Button>();

            // Add button functionality to select the character
            selectCharacter.onClick.AddListener(() => SelectCharacter(character));

            // Highlight the selected character
            int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
            if (selectedCharacter == characters.IndexOf(character))
            {
                option.GetComponent<Image>().color = Color.green; // Example highlight (can be customized)
            }
        }
        else
        {
            Debug.LogError("Failed to instantiate characterOptionPrefab.");
        }
    }

    private void SetDefaultCharacter()
    {
        if (!PlayerPrefs.HasKey("SelectedCharacter"))
        {
            PlayerPrefs.SetInt("SelectedCharacter", 0); // Set default character index to 0
            PlayerPrefs.Save();
        }
    }

    public void SelectCharacter(ShopItem character)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(character.model, characterDisplay.position, characterDisplay.rotation);
        PlayerPrefs.SetString("SelectedCharacter", character.itemName);
        PlayerPrefs.Save();
        Debug.Log($"Selected {character.itemName}!");
        // Highlight the selected character
        HighlightSelectedCharacter(character);
    }

    private void HighlightSelectedCharacter(ShopItem selectedCharacter)
    {
        foreach (Transform option in characterGrid)
        {
            if (option.name == selectedCharacter.itemName)
            {
                option.GetComponent<Image>().color = Color.green; // Example highlight color
            }
            else
            {
                option.GetComponent<Image>().color = Color.white; // Reset other characters
            }
        }
    }
}
