using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player Player;
    private InputSystem_Actions controls;
    private Vector2 aimInput;
    [Header("Aim Visual - Laser")]
    [SerializeField] private LineRenderer aimLaser;
    [Header("Aiming control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingprecisely = false;
    [SerializeField] private bool isLookingAtTarget = false;
    [Header("Camera Setting")]
    [SerializeField] private Transform cameraTarget;
    [Range(0.5f ,1)]
    [SerializeField] private float minCameraDistance =1.4f;
    [Range(1,3)]
    [SerializeField] private float maxCameraDistance =4f;
    [Range(3,5)]
    [SerializeField] private float aimSensetitvity = 5f;
    
    [Space]
    [SerializeField] private LayerMask aimLayerMask;

    private RaycastHit lastKnownMouseHit;
    void Start()
    {
        Player = GetComponent<Player>();
        AssignInputEvents();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isAimingprecisely = !isAimingprecisely;
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            isLookingAtTarget = !isLookingAtTarget;
        }
        UpdateAimLaser();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimLaser()
    {
        Transform gunPoint = Player.weapon.GunPoint();
        Vector3 laserDirection = Player.weapon.BulletDirection();
        float gunDistance = 4f;
        aimLaser.SetPosition(0, gunPoint.position);
        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;
        aimLaser.SetPosition(1, endPoint);
    }

    public Transform Target()
    {
        Transform target = null;
        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;

        }
        return target;
    }
    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), Time.deltaTime * aimSensetitvity);
    }

    private void UpdateAimPosition()
    {
        Transform target = Target();
        if (target != null && isLookingAtTarget)
        {
            aim.position = target.position;
            return;
        }


        aim.position = GetMouseHitInfo().point;
        if (!isAimingprecisely)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }

    public bool IsAimingPrecisely()
    {
        return isAimingprecisely;
    }
    private Vector3 DesieredCameraPosition()
    {
        float actualMaxCameraDistance = Player.movement.movementInput.y <-0.5f ? minCameraDistance : maxCameraDistance;
        Debug.Log("actualMaxCameraDistance: " + actualMaxCameraDistance);
        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - Player.transform.position).normalized;
        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float calmpedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);
        desiredCameraPosition = Player.transform.position + aimDirection * calmpedDistance;
        desiredCameraPosition.y = transform.position.y+1;

        return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }
        return lastKnownMouseHit;
    }
    private void AssignInputEvents()
    {
        controls = Player.controls;
        controls.Player.Look.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => aimInput = Vector2.zero;
    }
}
