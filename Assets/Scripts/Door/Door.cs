using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteractable
{
    public InteractionType InteractionType => InteractionType.Press;
    public float HoldDuration => 0f;

    public void Interact()
    {
        Destroy(gameObject);
    }
    public void HoldInteract()
    {

    }

    public void UpdateProgress(float progress)
    {

    }

    public void ShowProgress()
    {

    }

    public void HideProgress()
    {

    }
}
