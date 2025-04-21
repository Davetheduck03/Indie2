using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int inventorySize = 12;

    private Transform inventoryPanel;
    private List<InventorySlot> slots = new List<InventorySlot>();
    private List<InventoryItemData> inventoryData = new List<InventoryItemData>();
    private bool needsUIUpdate = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (inventoryData.Count == 0)
            {
                for (int i = 0; i < inventorySize; i++)
                {
                    inventoryData.Add(new InventoryItemData(null, 0));
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInventoryPanel(Transform panelTransform)
    {
        inventoryPanel = panelTransform;
        MarkForUIUpdate();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindInventoryPanel();
        MarkForUIUpdate();
    }

    private void Update()
    {
        if (needsUIUpdate && inventoryPanel != null && inventoryPanel.gameObject.activeInHierarchy)
        {
            InitializeInventoryUI();
        }
    }

    private void InitializeInventoryUI()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        slots.Clear();

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, inventoryPanel);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slots.Add(slot);

            InventoryItemData data = inventoryData[i];
            if (data.item != null)
            {
                slot.SetItem(data.item, data.count);
            }
        }
        needsUIUpdate = false;
    }

    private void FindInventoryPanel()
    {
        InventoryPanelFinder[] finders = Resources.FindObjectsOfTypeAll<InventoryPanelFinder>();
        foreach (InventoryPanelFinder finder in finders)
        {
            if (finder.gameObject.scene == SceneManager.GetActiveScene())
            {
                inventoryPanel = finder.transform;
                return;
            }
        }
        Debug.LogWarning("Inventory panel not found in scene!");
    }

    public void MarkForUIUpdate()
    {
        needsUIUpdate = true;
        if (inventoryPanel != null && inventoryPanel.gameObject.activeInHierarchy)
        {
            InitializeInventoryUI();
        }
    }

    public bool AddItem(InventoryItemSO item, int count = 1)
    {
        for (int i = 0; i < inventoryData.Count; i++)
        {
            InventoryItemData data = inventoryData[i];
            if (data.item == item && data.count < item.maxStack)
            {
                int spaceRemaining = item.maxStack - data.count;
                int addAmount = Mathf.Min(spaceRemaining, count);
                data.count += addAmount;
                inventoryData[i] = data;
                count -= addAmount;

                if (i < slots.Count && slots[i] != null)
                    slots[i].SetItem(data.item, data.count);

                if (count <= 0) return true;
            }
        }

        for (int i = 0; i < inventoryData.Count; i++)
        {
            InventoryItemData data = inventoryData[i];
            if (data.item == null)
            {
                int addAmount = Mathf.Min(count, item.maxStack);
                data.item = item;
                data.count = addAmount;
                inventoryData[i] = data;
                count -= addAmount;

                // Update UI slot
                if (i < slots.Count && slots[i] != null)
                    slots[i].SetItem(data.item, data.count);

                if (count <= 0) return true;
            }
        }

        return false;
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        int slotIndex = slots.IndexOf(slot);
        if (slotIndex == -1 || !slot.HasItem) return;

        // Use the item
        slot.Item.OnUseItem();

        // Update inventoryData
        InventoryItemData data = inventoryData[slotIndex];
        data.count--;
        if (data.count <= 0)
        {
            data.item = null;
            data.count = 0;
        }
        inventoryData[slotIndex] = data;

        // Update the slot's UI
        if (data.item == null)
            slot.ClearSlot();
        else
            slot.SetItem(data.item, data.count);
    }

    // Struct to hold inventory data
    [System.Serializable]
    public struct InventoryItemData
    {
        public InventoryItemSO item;
        public int count;

        public InventoryItemData(InventoryItemSO item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}