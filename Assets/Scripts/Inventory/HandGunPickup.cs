using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunPickup : MonoBehaviour, IInteractable
{
    public BaseWeapon handGun;
    public InteractionType InteractionType => InteractionType.Press;

    public float HoldDuration => 0f;

    public void Interact()
    {
        GameObject emHo = Instantiate(handGun.gameObject, transform.position, Quaternion.identity, transform);
        Player.Instance.PickupWeapon(emHo.GetComponent<IWeapon>(), emHo);
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
