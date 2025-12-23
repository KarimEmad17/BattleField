using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
   private WeaponVisualController visualController;
    void Start()
    {
        visualController = GetComponent<WeaponVisualController>();
    }

    public void ReoloadIsOver()
    {
        visualController.EnableRigIncrease();
        //refile bullet
    }

    public void ReturnRig()
    {
        visualController.EnableRigIncrease();
        visualController.EnableLeftHandIKIncrease();
    }
    public void GrabWeaponIsOver()
    {
       
        visualController.SetBusyGrabingWeapon(false);
    }
}
