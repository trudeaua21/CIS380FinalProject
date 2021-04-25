using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCubeExample : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered");
        if (other.gameObject.CompareTag("Sword"))
        {
            Destroy(gameObject);
        }
    }
}
