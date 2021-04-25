using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    public bool toggleToSprint;

    public Transform cameraTransform;

    public GameObject swingHitboxes;

    private CharacterController controller;
    private Animator animator;

    CharacterCombat combat;

    private Vector2 movementInput;
    private Vector3 movement;

    private bool isMoving;
    private bool isRunning;
    private bool isSwinging;

    private float swingTimer;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        isSwinging = false;

        movement = new Vector3(0, 0, 0);
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSwinging)
        {
            // make sure the animation is at the correct speed
            animator.speed = 1f;

            swingTimer -= Time.deltaTime;

            if(swingTimer <= 0)
            {
                isSwinging = false;
                swingTimer = 0;
                swingHitboxes.SetActive(false);
                animator.SetBool("isAttacking", false);
            }
            // if we're far enough into the animation, make the hitbox active
            else if (!swingHitboxes.activeSelf && swingTimer < 1f)
            {
                swingHitboxes.SetActive(true);
            }
        }

        if (!isSwinging && isMoving)
        {
            if (isRunning)
                animator.speed = 2f;
            else
                animator.speed = 1f;
            // rotate our input to the correct orientation

            // get 2d vectors for our camera and player positions (since we don't care about the y axis for this)
            Vector2 cameraPos = new Vector2(cameraTransform.position.x, cameraTransform.position.z);
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);

            // the degree we want to rotate our input by is the rotation between up and the vector between player and camera
            float movementRotationDeg = Vector2.SignedAngle(Vector2.up, playerPos - cameraPos);
            
            // change the degree to radians
            float rads = movementRotationDeg * Mathf.Deg2Rad;
            float x = movementInput.x;
            float y = movementInput.y;

            // in 2d vector rotation, rotation by angle d is:
            // x' = x cos d - y sin d
            // y' = x sin d - y cos d
            float newX = (x * Mathf.Cos(rads)) - (y * Mathf.Sin(rads));
            float newY = (x * Mathf.Sin(rads)) + (y * Mathf.Cos(rads));

            // apply the rotated inputs to our movement vector
            movement.Set(newX, 0, newY);

            // rotate the player to face the direction of the movement
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.05f);

            transform.rotation = newRotation;

            // move the player 
            controller.Move(movement * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        if (!movementInput.Equals(Vector2.zero))
        {
            isMoving = true;
            animator.SetBool("isMoving", true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
    }

    public void SwingSword(InputAction.CallbackContext context)
    {
        if (!isSwinging && context.performed)
        {
            isSwinging = true;
            swingTimer = 1.3f;
            animator.SetBool("isAttacking", true);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (toggleToSprint)
        {
            if(context.performed)
                isRunning = !isRunning;
        }
        else
        {
            if (context.performed)
                isRunning = true;
            else if (context.canceled)
                isRunning = false;
        }
    }
}
