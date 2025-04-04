using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lootable : MonoBehaviour, IInteractable
{

    [SerializeField] private float holdDuration = 2f;
    [SerializeField] private Canvas progressCanvas;
    [SerializeField] private Image progressFill;

    public InteractionType InteractionType => InteractionType.Hold;
    public float HoldDuration => holdDuration;

    private void Start()
    {
        HideProgress();
    }

    public void UpdateProgress(float progress)
    {
        progressFill.fillAmount = progress;
    }

    public void ShowProgress()
    {
        progressCanvas.gameObject.SetActive(true);
    }

    public void HideProgress()
    {
        progressCanvas.gameObject.SetActive(false);
        progressFill.fillAmount = 0;
    }

    public void Interact() { }

    public void HoldInteract()
    {
        Debug.Log("Loot collected!");
        Destroy(gameObject);
    }
}
