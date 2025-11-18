using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    [SerializeField] private CharacterController characterController;
    private Animator animator;

    [Header("Inputs")]
    private InputSystem_Actions controllers;
    private Vector2 movementInput;
    private Vector2 mouseInput;

    [Header("MovementSetting")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3f;
    private float Speed;

    [Header("Animation Data")]
    private bool isRunning;
    private Vector3 MoveDirection;

    [Header("Aim Setting")]
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private Transform aim;

    [Header("Gravity")]
    private float velocityY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    }


    private void Start()
    {
        
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        Speed = walkSpeed;
        
        AssignInputEvents();
    }

    // Update is called once per frame
    void Update()
    {
       
        ApplyMovement();
        ApplyAim();
        AnimatorController();
    }

    

    private void AssignInputEvents()
    {
        controllers = player.controllers;
        controllers.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controllers.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        controllers.Player.Look.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controllers.Player.Look.canceled += ctx => mouseInput = Vector2.zero;
        controllers.Player.Sprint.performed += ctx =>
        {

            isRunning = true;
            Speed = runSpeed;


        };
        controllers.Player.Sprint.canceled += ctx =>
        {
            isRunning = false;
            Speed = walkSpeed;
        };
       
    }
    private void AnimatorController()
    {
        float xVelocity = Vector3.Dot(MoveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(MoveDirection.normalized, transform.forward);
        animator.SetFloat("xVelocity", xVelocity,0.1f,Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity,0.1f,Time.deltaTime);
        bool PlayAnimationRunning = MoveDirection.magnitude > 0f && isRunning;    
        animator.SetBool("IsRunning", PlayAnimationRunning);
        
    }
    private void ApplyAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);


        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            Vector3 lookAtTarget = (hitInfo.point - transform.position);
            lookAtTarget.y = 0;
            lookAtTarget.Normalize();
            transform.forward = lookAtTarget;
            aim.position = new Vector3(hitInfo.point.x, transform.position.y+1, hitInfo.point.z);

        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            velocityY += Physics.gravity.y * Time.deltaTime;
            MoveDirection.y = velocityY;
        }
        else
        {
            velocityY = -0.5f;
        }
    }

    private void ApplyMovement()
    {
       
        MoveDirection = new Vector3(movementInput.x, 0, movementInput.y);
         ApplyGravity();
        if (MoveDirection.magnitude > 0)
        {
            characterController.Move(MoveDirection * Time.deltaTime * Speed);
        }
    }
   
}
