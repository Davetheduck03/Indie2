using UnityEngine;

public class NormalEnemy : BaseEnemy
{
    [Header("Charge Settings")]
    public float chargeSpeed = 10f;       // Speed during charge
    public float chargeDuration = 0.5f;   // How long the charge lasts
    public float chargeCooldown = 2f;     // Cooldown between charges

    private bool isCharging = false;
    private Vector2 chargeDirection;
    private float chargeStartTime;
    private float lastChargeTime = -Mathf.Infinity;

    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(50f, 1f, 20);
    }

    private void Update()
    {
        if (!isCharging)
        {
            bool targetFound = FindTarget();
            if (targetFound)
            {
                path.canMove = true;
                path.destination = playerPos.position;
                distanceToTarget = Vector2.Distance(transform.position, playerPos.position);

                if (distanceToTarget <= stoppingThreshold)
                {
                    path.canMove = false;
                    Attack();
                }
            }
            else
            {
                path.canMove = false;
            }

            if (path.velocity.magnitude > 0.1f)
            {
                Flip(path.velocity.normalized);
            }
        }
        else
        {
            if (Time.time - chargeStartTime >= chargeDuration)
            {
                StopCharge();
            }
            else
            {
                rb.velocity = chargeDirection * chargeSpeed;
                Flip(chargeDirection);
            }
        }
    }

    public override void Attack()
    {
        if (!isCharging && Time.time >= lastChargeTime + chargeCooldown)
        {
            anim.SetTrigger("Attack");
            isCharging = true;
            chargeStartTime = Time.time;
            path.canMove = false;
            chargeDirection = (playerPos.position - transform.position).normalized;
            rb.velocity = chargeDirection * chargeSpeed;
            lastChargeTime = Time.time;
        }
    }

    private void StopCharge()
    {
        isCharging = false;
        rb.velocity = Vector2.zero;
        path.canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.Instance.TakeDamage(damage);
            }
            StopCharge();
        }
    }
}