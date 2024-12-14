using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float gravity = -9.8f; // Gravity force

    private CharacterController controller; // Reference to the CharacterController
    private Vector3 velocity; // Tracks vertical velocity

    void Start()
    {
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input from the keyboard (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal"); // Left/Right
        float moveZ = Input.GetAxis("Vertical"); // Forward/Backward

        // Create a movement vector based on input
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the character
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }

        // Apply vertical velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
