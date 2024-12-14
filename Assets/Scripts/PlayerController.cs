using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    
    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        // Get references to the CharacterController and Animator components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get movement input (WASD)
        float moveX = -Input.GetAxis("Horizontal"); // A/D
        float moveZ = -Input.GetAxis("Vertical");   // W/S

        // Create movement vector in world space
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        // Apply the movement to the controller
        controller.Move(move * moveSpeed * Time.deltaTime);
if (move != Vector3.zero)
{
    Quaternion targetRotation = Quaternion.LookRotation(move);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
}

        // Update the Speed parameter in the Animator based on the magnitude of movement
        animator.SetFloat("Speed", move.magnitude);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jumping (optional)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");  // Trigger the Jump animation if you have it set up
        }
    }
}
