using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.1f;

    [Header("Player Animator & Gravity")]
    public CharacterController characterControllerObj;
    public float gravity = -9.81f;

    [Header("Player Script Camera")]
    public Transform playerCamera;

    [Header("Player Jump & Velocity")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    UnityEngine.Vector3 velocity;
    public Transform groundCheck;
    private bool isGrounded;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        characterControllerObj.Move(velocity * Time.deltaTime);
        
        playerMove();
    }

    void playerMove() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        UnityEngine.Vector3 direction = new UnityEngine.Vector3(horizontalAxis, 0f, verticalAxis).normalized; // Normalize the vector to prevent faster diagonal movement

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = UnityEngine.Quaternion.Euler(0f, angle, 0f);
            
            UnityEngine.Vector3 moveDirection = UnityEngine.Quaternion.Euler(0f, targetAngle, 0f) * UnityEngine.Vector3.forward;
            characterControllerObj.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
    }
}
