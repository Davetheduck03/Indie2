using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float savedHealth;
    public float savedHunger;
    public bool hasSavedData = false;
    public GameObject savedPrimaryWeaponPrefab;
    public GameObject savedSecondaryWeaponPrefab;
    public static bool isExiting = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SavePlayerStats(float health, float hunger)
    {
        savedHealth = health;
        savedHunger = hunger;
        hasSavedData = true;
    }

    void OnApplicationQuit()
    {
        isExiting = true;
    }

}