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
        primaryWeapon = null;
        secondaryWeapon = null;
        currentSpeed = playerSpeed;
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        playerHealth = 100f;
        playerHunger = 10000f;
        hungerDrain = true;
        StartCoroutine(HungerDrain());
        OnHealthChanged?.Invoke(playerHealth / 100f);
        OnHungerChanged?.Invoke(playerHunger / 100f);
        m_ShootingButtonHandler.onPointerDown += OnShootButtonDown;
        m_ShootingButtonHandler.onPointerUp += OnShootButtonUp;
    }


    void FixedUpdate()
    {
        if (joystickScript != null)
        {
            moveDirection = new Vector2(joystickScript.joystickVec.x * currentSpeed, joystickScript.joystickVec.y * currentSpeed);
            playerRb.velocity = moveDirection;
        }

        else
        {
            playerRb.velocity = Vector2.zero;
        }
        CalculateArrowDirection();
        ProcessInput();
        Animate();
    }

    void Animate()
    {

        if (joystickScript.joystickVec.magnitude > 0)
        {
            anim.SetFloat("MoveX", joystickScript.joystickVec.x);
            anim.SetFloat("MoveY", joystickScript.joystickVec.y);
            anim.SetFloat("MoveMagnitude", joystickScript.joystickVec.magnitude);


            lastMoveDirection = joystickScript.joystickVec;
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
        if (joystickScript.joystickVec.magnitude > 0)
        {
            lastMoveDirection = joystickScript.joystickVec;
        }
    }



    public void PickupWeapon(IWeapon newWeapon, GameObject pickupPrefab)
    {
        if (primaryWeapon == null)
        {
            primaryWeapon = newWeapon;
            primaryWeaponPickupPrefab = pickupPrefab;
            m_ShootingHandler.SetWeapon(primaryWeapon);
        }
        else if (secondaryWeapon == null)
        {
            secondaryWeapon = newWeapon;
            secondaryWeaponPickupPrefab = pickupPrefab;
        }
        else
        {
            DropWeapon(secondaryWeaponPickupPrefab);
            secondaryWeapon = newWeapon;
            secondaryWeaponPickupPrefab = pickupPrefab;
        }
    }

    private void DropWeapon(GameObject pickupPrefab)
    {
        if (pickupPrefab != null)
        {
            Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        }
    }


    public void SwitchWeapons()
    {
        (primaryWeapon, secondaryWeapon) = (secondaryWeapon, primaryWeapon);
        (primaryWeaponPickupPrefab, secondaryWeaponPickupPrefab) = (secondaryWeaponPickupPrefab, primaryWeaponPickupPrefab);
        m_ShootingHandler.SetWeapon(primaryWeapon);
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

    private void PlayerDead()
    {
        SceneManager.LoadScene("Mall Level 1");
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
            playerHunger -= 1;
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

    [SerializeField] private IWeapon primaryWeapon;
    [SerializeField] private IWeapon secondaryWeapon;
    private GameObject primaryWeaponPickupPrefab;
    private GameObject secondaryWeaponPickupPrefab;
    private bool isShootingButtonHolding = false;
    public GameObject switchButton;

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
