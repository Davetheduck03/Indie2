using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDoorEvent : MonoBehaviour
{
    [SerializeField] SpriteRenderer uDoorSpriteRenderer;
    [SerializeField] Sprite uDoorOpenSprite;

    public void OnUDoorOpen()
    {
        uDoorSpriteRenderer.sprite = uDoorOpenSprite;
    }
}
