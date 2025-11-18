using System.Runtime.CompilerServices;
using UnityEngine;
public class Player2 : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Gun Data")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed= 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    [Header("Aim")]
    public GameObject aimTower;
    public GameObject AimTarget;
    public float aimSpeed= 5f;
    [Header("Movement Data")]

    public float moveSpeed= 5f;
    public float turnSpeed= 5f;

    [Header("Inputs")]
    private float horizontalInput;
    private float verticalInput;

    public LayerMask aimLayerMask;

    private InputSystem_Actions controller;
    public InputType inputType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        controller = new InputSystem_Actions();
        controller.Player.Attack.performed += (ctx) => Shoot();
    }

   

    private void OnEnable()
    {
        controller.Enable();
       
    }
    private void OnDisable()
    {
        controller.Disable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
        CheckInputs();

    }

    

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();

    }
    private void Shoot()
    {
        Debug.Log("Pew Pew");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = firePoint.forward * bulletSpeed;
        Destroy(bullet, 7f);
    }
    private void CheckInputs()
    {
        switch (inputType)
        {
            case InputType.OldInputSystem :
                
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
                if (verticalInput < 0)
                    horizontalInput = -Input.GetAxis("Horizontal");
                break;
            case InputType.NewInputSystem :
               horizontalInput = controller.Player.Move.ReadValue<Vector2>().x;
                verticalInput = controller.Player.Move.ReadValue<Vector2>().y;
                if (verticalInput < 0)
                    horizontalInput = -controller.Player.Move.ReadValue<Vector2>().x;
                break;
        }
       
    }
    private void ApplyRotation()
    {
        transform.Rotate(0, horizontalInput * turnSpeed, 0);
        aimTower.transform.rotation = Quaternion.RotateTowards(aimTower.transform.rotation, Quaternion.LookRotation(AimTarget.transform.position - aimTower.transform.position), aimSpeed);
    }

    private void ApplyMovement()
    {
        Vector3 movement = transform.forward * verticalInput * moveSpeed;

        rb.linearVelocity = movement;
    }

    public void UpdateAim()
    {
        Ray ray = new Ray();
        switch (inputType)
        {
            case InputType.OldInputSystem:
                 ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                break;
            case InputType.NewInputSystem:
                 ray = Camera.main.ScreenPointToRay(controller.Player.Look.ReadValue<Vector2>());
                break;

        }
        
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, Mathf.Infinity, aimLayerMask))
        {
            AimTarget.transform.position = new Vector3( hit.point.x,AimTarget.transform.position.y,hit.point.z);
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
        }
    }
}
