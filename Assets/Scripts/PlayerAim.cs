using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player Player;
    private InputSystem_Actions controls;
    private Vector2 aimInput;


    [Header("Aim Setting")]
    [SerializeField] private float minCameraDistance =1.4f;
    [SerializeField] private float maxCameraDistance =4f;
    [SerializeField] private float aimSensetitvity = 5f;
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private Transform aim;
    void Start()
    {
        Player = GetComponent<Player>();
        AssignInputEvents();
    }
    private void Update()
    {
        aim.position = Vector3.Lerp(aim.position, DesieredAimPosition(), Time.deltaTime * aimSensetitvity);
    }

    private Vector3 DesieredAimPosition()
    {
        float actualMaxCameraDistance;
        bool moveingDownWards = Player.movement.movementInput.y < -0.5f;
        if (moveingDownWards)
        {
            actualMaxCameraDistance = minCameraDistance;
        }
        else
        {
            actualMaxCameraDistance = maxCameraDistance;
        }
        Debug.Log("actualMaxCameraDistance: " + actualMaxCameraDistance);
        Vector3 desiredAimPosition = GetMousePostion();
        Vector3 aimDirection = (desiredAimPosition - Player.transform.position).normalized;
        float distanceToDesiredPosition = Vector3.Distance(Camera.main.transform.position, Player.transform.position);
        float calmpedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, maxCameraDistance);
        desiredAimPosition = Player.transform.position + aimDirection * calmpedDistance;
        desiredAimPosition.y = transform.position.y+1;

        return desiredAimPosition;
    }

    public Vector3 GetMousePostion()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    private void AssignInputEvents()
    {
        controls = Player.controls;
        controls.Player.Look.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => aimInput = Vector2.zero;
    }
}
