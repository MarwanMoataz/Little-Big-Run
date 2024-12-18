using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;    // Reference to the Pause Menu panel
    public Slider musicVolumeSlider;     // Reference to the music volume slider
    public Slider soundVolumeSlider;     // Reference to the sound effects volume slider

    private bool isPaused = false;

    void Start()
    {
        // Initialize the sliders to match current volume levels
        musicVolumeSlider.value = AudioManager.instance.musicVolume;
        soundVolumeSlider.value = AudioManager.instance.soundVolume;

        // Add listeners to sliders to change volume dynamically
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Pause when pressing Escape
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Stop the game time
    }

    void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game time
    }

    void OnMusicVolumeChanged(float value)
    {
        AudioManager.instance.SetMusicVolume(value); // Adjust the music volume
    }

    void OnSoundVolumeChanged(float value)
    {
        AudioManager.instance.SetSoundVolume(value); // Adjust the sound effects volume
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Ensure game is running normally
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current level
    }

    public void QuitGame()
    {
        // Exit the game (useful for build)
        Application.Quit();
    }
}
