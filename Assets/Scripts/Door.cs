using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Collider2D doorCollider;
    [SerializeField] Animator uDoorAnimator;
    [SerializeField] Animator lDoorAnimator;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Destroy(doorCollider);

            uDoorAnimator.SetTrigger("Open");
            lDoorAnimator.SetTrigger("Open");
        }
    }

}
