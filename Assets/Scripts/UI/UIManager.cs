using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Health Settings")]
    public Image healthBar;

    [Header("Hunger Settings")]
    public Image hungerBar;

    [SerializeField] private GameObject inventoryUI;

    [SerializeField] private Image primarySlotIcon;
    [SerializeField] private Image secondarySlotIcon;


    private static UIManager _instance;
    public static UIManager Instance {get{return _instance; }}
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIComponents();
        SetupPlayerEvents();
    }

    private void FindUIComponents()
    {
        var allImages = FindObjectsByType<Image>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var img in allImages)
        {
            if (img.CompareTag("HealthBar")) healthBar = img;
            if (img.CompareTag("HungerBar")) hungerBar = img;
            if (img.CompareTag("PrimaryWeaponIcon")) primarySlotIcon = img;
            if (img.CompareTag("SecondaryWeaponIcon")) secondarySlotIcon = img;
        }

        var inventoryObjs = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var obj in inventoryObjs)
        {
            if (obj.CompareTag("InventoryUI"))
            {
                inventoryUI = obj;
                break;
            }
        }
    }

    private void SetupPlayerEvents()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += UpdateHealthBar;
            Player.Instance.OnHungerChanged += UpdateHungerBar;
            Player.Instance.OnWeaponsChanged += UpdateWeaponDisplay;

            UpdateHealthBar(Player.Instance.playerHealth / 100f);
            UpdateHungerBar(Player.Instance.playerHunger / 100f);
            UpdateWeaponDisplay(Player.Instance.primaryWeapon, Player.Instance.secondaryWeapon);
        }
        else
        {
            StartCoroutine(WaitForPlayerInstance());
        }
    }

    private IEnumerator WaitForPlayerInstance()
    {
        while (Player.Instance == null)
        {
            yield return null;
        }
        SetupPlayerEvents();
    }

    private void Start()
    {
        
        Player.Instance.OnWeaponsChanged += UpdateWeaponDisplay;
        UpdateWeaponDisplay(Player.Instance.primaryWeapon, Player.Instance.secondaryWeapon);
    }

    private void OnEnable()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += UpdateHealthBar;
            Player.Instance.OnHungerChanged += UpdateHungerBar;
        }
    }

    

    private void OnDisable()
    {
        Player.Instance.OnHealthChanged -= UpdateHealthBar;
        Player.Instance.OnHungerChanged -= UpdateHungerBar;
    }
    
    private void UpdateHealthBar(float healthPercentage)
    {
        healthBar.fillAmount = healthPercentage;
    }

    private void UpdateHungerBar(float hungerPercentage)
    {
        hungerBar.fillAmount = hungerPercentage;
    }

    public void EnableInventoryUI()
    {
        if (inventoryUI == null)
        {
            FindUIComponents();
        }

        if (inventoryUI != null)
        {
            if(!inventoryUI.activeSelf)
            {inventoryUI.SetActive(true); }
            else { inventoryUI.SetActive(false); }

           
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.MarkForUIUpdate();
            }
        }
        else
        {
            Debug.LogError("Inventory UI reference is missing!");
        }
    }

    public void UpdateWeaponDisplay(IWeapon primary, IWeapon secondary)
    {
        if (primary != null)
        {
            primarySlotIcon.sprite = primary.GetWeaponIcon();
            primarySlotIcon.enabled = true;

        }
        else
        {
            primarySlotIcon.enabled = false;

        }

        if (secondary != null)
        {
            secondarySlotIcon.sprite = secondary.GetWeaponIcon();
            secondarySlotIcon.enabled = true;

        }
        else
        {
            secondarySlotIcon.enabled = false;

        }
    }
}