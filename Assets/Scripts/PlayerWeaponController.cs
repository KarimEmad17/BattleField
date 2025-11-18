using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        player.controllers.Player.Attack.performed += ctx => Shoot();
    }
    private void Shoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}
