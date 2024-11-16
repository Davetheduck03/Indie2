using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Transform cameraTargetPosition; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            Camera.main.transform.position = new Vector3(
                cameraTargetPosition.position.x,
                cameraTargetPosition.position.y,
                Camera.main.transform.position.z); 
        }
    }
}
