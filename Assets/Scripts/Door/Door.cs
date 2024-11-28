using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] Collider2D doorCollider;
    [SerializeField] Animator uDoorAnimator;
    [SerializeField] Animator lDoorAnimator;
    [SerializeField] bool playerInside = false;
    [SerializeField] LayerMask layerPlayer;


    private void Update()
    {
        PlayerCheck();
        if (playerInside == true && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    public void Interact()
    {
        Destroy(doorCollider);

        uDoorAnimator.SetTrigger("Open");
        lDoorAnimator.SetTrigger("Open");
    }

    private void PlayerCheck()
    {
         if(Physics2D.OverlapCircle(transform.position, 2f, layerPlayer))
         {
                playerInside = true;
         }
        else
        {
            playerInside = false;
        }

    }

}
