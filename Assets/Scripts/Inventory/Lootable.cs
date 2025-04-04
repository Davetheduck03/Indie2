using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lootable : MonoBehaviour, IInteractable
{

    [SerializeField] private float holdDuration = 2f;
    [SerializeField] private Canvas progressCanvas;
    [SerializeField] private Image progressFill;
    [Header("Loot Settings")]
    [SerializeField] private List<GameObject> lootTable;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 0);

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

    private void SpawnRandomLoot()
    {
        if (lootTable.Count == 0) return;

        int randomIndex = Random.Range(0, lootTable.Count);
        Vector3 spawnPosition = transform.position + spawnOffset;

        Instantiate(lootTable[randomIndex], spawnPosition, Quaternion.identity);
    }

    public void HoldInteract()
    {
        SpawnRandomLoot();
        Destroy(gameObject);
    }
}
