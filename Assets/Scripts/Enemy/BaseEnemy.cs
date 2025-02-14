using System.Collections;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

public class BaseEnemy : MonoBehaviour
{
    public static BaseEnemy Instance { get; private set; }

    protected enum EnemyState { Idle, Chase, Attack }

    EnemyState Idle;

    [SerializeField] protected Animator anim;
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float coin;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Transform playerPos;

    protected Vector2 movementDirection;
    protected Rigidbody2D rb;
    protected Vector2 direction;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 8);
        Gizmos.DrawWireSphere(transform.position, 4);
    }


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
    public virtual void Initialize(float health, float speed, float damage)
    {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Initialize(health, speed, damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            anim.SetTrigger("TakeDamage");
        }
        else
        {
            health = 0;
            Die();
        }
    }
    public void Die()
    {
        anim.SetTrigger("Die");
    }

    public void OnAnimationDeadEventEnd()
    {
        Destroy(gameObject);
    }

    public bool FindTarget()
    {
        if (Physics2D.OverlapCircle(transform.position, 8, playerLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Flip()
    {
        movementDirection.x = Mathf.Sin(Time.time);
        transform.Translate(movementDirection * speed * Time.deltaTime);
        if (movementDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void MoveTowardsPlayer()
    {
        direction = (playerPos.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
    }

    public virtual void Attack()
    {

    }
}
