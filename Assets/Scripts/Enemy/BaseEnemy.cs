using System.Collections;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEditor;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected Animator anim;

    public System.Action OnDeath;

    [Header("Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float coin;

    [Header("Detection")]
    [SerializeField] protected float detectionRadius = 12f;
    [SerializeField] protected LayerMask playerLayer;
    protected Transform playerPos;
    protected Vector2 movementDirection;
    protected Rigidbody2D rb;
    protected Vector2 direction;

    [Header("Pathfinding Components")]
    protected AIPath path;
    protected float distanceToTarget;
    [SerializeField] protected float stoppingThreshold;

    public virtual void Initialize(float health, float speed, float damage)
    {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
    }

    private void Start()
    {
        path = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();
        Initialize(health, speed, damage);
        path.enableRotation = false;
        path.maxSpeed = speed;
    }

    public bool FindTarget()
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (detectedPlayer != null)
        {
            playerPos = detectedPlayer.transform;
            return true;
        }
        playerPos = null;
        return false;
    }

    public virtual void Flip(Vector2 direction)
    {
        if (direction.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x > 0.1f) transform.localScale = Vector3.one;
    }

    public virtual void TakeDamage(float damage)
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
        if (OnDeath != null)
        {
            OnDeath();
        }
        path.canMove = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public void OnAnimationDeadEventEnd()
    {
        Destroy(gameObject);
    }

    public virtual void Attack()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingThreshold);
    }
}