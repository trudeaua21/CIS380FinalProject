using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Some aspects taken from: https://answers.unity.com/questions/600577/camera-rotation-around-player-while-following.html
/// </summary>
public class CameraController : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    public Transform playerTransform;

    // the offset of the player's height (so we look at their head and not their feet)
    private Vector3 playerHeightOffset;

    // offset is the vector from the player to the camera
    private Vector3 offset;

    // the minimum y value of the transform of the camera
    private const float yMin = 0f;

    // the rotation for the z axis of the camera should always be 0
    private const float zRotation = 0f;

    private Vector2 lookInput;
    private bool isLooking;

    void Start()
    {
        lookInput = new Vector2(0, 0);

        playerHeightOffset = new Vector3(0f, 1.5f, 0f);

        // make sure we start at the same offset by the player whenever the game runs
        offset = new Vector3(0f, 2f, -3.2f);

        transform.position = playerTransform.position + offset;
    }


    // follow cameras should be in late update
    private void LateUpdate()
    {
        // apply y rotation (apparently rotation * vector is how the rotation is applied)
        offset = Quaternion.AngleAxis(lookInput.y * ySpeed * Time.deltaTime, Vector3.right) * offset;

        // apply x rotation
        offset = Quaternion.AngleAxis(lookInput.x * xSpeed * Time.deltaTime, Vector3.up) * offset;

        Vector3 newPosition = playerTransform.position + offset;

        // make sure our camera is above the ground
        if (newPosition.y < yMin)
            newPosition.y = yMin;

        // update camera position
        transform.position = newPosition;

        // look at the player
        transform.LookAt(playerTransform.position + playerHeightOffset);
    }


    /// <summary>
    /// Get the Vector2 camera rotation and apply it to the offset. Also, apply it to our rotation
    /// 
    /// See https://answers.unity.com/questions/46770/rotate-a-vector3-direction.html for vector rotation details.
    /// </summary>
    /// <param name="context">The context from which we get Vector2 input.</param>
    public void RotateCamera(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();   
    }
}