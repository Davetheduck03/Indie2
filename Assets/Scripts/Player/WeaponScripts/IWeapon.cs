
using UnityEngine;

public interface IWeapon
{
    public void ShootStart();
    public void Shoot(Vector3 shootPoint, Transform pivotPoint);
    public void ShootEnd();
    GameObject GetPickupPrefab();
    Sprite GetWeaponIcon();
    string GetWeaponName();
}