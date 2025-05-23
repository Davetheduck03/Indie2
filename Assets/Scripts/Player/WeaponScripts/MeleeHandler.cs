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
    [SerializeField] private float cooldownTime = 1f;
    private bool canShoot;
    private float nextAttackTime = 0f;
    public ParticleSystem hitEffect;
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private string weaponName = "Pistol";
    [SerializeField] private GameObject pickupPrefab;

    public void Shoot(Vector3 shootPoint, Transform pivotPoint)
    {

        if (!canShoot) { return; }
        hitEffect = Player.Instance.GetComponentInChildren<ParticleSystem>();
        hitEffect.Play();
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
        nextAttackTime = Time.time + cooldownTime;
    }

    public void ShootEnd()
    {
        canShoot = false;
    }

    public void ShootStart()
    {
        if (Time.time >= nextAttackTime)
        {
            canShoot = true;
        }
    }

    public GameObject GetPickupPrefab()
    {
        return pickupPrefab;
    }

    public Sprite GetWeaponIcon()
    {
        return weaponIcon;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }
}