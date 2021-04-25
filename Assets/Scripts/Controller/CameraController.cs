using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Some aspects taken from: https://answers.unity.com/questions/600577/camera-rotation-around-player-while-following.html
/// </summary>
public class CameraController : MonoBehaviour
{
    // speed of the x rotation of the camera
    public float xSpeed;

    // player's transform
    public Transform playerTransform;

    // the offset of the player's height (so we look at their head and not their feet)
    private Vector3 playerHeightOffset;

    // offset is the vector from the player to the camera
    private Vector3 offset;

    // the input we're getting to move the camera
    private Vector2 lookInput;


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
        // Only apply rotation if we get input
        // See https://answers.unity.com/questions/46770/rotate-a-vector3-direction.html for vector rotation details.
        if (lookInput.x != 0)
        {
            // apply x rotation
            offset = Quaternion.AngleAxis(lookInput.x * xSpeed * Time.deltaTime, Vector3.up) * offset;
        }

        // update camera position
        transform.position = playerTransform.position + offset;

        // look at the player
        transform.LookAt(playerTransform.position + playerHeightOffset);
    }


    /// <summary>
    /// Get the Vector2 camera rotation input.
    /// </summary>
    /// <param name="context">The context from which we get Vector2 input.</param>
    public void RotateCamera(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();   
    }
}