using UnityEngine;

public enum InteractionType
{
    Press,
    Hold
}

public interface IInteractable
{
    InteractionType InteractionType { get; }
    float HoldDuration { get; }
    void UpdateProgress(float progress);
    void ShowProgress();
    void HideProgress();
    void Interact();
    void HoldInteract();
}
