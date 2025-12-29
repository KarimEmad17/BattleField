using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
   private PlayerWeaponVisuals visualController;
    void Start()
    {
        visualController = GetComponent<PlayerWeaponVisuals>();
    }

    public void ReoloadIsOver()
    {
        visualController.MaximizeRigWeight();
        //refile bullet
    }

    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        visualController.MaximizeLeftHandIKIncrease();
    }
    public void GrabWeaponIsOver()
    {
       
        visualController.SetBusyGrabingWeapon(false);
    }
}
