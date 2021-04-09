using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;


    private Rigidbody rb;
    private Animator animator;

    private Vector3 movement;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the player to face the direction they're moving
        if (isMoving)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.AddForce(movement * speed, ForceMode.Acceleration);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 moveValue = context.ReadValue<Vector2>();
        if (!moveValue.Equals(Vector2.zero))
        {
            isMoving = true;
            animator.SetBool("isMoving", true);
            movement.Set(moveValue.x, 0, moveValue.y);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
            movement.Set(0, 0, 0);
        }
    }
}