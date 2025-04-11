using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunHandler : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject m_Bullet;
    private bool canShoot;
    private float nextAttackTime = 0f;
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private string weaponName = "Pistol";

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

    public void Shoot(Vector3 shootPoint, Transform pivotPoint)
    {
        if (!canShoot)
            return;

        GameObject bullet = Instantiate(m_Bullet, shootPoint, pivotPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(pivotPoint.up * 15f, ForceMode2D.Impulse);
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
}
