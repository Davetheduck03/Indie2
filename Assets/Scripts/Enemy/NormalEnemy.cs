using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemy : BaseEnemy
{
    private readonly float cooldownTime = 2.5f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(100, 1f, 1);
    }

    
    public override void Attack()
    {
        if (Physics2D.OverlapCircle(transform.position, 4, playerLayer) && isOnCooldown == false)
        {
            Charge();
            isOnCooldown |= true;
            if (isOnCooldown)
            {
                cooldownTimer = cooldownTime;
                cooldownTimer -= Time.deltaTime; 
                if (cooldownTimer <= 0f)
                {
                    isOnCooldown = false; 
                    cooldownTimer = 0f;  
                }
            }
        }
        else
        {
            speed = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            
        }
    }

    private void Charge()
    {
        speed = 0.25f;
        rb.AddForce(direction * 10f);
    }

}
