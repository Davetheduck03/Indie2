using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public MovementJoystick joystickScript;
    private Rigidbody2D playerRb;
    public float playerSpeed = 4f;
    public float currentSpeed;
    public float playerHunger;
    public float playerHealth;
    [SerializeField] private bool hungerDrain;
    private Vector2 moveDirection;
    public Transform m_ShootingPoint;
    public Transform triangle;
    [SerializeField] private ShootingHandler m_ShootingHandler;
    [SerializeField] private ShootingButtonHandler m_ShootingButtonHandler;
    private Coroutine speedBoostRoutine;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnHungerChanged;
    private float remainingBoostTime;
    Animator anim;
    private Vector2 lastMoveDirection;
    public int hungerDrainAmount = 1;
    public int hungerDrainAmountLarge = 2;
    public bool inColdZone = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        playerDied = false;
        primaryWeapon = null;
        secondaryWeapon = null;
        currentSpeed = playerSpeed;
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();

        if (GameManager.Instance != null && GameManager.Instance.hasSavedData)
        {
            playerHealth = GameManager.Instance.savedHealth;
            playerHunger = GameManager.Instance.savedHunger;
            if (GameManager.Instance.savedPrimaryWeaponPrefab != null)
            {
                PickUp primaryPickup = GameManager.Instance.savedPrimaryWeaponPrefab.GetComponent<PickUp>();
                if (primaryPickup != null && primaryPickup.weaponPrefab != null)
                {
                    GameObject weaponObj = Instantiate(primaryPickup.weaponPrefab, transform);
                    IWeapon weapon = weaponObj.GetComponent<IWeapon>();
                    if (weapon != null)
                        PickupWeapon(weapon);
                }
            }
            if (GameManager.Instance.savedSecondaryWeaponPrefab != null)
            {
                PickUp secondaryPickup = GameManager.Instance.savedSecondaryWeaponPrefab.GetComponent<PickUp>();
                if (secondaryPickup != null && secondaryPickup.weaponPrefab != null)
                {
                    GameObject weaponObj = Instantiate(secondaryPickup.weaponPrefab, transform);
                    IWeapon weapon = weaponObj.GetComponent<IWeapon>();
                    if (weapon != null)
                        PickupWeapon(weapon);
                }
            }
        }
        else
        {
            playerHealth = 100f;
            playerHunger = 100f;
        }

        hungerDrain = true;
        StartCoroutine(HungerDrain());
        OnHealthChanged?.Invoke(playerHealth / 100f);
        OnHungerChanged?.Invoke(playerHunger / 100f);
        m_ShootingButtonHandler.onPointerDown += OnShootButtonDown;
        m_ShootingButtonHandler.onPointerUp += OnShootButtonUp;
    }

    public void PlayerDead()
    {
        playerDied = true;
        GameManager.Instance.hasSavedData = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private Vector2 currentMovementInput;
    void FixedUpdate()
    {
        Vector2 keyboardInputRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 keyboardInput = keyboardInputRaw.normalized;

        if (keyboardInputRaw != Vector2.zero)
        {
            // Use keyboard input
            currentMovementInput = keyboardInput;
            moveDirection = currentMovementInput * currentSpeed;
        }
        else if (joystickScript != null && joystickScript.joystickVec != Vector2.zero)
        {
            // Use joystick input
            currentMovementInput = joystickScript.joystickVec;
            moveDirection = currentMovementInput * currentSpeed;
        }
        else
        {
            // No input
            currentMovementInput = Vector2.zero;
            moveDirection = Vector2.zero;
        }

        playerRb.velocity = moveDirection;
        CalculateArrowDirection();
        ProcessInput();
        Animate();
    }

    void Animate()
    {

        if (currentMovementInput.magnitude > 0)
        {
            anim.SetFloat("MoveX", currentMovementInput.x);
            anim.SetFloat("MoveY", currentMovementInput.y);
            anim.SetFloat("MoveMagnitude", currentMovementInput.magnitude);
            lastMoveDirection = currentMovementInput;
        }
        else
        {
            anim.SetFloat("MoveX", lastMoveDirection.x);
            anim.SetFloat("MoveY", lastMoveDirection.y);
            anim.SetFloat("MoveMagnitude", 0);
        }

        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }


    void ProcessInput()
    {
        if (currentMovementInput.magnitude > 0)
        {
            lastMoveDirection = currentMovementInput;
        }
    }

    public IWeapon primaryWeapon;
    public IWeapon secondaryWeapon;
    [SerializeField] private GameObject primaryWeaponPickupPrefab;
    [SerializeField] private GameObject secondaryWeaponPickupPrefab;
    private bool isShootingButtonHolding = false;
    public GameObject switchButton;
    [SerializeField] private bool playerDied;

    public event Action<IWeapon, IWeapon> OnWeaponsChanged;

    private void UpdateWeaponUI()
    {
        OnWeaponsChanged?.Invoke(primaryWeapon, secondaryWeapon);
    }

    public void PickupWeapon(IWeapon newWeapon)
    {
        if (primaryWeapon == null)
        {
            primaryWeapon = newWeapon;
            m_ShootingHandler.SetWeapon(primaryWeapon);
            SetWeaponActive(primaryWeapon, true);
        }
        else if (secondaryWeapon == null)
        {
            secondaryWeapon = newWeapon;
            SetWeaponActive(secondaryWeapon, false);
        }
        else
        {
            DropWeapon(false);
            secondaryWeapon = newWeapon;
            SetWeaponActive(secondaryWeapon, false);
        }
        UpdateWeaponUI();
    }

    private void DropWeapon(bool dropPrimary)
    {
        IWeapon weaponToDrop = dropPrimary ? primaryWeapon : secondaryWeapon;

        if (weaponToDrop != null)
        {
            GameObject pickupPrefab = weaponToDrop.GetPickupPrefab();
            if (pickupPrefab != null)
            {
                Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            }

            MonoBehaviour weaponInstance = weaponToDrop as MonoBehaviour;
            if (weaponInstance != null)
            {
                Destroy(weaponInstance.gameObject);
            }

            if (dropPrimary) primaryWeapon = null;
            else secondaryWeapon = null;
        }
        UpdateWeaponUI();
    }

    private void SetWeaponActive(IWeapon weapon, bool active)
    {
        MonoBehaviour weaponBehaviour = weapon as MonoBehaviour;
        if (weaponBehaviour != null)
        {
            weaponBehaviour.gameObject.SetActive(active);
        }
    }


    public void SwitchWeapons()
    {
        if (secondaryWeapon == null) return;

        IWeapon temp = primaryWeapon;
        primaryWeapon = secondaryWeapon;
        secondaryWeapon = temp;

        m_ShootingHandler.SetWeapon(primaryWeapon);
        UpdateWeaponUI();
    }

    private void CalculateArrowDirection()
    {
        if (moveDirection == Vector2.zero)
        {

            return;
        }
        m_ShootingPoint.gameObject.SetActive(true);
        float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        m_ShootingPoint.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    public void AddHealth(float health)
    {
        if(playerHealth >= 100)
        {
            playerHealth = 100;
        }
        else
        {
            playerHealth += health;
            OnHealthChanged?.Invoke(playerHealth / 100f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damage;
            OnHealthChanged?.Invoke(playerHealth / 100f);
            Camera camera = Camera.main;
            Shake shake = camera.GetComponent<Shake>();
            shake.startShake = true;
            if(playerHealth <= 0)
            {
                PlayerDead();
            }
        }
    }

    public void AddPlayerHunger(int hunger)
    {
        playerHunger = Mathf.Clamp(playerHunger + hunger, 0, 100);
        OnHungerChanged?.Invoke(playerHunger / 100f);
    }

    public void SpeedModifier(float amount, float time)
    {
        if (amount > currentSpeed / playerSpeed)
        {
            currentSpeed = playerSpeed * amount;
        }

        remainingBoostTime += time;

        if (speedBoostRoutine == null)
            speedBoostRoutine = StartCoroutine(SpeedBoostCountdown());
    }

    public IEnumerator HungerDrain()
    {
       while (hungerDrain == true)
        {
            if (inColdZone == true)
            {
                playerHunger -= hungerDrainAmountLarge;
            }
            else
            {
                playerHunger -= hungerDrainAmount;
            }
            OnHungerChanged?.Invoke(playerHunger / 100f);
            yield return new WaitForSeconds(2f);
            if (playerHunger <= 0)
            {
                PlayerDead();
            }
        }
    }

    private IEnumerator SpeedBoostCountdown()
    {
        while (remainingBoostTime > 0)
        {
            remainingBoostTime -= Time.deltaTime;
            yield return null;
        }
        currentSpeed = playerSpeed;
        speedBoostRoutine = null;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && !GameManager.isExiting && !playerDied)
        {
            GameManager.Instance.savedHealth = playerHealth;
            GameManager.Instance.savedHunger = playerHunger;
            GameManager.Instance.savedPrimaryWeaponPrefab = primaryWeapon != null ? primaryWeapon.GetPickupPrefab() : null;
            GameManager.Instance.savedSecondaryWeaponPrefab = secondaryWeapon != null ? secondaryWeapon.GetPickupPrefab() : null;
            GameManager.Instance.hasSavedData = true;
        }
    }


    private void Update()
    {
        if(primaryWeapon != null && secondaryWeapon != null)
        { switchButton.SetActive(true);}
        else { switchButton.SetActive(false);}

        
        if (isShootingButtonHolding)
        {
            m_ShootingHandler.OnShoot();
        }
    }

    private void OnShootButtonDown()
    {
        m_ShootingHandler.OnShootStart();
        isShootingButtonHolding = true;
    }

    private void OnShootButtonUp()
    {
        m_ShootingHandler.OnShootEnd();
        isShootingButtonHolding = false;
    }

}
