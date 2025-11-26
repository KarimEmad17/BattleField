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
}
