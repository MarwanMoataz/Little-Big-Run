using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhoneInteraction : MonoBehaviour
{
    public GameObject interactionPopup; // UI element for "Press E to start"
    public Camera mainCamera; // Main camera
    public GameObject menu; // Menu UI
    public GameObject player; // Player GameObject
    public Transform playerLockPosition; // Position to lock the player
    public Transform secondaryCameraPosition; // Target position for the camera during interaction
    public Button[] menuButtons; // Array of menu buttons (Start, Settings, Exit)

    private bool isPlayerNear = false; // Is the player near the object
    private bool isMenuActive = false; // Is the menu currently active
    private bool isTransitioning = false; // Is a camera transition currently happening
    private int selectedButtonIndex = 0; // Currently highlighted menu button

    private Vector3 originalCameraPosition; // Save the original camera position
    private Quaternion originalCameraRotation; // Save the original camera rotation

    private Vector3 originalPlayerPosition; // Save player's original position
    private Quaternion originalPlayerRotation; // Save player's original rotation
    private bool isInputBlocked = false; // To block input briefly

    void Start()
    {
        interactionPopup.SetActive(false);
        menu.SetActive(false);

        // Highlight the first button in the menu
        HighlightMenuButton();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactionPopup.SetActive(true); // Show interaction popup
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionPopup.SetActive(false); // Hide interaction popup
        }
    }

    void Update()
    {
        if (isInputBlocked) return; // Block inputs while transitioning or shortly after exiting the menu

        // Show the menu when the player presses E near the phone
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            StartCoroutine(TransitionToInteractionPosition());
        }

        // Navigate the menu
        if (isMenuActive)
        {

            // Exit the menu when ESC is pressed
            if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
            {
                StartCoroutine(TransitionToOriginalPosition());
            }
        }
    }

    private IEnumerator TransitionToInteractionPosition()
    {
        isTransitioning = true;

        // Save the original positions
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        originalPlayerPosition = player.transform.position;
        originalPlayerRotation = player.transform.rotation;

        // Lock player movement and rotation
        LockPlayer();

        // Smoothly transition the camera
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, secondaryCameraPosition.position, elapsedTime / duration);
            mainCamera.transform.rotation = Quaternion.Lerp(originalCameraRotation, secondaryCameraPosition.rotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = secondaryCameraPosition.position;
        mainCamera.transform.rotation = secondaryCameraPosition.rotation;

        interactionPopup.SetActive(false); // Hide interaction popup
        menu.SetActive(true); // Show the menu

        isMenuActive = true;
        isTransitioning = false;
    }

    private IEnumerator TransitionToOriginalPosition()
    {
        isTransitioning = true;

        // Smoothly transition the camera back
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, elapsedTime / duration);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, originalCameraRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;

        // Unlock player controls and reset position
        UnlockPlayer();

        menu.SetActive(false); // Hide the menu
        isMenuActive = false;
        isTransitioning = false;
    }

   


    private void HighlightMenuButton()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = i == selectedButtonIndex ? Color.yellow : Color.white; // Highlight the selected button
            menuButtons[i].colors = colors;
        }
    }

    private void LockPlayer()
    {
        MenuManager.LockPlayer();
        // Additional locking logic if needed
    }


    private void UnlockPlayer()
    {
        MenuManager.UnlockPlayer();
        // Additional unlocking logic if needed
    }




}