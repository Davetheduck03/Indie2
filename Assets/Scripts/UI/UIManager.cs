using System.Collections;
using UnityEngine;
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
        else
        {
            Debug.LogError("Player instance not found!");
            StartCoroutine(WaitForPlayerInstance());
        }
    }

    private IEnumerator WaitForPlayerInstance()
    {
        while (Player.Instance == null)
        {
            yield return null;
        }

        Player.Instance.OnHealthChanged += UpdateHealthBar;
        Player.Instance.OnHungerChanged += UpdateHungerBar;

        UpdateHealthBar(Player.Instance.playerHealth / 100f);
        UpdateHungerBar(Player.Instance.playerHunger / 100f);
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
        if (!inventoryUI.activeSelf)
        { inventoryUI.SetActive(true); }
        else 
        { inventoryUI.SetActive(false); }
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