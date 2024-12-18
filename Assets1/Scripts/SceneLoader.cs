using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CharacterManager characterManager;

    public void LoadSceneWithCharacterManager(int sceneIndex)
    {
        characterManager = CharacterManager.instance; // Assuming it's a singleton
        SceneManager.LoadScene(sceneIndex);
    }
}
