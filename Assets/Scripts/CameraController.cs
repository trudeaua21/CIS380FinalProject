using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Some aspects taken from: https://answers.unity.com/questions/600577/camera-rotation-around-player-while-following.html
/// </summary>
public class CameraController : MonoBehaviour
{
    public float turnSpeed;
    public Transform playerTransform;

    private Vector3 startingOffset;


    // Start is called before the first frame update
    void Start()
    {
        // make sure we start at the same offset by the player whenever the game runs
        startingOffset = new Vector3(0f, 2f, -3.2f);

        transform.position = playerTransform.position + startingOffset;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
