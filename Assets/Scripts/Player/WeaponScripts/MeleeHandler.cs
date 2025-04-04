using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHandler : MonoBehaviour, IWeapon
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damage = 5f;
    private bool canShoot;

    public void Shoot(Vector3 shootPoint, Transform pivotPoint)
    {
        if (!canShoot) {return;}
        Vector2 attackDirection = (shootPoint - pivotPoint.position).normalized;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            pivotPoint.position,
            attackRadius,
            attackDirection,
            attackRange,
            enemyLayer
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent<BaseEnemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
        canShoot = false;
    }


    public void ShootEnd()
    {
        canShoot = false;
    }

    public void ShootStart()
    {
        canShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * attackRange);
    }
}

