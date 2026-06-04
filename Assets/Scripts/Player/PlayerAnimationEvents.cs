using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
   private PlayerWeaponVisuals visualController;
   private PlayerWeaponController weaponController;

    void Start()
    {
        visualController = GetComponent<PlayerWeaponVisuals>();
        weaponController = GetComponent<PlayerWeaponController>();
    }

    public void ReoloadIsOver()
    {
        weaponController.SetWeaponReady(true);
        visualController.MaximizeRigWeight();
        weaponController.CurrentWeapon().ReloadBullets();
        //refile bullet
    }

    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        visualController.MaximizeLeftHandIKIncrease();
    }
    public void GrabWeaponIsOver()
    {
        weaponController.SetWeaponReady(true);
        visualController.SetBusyGrabingWeapon(false);
    }
    public void SwitchOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
}
