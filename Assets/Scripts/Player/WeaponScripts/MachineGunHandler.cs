using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunHandler : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject m_Bullet;
    private bool canShoot;

    public void Shoot(Vector3 shootPoint, Transform pivotPoint)
    {
        if (!canShoot)
            return;

        GameObject bullet = Instantiate(m_Bullet, shootPoint, pivotPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(pivotPoint.up * 15f, ForceMode2D.Impulse);
    }

    public void ShootEnd()
    {
        canShoot = false;
    }

    public void ShootStart()
    {
        canShoot = true;
    }
}
