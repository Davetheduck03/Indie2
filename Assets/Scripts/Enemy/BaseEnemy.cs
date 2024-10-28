using System.Collections;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public class BaseEnemy : MonoBehaviour
{
    public static BaseEnemy Instance { get; private set; }

    public float health;
    public float speed;
    public float damage;
    public float coin;

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
        if (health > 0) 
        {
            health -= damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
