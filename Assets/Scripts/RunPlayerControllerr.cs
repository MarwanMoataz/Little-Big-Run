using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RunPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float laneDistance = 3f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Slide Settings")]
    public float slideDuration = 0.5f;

    // Movement variables
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private Vector3 targetPosition;

    // Jump variables
    private float verticalVelocity = 0f;

    // Slide variables
    private bool isSliding = false;
    private float slideTimer;
    private float originalHeight;
    private Vector3 originalCenter;

    // Component references
    private CharacterController characterController;
    private Animator animator;

    private void Start()
    {
        InitializeComponents();
        SetupInitialState();

        SnapToGround();
    }
    private void SnapToGround()
    {
        // Move the character slightly down to ensure it detects the ground
        characterController.enabled = false; // Temporarily disable to manually adjust position
        transform.position = new Vector3(transform.position.x, 0.1f, -28); // Adjust height and Z-axis
        characterController.enabled = true;
    }
    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void SetupInitialState()
    {
        targetPosition = transform.position;
        originalHeight = characterController.height;
        originalCenter = characterController.center;

        verticalVelocity = -1f; // Small downward force to ensure immediate grounding
    }

    private void Update()
    {
        HandleLaneMovement();
        HandleJumpAndGravity();
        HandleSlide();
    }

    #region Movement
    private void HandleLaneMovement()
    {
        // Handle lane switching input
        if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;
            animator.SetTrigger("MoveRight");
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
            animator.SetTrigger("MoveLeft");
        }

        UpdateTargetPosition();
        MoveLane();
    }

    private void UpdateTargetPosition()
    {
        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void MoveLane()
    {
        // Smoothly move the character to the target lane
        Vector3 moveDirection = new Vector3((targetPosition.x - transform.position.x) * 10f, 0, 0);
        characterController.Move(moveDirection * Time.deltaTime);
    }

    #endregion

    #region Jump
    private bool jumpBuffered = false;
    private int jumpCount = 0; // Tracks how many jumps have been performed
    public int maxJumps = 2; // Allow up to 2 jumps

    private void HandleJumpAndGravity()
    {
        // Always lock Z-axis
        transform.position = new Vector3(transform.position.x, transform.position.y, -28);

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -1f; // Ensure player stays grounded

            // Reset jump count when grounded
            jumpCount = 0;

            if (Input.GetKeyDown(KeyCode.W)) // First jump trigger
            {
                StartJump();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumps) // Double jump trigger
            {
                StartJump();
            }

            verticalVelocity += gravity * Time.deltaTime; // Apply gravity
        }

        // Apply vertical movement
        Vector3 verticalMove = new Vector3(0, verticalVelocity, 0);
        characterController.Move(verticalMove * Time.deltaTime);
    }

    private void StartJump()
    {
        verticalVelocity = jumpForce;
        jumpCount++; // Increment the jump count
        animator.SetTrigger("Jump");
    }

    #endregion

    #region Slide
    private void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isSliding && characterController.isGrounded)
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        characterController.height = originalHeight / 2;
        characterController.center = new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z);

        animator.SetTrigger("Slide");
    }

    private void EndSlide()
    {
        isSliding = false;

        characterController.height = originalHeight;
        characterController.center = originalCenter;
    }

    #endregion

    #region Collision
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered with: {other.gameObject.name} with tag {other.gameObject.tag}");
        if (other.CompareTag("Obstacle"))
        {
            HandleObstacleCollision();
        }
    }

    private void HandleObstacleCollision()
    {
        Debug.Log("Game Over - Hit Obstacle!");
        SceneManager.LoadScene(2); // Reload or go to Game Over scene
    }

    #endregion
}
