using UnityEngine;

public class Player : MonoBehaviour
{

    public InputSystem_Actions controllers;
    private void Awake()
    {
        controllers = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        controllers.Enable();
    }
    private void OnDisable()
    {
        controllers.Disable();
    }
}
