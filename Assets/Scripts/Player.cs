using UnityEngine;

public class Player : MonoBehaviour
{
    public InputSystem_Actions controls { get; private set; }
    public PlayerAim aim { get; private set; } // read-only-settings;
    public PlayerMovement movement { get; private set; } // read-write-settings;
    public PlayerWeaponController weapon { get; private set; }
    private void Awake()
    {
        controls = new InputSystem_Actions();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponController>();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
