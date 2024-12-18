using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public List<ShopItem> allCharacters; // Assign all character ScriptableObjects
    public Transform spawnPoint; // Set the spawn location

    private void Start()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "DefaultCharacter");
        ShopItem selectedCharacter = allCharacters.Find(c => c.itemName == selectedCharacterName);

        if (selectedCharacter != null)
        {
            Instantiate(selectedCharacter.model, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("No character selected or found!");
        }
    }
}
