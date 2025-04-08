using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : BaseEnemy
{
    [Header("AoE Attack Settings")]
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private float chargeDuration = 1.5f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private GameObject aoeIndicatorPrefab;

    [SerializeField] private Color startColor = new Color(1, 0, 0, 0.2f);
    [SerializeField] private Color endColor = Color.red;

    private float nextAttackTime;
    private GameObject activeIndicator;
    private SpriteRenderer indicatorRenderer;
    private float chargeStartTime;
    private bool isCharging;


    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(80f, 0.7f, 10);
        stoppingThreshold = attackRadius * 0.9f;
    }

    private void Update()
    {
    
            if (isCharging)
            {
                HandleChargeProgress();
                return;
            }

            bool targetFound = FindTarget();
            if (targetFound)
            {
                UpdateMovement();

                if (ShouldStartAttack())
                {
                    StartCharge();
                }
            }
            else
            {
                path.canMove = false;
            }
    }


    private void UpdateMovement()
    {
        path.canMove = true;
        path.destination = playerPos.position;
        distanceToTarget = Vector2.Distance(transform.position, playerPos.position);

        if (path.velocity.magnitude > 0.1f)
        {
            Flip(path.velocity.normalized);
        }
    }

    private bool ShouldStartAttack()
    {
        return distanceToTarget <= stoppingThreshold &&
               Time.time >= nextAttackTime;
    }

    private void StartCharge()
    {
        anim.SetTrigger("Attack");
        isCharging = true;
        path.canMove = false;
        chargeStartTime = Time.time;
        CreateAoeIndicator();
    }

    private void CreateAoeIndicator()
    {
        if (aoeIndicatorPrefab != null)
        {
            activeIndicator = Instantiate(aoeIndicatorPrefab, transform.position, Quaternion.identity);
            activeIndicator.transform.SetParent(transform);
            activeIndicator.transform.localPosition = Vector3.zero;


            float diameter = attackRadius * 2;
            activeIndicator.transform.localScale = new Vector3(diameter, diameter, 1);


            indicatorRenderer = activeIndicator.GetComponent<SpriteRenderer>();
            if (indicatorRenderer != null)
            {
                indicatorRenderer.color = startColor;
            }
        }
    }

    private void HandleChargeProgress()
    {
        if (Time.time - chargeStartTime >= chargeDuration)
        {
            ExecuteAttack();
            return;
        }

        UpdateIndicatorVisual();
    }

    private void UpdateIndicatorVisual()
    {
        if (indicatorRenderer != null)
        {
            float progress = Mathf.Clamp01((Time.time - chargeStartTime) / chargeDuration);

            indicatorRenderer.color = Color.Lerp(startColor, endColor, progress);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (isCharging)
        {
            CleanUp();
            nextAttackTime = chargeStartTime + chargeDuration + attackCooldown;
        }
    }

    private void ExecuteAttack()
    {
        if (indicatorRenderer != null)
        {
            indicatorRenderer.color = endColor;
            Destroy(activeIndicator, 0.1f);
        }

        ApplyDamage();
        CleanUp();
        ResetAttackCooldown();
    }


    private void ApplyDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Player.Instance.TakeDamage(damage);
            }
        }
    }

    private void CleanUp()
    {
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
        }
        isCharging = false;
    }

    private void ResetAttackCooldown()
    {
        nextAttackTime = Time.time + attackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
