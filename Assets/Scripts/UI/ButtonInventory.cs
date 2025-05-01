using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInventory : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleInventory);
    }

    private void ToggleInventory()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.EnableInventoryUI();
        }
        else
        {
            Debug.LogError("UIManager instance not found!");
        }
    }
}
