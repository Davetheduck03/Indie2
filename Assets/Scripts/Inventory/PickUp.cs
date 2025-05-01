using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    public InteractionType InteractionType => InteractionType.Press;

    public float HoldDuration => 0f;

    [SerializeField] public GameObject weaponPrefab;

    public void Interact()
    {
        AttemptPickup();
    }

    private void AttemptPickup()
    {
        GameObject weaponObject = Instantiate(weaponPrefab);
        IWeapon weapon = weaponObject.GetComponent<IWeapon>();

        if (weapon != null && Player.Instance != null)
        {
            Player.Instance.PickupWeapon(weapon);
            Destroy(gameObject);
        }
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
