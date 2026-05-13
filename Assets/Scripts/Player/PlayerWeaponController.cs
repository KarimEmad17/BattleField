using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private const float REFRENCE_BULLET_SPEED = 20f;

    [SerializeField] private Weapon currentWeapon;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    //This Is the default Speed From which our mass

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlots;
    [SerializeField] private int maxWeaponSlots = 2;


    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
        currentWeapon.bulletInMagazine = currentWeapon.totalReserveAmmo;
    }

    #region Slots Management , Equip , picking up and dropping weapons

        private void EquipWeapon(int i)
        {
            currentWeapon = weaponSlots[i];
            player.weaponVisuals.SwitchOffWeaponModel();
            player.weaponVisuals.PlayWeaponGrabAnimation();
        }
        public void PickUpWeapon(Weapon newWeapon)
        {
            if (weaponSlots.Count >= maxWeaponSlots)
            {
                Debug.Log("Inventory Full");
                return;
            }
            weaponSlots.Add(newWeapon);
            currentWeapon = newWeapon;
        }
        private void DropWeapon()
        {
            if (HasOnlyOneWeapon())
                return;
            weaponSlots.Remove(currentWeapon);
            currentWeapon = weaponSlots[0];
            EquipWeapon(0);
        }

    #endregion
    private void Shoot()
    {
        if(!currentWeapon.CanShoot())
            return;
        
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        bulletRb.mass = REFRENCE_BULLET_SPEED / bulletSpeed; // Adjust mass to maintain
        bulletRb.linearVelocity = BulletDirection() * bulletSpeed;
        Destroy(newBullet, 10f);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;
    public Weapon CurrentWeapon() => currentWeapon;
    public Weapon BackUpWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
                return weapon;
        }
        return null;
    }
    public Transform GunPoint()
    {
        return gunPoint;
    }
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;
        if(!player.aim.IsAimingPrecisely() && player.aim.Target() == null)
            direction .y = 0;
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);
        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position ,weaponHolder.position + weaponHolder.forward * 25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gunPoint.position ,gunPoint.position + gunPoint.forward * 25f);
    }

    #region Input Events

        private void AssignInputEvents()
        {
            InputSystem_Actions controls = player.controls;
            controls.Player.Attack.performed += ctx => Shoot();
            controls.Player.Equipslot1.performed += ctx => EquipWeapon(0);
            controls.Player.Equipslot2.performed += ctx => EquipWeapon(1);
            controls.Player.DropCurrentWeapon.performed += ctx => DropWeapon();
            controls.Player.Reload.performed += ctx =>
            {
               if(currentWeapon.CanReload())
               {
                    player.weaponVisuals.PlayReloadAnimation();
               }
            };
    }
    #endregion
}
