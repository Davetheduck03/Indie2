using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunHandler : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject m_Bullet;
    private bool canShoot;

    public void Shoot(Vector3 shootPoint, Transform pivotPoint)
    {
        ShootShotgun(3, 30f, shootPoint, pivotPoint);
    }

    public void ShootEnd()
    {

    }

    public void ShootStart()
    {
        canShoot = true;
    }

    private void ShootShotgun(int bulletCount, float spreadAngle, Vector3 shootPoint, Transform pivotPoint)
    {
        if (!canShoot)
            return;

        float startAngle = -spreadAngle / 2;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = pivotPoint.rotation * Quaternion.Euler(0, 0, angle);

            GameObject bullet = Instantiate(m_Bullet, shootPoint, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = rotation * Vector2.up * 15f;
            }
        }
        canShoot = false;
    }
}