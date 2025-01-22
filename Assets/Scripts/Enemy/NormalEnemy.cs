using System.Collections;
using UnityEngine;

public class NormalEnemy : BaseEnemy
{
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeCooldown;

    private enum EnemyState { Idle, Following, Charging }
    private EnemyState currentState = EnemyState.Idle;
    private Vector2 chargeTarget;
    private bool isCharging = false;
    private float originalSpeed;

    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(100, 1f, 1);
        originalSpeed = this.speed;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                rb.velocity = Vector2.zero;
                if (FindTarget())
                {
                    currentState = EnemyState.Following;
                }
                break;

            case EnemyState.Following:
                if (FindTarget())
                {
                    MoveTowardsPlayer();
                    Flip();
                    if (ReadyToCharge())
                    {
                        StartCharge();
                    }
                }
                else
                {
                    currentState = EnemyState.Idle;
                }
                break;

            case EnemyState.Charging:
                PerformCharge();
                break;
        }
    }

    private bool ReadyToCharge()
    {
        return Vector2.Distance(transform.position, playerPos.position) <= 4 && !isCharging;
    }

    private void StartCharge()
    {
        isCharging = true;
        chargeTarget = playerPos.position;
        speed *= chargeSpeedMultiplier;
        currentState = EnemyState.Charging;
    }

    private void PerformCharge()
    {
        transform.position = Vector2.MoveTowards(transform.position, chargeTarget, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, chargeTarget) < 0.1f || HitPlayer())
        {
            StopCharge();
        }
    }

    private bool HitPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(damage);
        }
        return false;
    }

    private void StopCharge()
    {
        isCharging = false;
        speed = originalSpeed;
        currentState = EnemyState.Following;
        StartCoroutine(ChargeCooldown());
    }

    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        isCharging = false;
    }
}
