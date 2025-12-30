using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;


    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;
    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Player.Attack.performed += ctx => Shoot();
    }
    private void Shoot()
    {
       
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;
        Destroy(newBullet, 10f);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    public Transform GunPoint()
    {
        return gunPoint;
    }
    public Vector3 BulletDirection()
    {

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
}
