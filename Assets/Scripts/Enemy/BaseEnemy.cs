using System.Collections;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public class BaseEnemy : MonoBehaviour
{
    public static BaseEnemy Instance { get; private set; }

    public Animator anim;
    public float health;
    public float speed;
    public float damage;
    public float coin;

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
    public virtual void Initialize(float health, float speed, float damage, float coin)
    {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
        this.coin = coin;
    }

    private void Start()
    {
        Initialize(health, speed, damage, coin);
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
            StartCoroutine(Die());
        }
    }
    public IEnumerator Die()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(0.433f);
        Destroy(gameObject);
    }

    private void FindTarget()
    {
        
    }

}
