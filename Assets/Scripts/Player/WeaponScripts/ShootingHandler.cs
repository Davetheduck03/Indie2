using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    [SerializeField] private Transform m_Triangle;
    [SerializeField] private Transform m_PivotPoint;
    private IWeapon activatedWeapon;

    public void SetWeapon(IWeapon weapon)
    {
        activatedWeapon = weapon;
    }

    public void OnShootStart()
    {
        if (activatedWeapon != null)
            activatedWeapon.ShootStart();
    }

    public void OnShoot()
    {
        if (activatedWeapon != null)
            activatedWeapon.Shoot(m_Triangle.position, m_PivotPoint);
    }

    public void OnShootEnd()
    {
        if (activatedWeapon != null)
            activatedWeapon.ShootEnd();
    }
}
