using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunPlayerController : MonoBehaviour
{
    public RoadManager roadManager; // Reference to the RoadManager script
    public float laneDistance = 3f; // Distance between lanes
    public float jumpForce = 5f; // Force for jumping
    public float gravity = -9.81f; // Gravity value
    public float slideDuration = 0.5f; // Duration of the slide

    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private bool isSliding = false;
    private bool isGameOver = false; // To prevent multiple triggers
    private CharacterController characterController;

    private Vector3 targetPosition; // Target position for the player
    private float originalHeight; // Original height of the player collider
    private float slideTimer;

    private float verticalVelocity = 0f; // Vertical velocity for jumping and gravity

    private Animator animator; // Reference to Animator

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator not assigned to player!");
        }

        originalHeight = characterController.height;
        targetPosition = transform.position;

        if (roadManager == null)
        {
            Debug.LogError("RoadManager reference is missing in RunPlayerController!");
        }
    }

    void Update()
    {
        if (isGameOver || Time.timeScale == 0) return; // Prevent updates if game is over or paused

        // Handle lane switching
        if (Input.GetKeyDown(KeyCode.D)) // Move Right
        {
            if (currentLane < 2) currentLane++;
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Move Left
        {
            if (currentLane > 0) currentLane--;
        }

        // Calculate the target position for side movement only
        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Smoothly move to the target lane (side movement)
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = (targetPosition.x - transform.position.x) * 10f; // Adjust speed if needed

        // Handle jumping and gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = 0; // Reset vertical velocity when grounded

            if (Input.GetKeyDown(KeyCode.W)) // Start Jump
            {
                StartJump();
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity
        }

        moveDirection.y = verticalVelocity;

        // Apply movement only for side and vertical directions
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle sliding
        if (Input.GetKeyDown(KeyCode.S) && !isSliding)
        {
            StartSlide();
        }

        // Handle sliding duration
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object has the "Obstacle" tag
        if (hit.gameObject.CompareTag("Obstacle") && !isGameOver)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true; // Prevent re-triggering
        Debug.Log("Player collided with an obstacle. Playing Game Over animation and switching scene.");

        // Stop the road movement
        if (roadManager != null)
        {
            roadManager.StopRoad();
        }

        // Play the "GameOver" animation
        if (animator != null)
        {
            animator.SetTrigger("GameOver");
        }

        // Wait for the animation to complete before switching the scene
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Wait for the "GameOver" animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Switch to the Game Over scene with ID 3
        SceneManager.LoadScene(3);
    }

    private void StartJump()
    {
        verticalVelocity = jumpForce; // Apply upward velocity
        animator.SetTrigger("Jump"); // Activate the Jump trigger
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        characterController.height = originalHeight / 2; // Halve the player's height
        animator.SetTrigger("Slide"); // Activate the Slide trigger
    }

    private void EndSlide()
    {
        isSliding = false;
        characterController.height = originalHeight; // Reset the player's height
    }
}
