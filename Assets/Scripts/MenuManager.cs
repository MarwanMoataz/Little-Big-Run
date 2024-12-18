using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu; // Reference to the Main Menu panel
    public GameObject descriptionMenu; // Reference to the Settings Menu panel
    public Button backButton; // Back button in the Settings Menu
    public Button exitButton; // Back button in Main Menu
    public List<Button> mainMenuButtons; // All buttons in the main menu
    public List<Button> settingsMenuButtons; // All buttons in the settings menu
    public static bool isPlayerLocked = false; // Global flag to lock/unlock player controls

    private GameObject currentMenu; // Tracks the currently active menu
    private List<Button> currentButtons; // The currently navigable buttons
    private int selectedButtonIndex = 0; // Currently highlighted button index

    private void Start()
    {
        // Start with only the Main Menu visible
        mainMenu.SetActive(true);
        descriptionMenu.SetActive(false);
        currentMenu = mainMenu;
        currentButtons = mainMenuButtons;

        

        

        // Add listener for the Back button
        backButton.onClick.AddListener(OpenMainMenu);

        // Highlight the first button
        HighlightButton();
    }

    private void Update()
    {

        // Handle ESC or Back button press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackAction();
        }
    }
    
    

    private void HighlightButton()
    {
        for (int i = 0; i < currentButtons.Count; i++)
        {
            ColorBlock colors = currentButtons[i].colors;
            colors.normalColor = i == selectedButtonIndex ? Color.yellow : Color.white; // Highlight the selected button
            currentButtons[i].colors = colors;
        }
    }

    // Handles the Back button or ESC action
    public void HandleBackAction()
    {
        if (currentMenu == descriptionMenu)
        {
            OpenMainMenu();
        }
        else if (currentMenu == mainMenu)
        {
            ExitMenu();
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    // Opens the Main Menu
    public void OpenMainMenu()
    {
        descriptionMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = mainMenu;
        currentButtons = mainMenuButtons;
        selectedButtonIndex = 0;
        HighlightButton();
    }

    // Opens the Settings Menu
    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        descriptionMenu.SetActive(true);
        currentMenu = descriptionMenu;
        currentButtons = settingsMenuButtons;
        selectedButtonIndex = 0;
        HighlightButton();
    }

    // Exits the menu and unlocks the player
    public void ExitMenu()
    {
        mainMenu.SetActive(false);
        descriptionMenu.SetActive(false);
        UnlockPlayer();
        Debug.Log("Exiting menu.");
    }

    // Locks player controls
    public static void LockPlayer()
    {
        isPlayerLocked = true;
        Debug.Log("Player controls locked.");
    }

    // Unlocks player controls
    public static void UnlockPlayer()
    {
        isPlayerLocked = false;
        Debug.Log("Player controls unlocked.");
    }

   

    // Exits the game
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}

