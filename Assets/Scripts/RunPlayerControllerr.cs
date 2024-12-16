using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RunPlayerControllerr : MonoBehaviour
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

    // Component references
    private CharacterController characterController;
    private Animator animator;

    private void Start()
    {
        InitializeComponents();
        SetupInitialState();
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
        // Handle lane switching
        if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
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
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = (targetPosition.x - transform.position.x) * 10f;
        characterController.Move(moveDirection * Time.deltaTime);
    }
    #endregion
    #region jump
    private bool jumpBuffered = false;

    private void HandleJumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (jumpBuffered || Input.GetKeyDown(KeyCode.W))
            {
                StartJump();
                jumpBuffered = false; // Reset buffer once the jump starts
            }
            else
            {
                verticalVelocity = 0; // Reset vertical velocity on the ground
            }
        }
        else
        {
            // Apply gravity when not grounded
            verticalVelocity += gravity * Time.deltaTime;

            // Buffer the jump input if W is pressed mid-air
            if (Input.GetKeyDown(KeyCode.W))
            {
                jumpBuffered = true;
            }
        }

        Vector3 moveDirection = new Vector3(0, verticalVelocity, 0);
        characterController.Move(moveDirection * Time.deltaTime);
    }


    private void StartJump()
    {
        verticalVelocity = jumpForce;
        animator.SetTrigger("Jump");
    }
    #endregion

    #region Slide
    private void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isSliding)
        {
            StartSlide();
        }

        if (isSliding)
        {
            UpdateSlide();
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        characterController.height = originalHeight / 2;
        animator.SetTrigger("Slide");
    }

    private void UpdateSlide()
    {
        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0)
        {
            EndSlide();
        }
    }

    private void EndSlide()
    {
        isSliding = false;
        characterController.height = originalHeight;
    }
    #endregion

    #region Collision
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision();
        }
    }

    private void HandleObstacleCollision()
    {
        SceneManager.LoadScene(2);
    }
    #endregion
}