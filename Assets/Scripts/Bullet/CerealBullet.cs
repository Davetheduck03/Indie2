using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerealbullet : MonoBehaviour
{
    public float damage = 10f;
    public float splitDamage = 5f;
    public GameObject bulletPrefab;

    public void Start()
    {
        SelfDestruct();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BaseEnemy enemyScript = collision.gameObject.GetComponent<BaseEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

    private void Spread()
    {

    }

    private void SelfDestruct()
    {
        Destroy(gameObject, 3);
    }
}
