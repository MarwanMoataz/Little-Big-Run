using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RunPlayerController : MonoBehaviour
{
    public float laneDistance = 3f; // Distance between lanes
    public float jumpForce = 5f; // Force for jumping
    public float gravity = -9.81f; // Gravity value
    public float slideDuration = 0.5f; // Duration of the slide

    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private bool isSliding = false;
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
    }

    void Update()
    {
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

        // Trigger running animation
        animator.SetBool("isRunning", true);

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
