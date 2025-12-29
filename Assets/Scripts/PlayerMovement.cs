using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    [SerializeField] private CharacterController characterController;
    private Animator animator;

    [Header("Inputs")]
    private InputSystem_Actions controllers;
    public Vector2 movementInput { get; private set; }
    private Vector2 mouseInput;

    [Header("MovementSetting")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 MoveDirection;
    private float Speed;

    [Header("Animation Data")]
    private bool isRunning;


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
        ApplyRotation();
        AnimatorController();
    }



    private void AssignInputEvents()
    {
        controllers = player.controls;
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
        animator.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);
        bool PlayAnimationRunning = MoveDirection.magnitude > 0f && isRunning;
        animator.SetBool("IsRunning", PlayAnimationRunning);
    }
    private void ApplyRotation()
    {
        Vector3 lookAtTarget = (player.aim.GetMousePostion() - transform.position);
        lookAtTarget.y = 0;
        lookAtTarget.Normalize();
        Quaternion desiredRotation = Quaternion.LookRotation(lookAtTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
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
