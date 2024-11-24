using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDoorEvent : MonoBehaviour
{
    [SerializeField] SpriteRenderer lDoorSpriteRenderer;
    [SerializeField] Sprite lDoorOpenSprite;

    public void OnLDoorOpen()
    {
        lDoorSpriteRenderer.sprite = lDoorOpenSprite;

    }
}
