using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunPickup : MonoBehaviour, IInteractable
{
    public InteractionType InteractionType => InteractionType.Press;

    public float HoldDuration => 0f;

    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject pickupPrompt;

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
            Destroy(gameObject); // Destroy pickup object
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
