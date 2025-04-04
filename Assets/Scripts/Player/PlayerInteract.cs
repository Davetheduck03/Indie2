using UnityEngine;
using UnityEngine.UI;
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Button interactButton;

    private IInteractable currentInteractable;
    private bool isHolding;
    private float holdTimer;

    private void Update()
    {
        HandleHoldInteraction();
        UpdateButtonVisibility();
    }

    private void HandleHoldInteraction()
    {
        if (!isHolding || currentInteractable == null) return;

        holdTimer += Time.deltaTime;
        currentInteractable.UpdateProgress(holdTimer / currentInteractable.HoldDuration);

        if (holdTimer >= currentInteractable.HoldDuration)
        {
            currentInteractable.HoldInteract();
            ClearInteraction();
        }
    }

    private void UpdateButtonVisibility()
    {
        interactButton.gameObject.SetActive(currentInteractable != null);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            interactButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            ClearInteraction();
        }
    }

    public void OnPointerDown()
    {
        if (currentInteractable == null) return;

        switch (currentInteractable.InteractionType)
        {
            case InteractionType.Press:
                currentInteractable.Interact();
                ClearInteraction();
                break;

            case InteractionType.Hold:
                currentInteractable.ShowProgress();
                isHolding = true;
                holdTimer = 0f;
                break;
        }
    }

    public void OnPointerUp()
    {
        if (isHolding && currentInteractable != null)
        {
            currentInteractable.HideProgress();
        }
        isHolding = false;
        holdTimer = 0f;
    }

    private void ClearInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.HideProgress();
        }
        currentInteractable = null;
        isHolding = false;
        holdTimer = 0f;
    }
}