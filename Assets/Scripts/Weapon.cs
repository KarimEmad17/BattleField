using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifel,
    Shotgun,
    Rifle
}




[System.Serializable] // make class visiable in inspector
public class Weapon
{
    public WeaponType weaponType;

    public int bulletInMagazine;

    public int magazineCapacity;

    public int totalReserveAmmo;

    [Range(1,3)]
    public float reloadSpeed = 1;
    [Range(1,3)]
    public float equipSpeed = 1;


    public bool CanShoot()
    {
        return HaveEnoughBullet();
    }

    private bool HaveEnoughBullet()
    {
        if (bulletInMagazine > 0)
        {
            bulletInMagazine--;
            return true;
        }
        Debug.Log("Out of Ammo");
        return false;
    }

    public bool CanReload()
    {
        return bulletInMagazine < magazineCapacity && totalReserveAmmo > 0;
    }


    public void ReloadBullets()
    {
        int bulletsToReload = magazineCapacity;

        if(bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletInMagazine = bulletsToReload;
    }
}
