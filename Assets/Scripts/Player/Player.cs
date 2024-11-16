using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public MovementJoystick joystickScript;
    private Rigidbody2D playerRb;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerHunger;
    [SerializeField] private float playerMental;
    [SerializeField] private float playerHealth;
    [SerializeField] private bool hungerDrain;
    [SerializeField] private bool mentalDrain;
    private Vector2 moveDirection;
    [SerializeField] public Transform m_ShootingPoint;
    [SerializeField] public Transform triangle;


    public GameObject bulletPrefab;


    Animator anim;
    private Vector2 lastMoveDirection;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
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
        playerSpeed = 3f;
        playerHealth = 4;
        playerHunger = 100;
        playerMental = 100;
        mentalDrain = true;
        hungerDrain = true;
        StartCoroutine(HungerDrain());
        StartCoroutine(MentalDrain());
    }


    void FixedUpdate()
    {
        if(joystickScript != null)
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


    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, triangle.position, m_ShootingPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(m_ShootingPoint.up * 15f, ForceMode2D.Impulse);
    }

    private void CalculateArrowDirection()
    {
        if(moveDirection == Vector2.zero)
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
        if(playerHealth <= 0 || playerHunger == 0 || playerMental == 0)
        {
            return;
        }
    }

    private void AddHealth()
    {
        playerHealth++;
        if(playerHealth > 4) 
        {
            playerHealth = 4;
        }
    }

    public void TakeDamage(float damage)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damage;
        }
    }

    public void AddPlayerHunger()
    {

    }

    public void AddPlayerMental() 
    {

    }

    public IEnumerator HungerDrain()
    {
        while (hungerDrain == true)
        {
            playerHunger--;
            yield return new WaitForSeconds(2f);
        }
    }

    public IEnumerator MentalDrain()
    {
        while (mentalDrain == true)
        {
            playerMental--;
            yield return new WaitForSeconds(5f);
        }
    }
}
