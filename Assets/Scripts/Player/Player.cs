using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public MovementJoystick joystickScript;
    private Rigidbody2D playerRb;
    public float playerSpeed;
    public float playerHunger;
    public float playerHealth;
    [SerializeField] private bool hungerDrain;
    [SerializeField] private bool mentalDrain;
    private Vector2 moveDirection;
    public Transform m_ShootingPoint;
    public Transform triangle;
    [SerializeField] private ShootingHandler m_ShootingHandler;
    [SerializeField] private ShootingButtonHandler m_ShootingButtonHandler;

    public event Action<float> OnHealthChanged;
    public event Action<float> OnHungerChanged;

    public GameObject bulletPrefab;

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
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSpeed = 4f;
        playerHealth = 100f;
        playerHunger = 100f;
        mentalDrain = true;
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
            moveDirection = new Vector2(joystickScript.joystickVec.x * playerSpeed, joystickScript.joystickVec.y * playerSpeed);
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



    private void CalculateArrowDirection()
    {
        if (moveDirection == Vector2.zero)
        {
            m_ShootingPoint.gameObject.SetActive(false);
            return;
        }
        m_ShootingPoint.gameObject.SetActive(true);
        float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        m_ShootingPoint.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    private void PlayerDead()
    {
        SceneManager.LoadScene("Test");
    }

    private void AddHealth(float health)
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

    [SerializeField] private GameObject[] weapons;
    private bool isShootingButtonHolding = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_ShootingHandler.SetWeapon(weapons[0].GetComponent<IWeapon>());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_ShootingHandler.SetWeapon(weapons[1].GetComponent<IWeapon>());
        }        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_ShootingHandler.SetWeapon(weapons[2].GetComponent<IWeapon>());
        }

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
