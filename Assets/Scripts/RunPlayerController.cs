using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayerController : MonoBehaviour
{
    public float laneDistance = 3f; // Distance between lanes
    public float speed = 10f; // Forward speed
    public float jumpForce = 5f; // Force for jumping
    public float slideDuration = 0.5f; // Duration of the slide

    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private bool isJumping = false;
    private bool isSliding = false;
    private CharacterController characterController;

    private Vector3 targetPosition; // Target position for the player
    private float originalHeight; // Original height of the player collider
    private float slideTimer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
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

        // Calculate the target position
        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Smoothly move to the target lane
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Handle jump
        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            StartJump();
        }

        // Handle slide
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
        isJumping = true;
        characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
        Invoke(nameof(EndJump), 0.5f); // Simulate jump duration
    }

    private void EndJump()
    {
        isJumping = false;
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        characterController.height = originalHeight / 2; // Halve the player's height
    }

    private void EndSlide()
    {
        isSliding = false;
        characterController.height = originalHeight; // Reset the player's height
    }
}
