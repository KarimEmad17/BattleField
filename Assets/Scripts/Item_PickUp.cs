using UnityEngine;

public class Item_PickUp : MonoBehaviour
{
    [SerializeField] private Weapon weaponToPickUp;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickUpWeapon(weaponToPickUp);
    }
}
