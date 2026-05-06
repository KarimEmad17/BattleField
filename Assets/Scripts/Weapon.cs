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
    public int ammo;
    public int maxAmmo;
}
