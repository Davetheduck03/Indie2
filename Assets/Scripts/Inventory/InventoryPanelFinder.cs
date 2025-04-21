// InventoryPanelFinder.cs
using UnityEngine;

public class InventoryPanelFinder : MonoBehaviour
{
    [SerializeField] private string panelTag = "InventoryPanel";

    private void Awake()
    {
        InventoryManager.Instance.SetInventoryPanel(transform);
    }
}
