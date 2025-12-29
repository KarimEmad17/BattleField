using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isGrabingWeapon = false;
    #region Gun Transforms 
    [SerializeField] private Transform[] gunsTransform;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;
    private Transform currentGun;
    #endregion

    [Header("Rig")]
    [SerializeField] private float rigWeightincreaseRate = 2f;
    private bool shouldIncrease_RigWeight = false;
    private Rig rig;
    [Header("Left hand IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private float leftHandIKWeightSpeed = 2f;
    private bool shouldIncrease_LeftHandIKWeight = false;

    private void Awake()
    {
        rig = GetComponentInChildren<Rig>();
    }
    private void Start()
    {
        SwitchOn(pistol);
       
    }

    private void Update()
    {
        CheckWeaponSwitch();
        if (Input.GetKeyDown(KeyCode.R) && isGrabingWeapon == false)
        {
            anim.SetTrigger("Reload");
            ReduceRigWeight();

        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightSpeed * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                shouldIncrease_LeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightincreaseRate * Time.deltaTime;
            if (rig.weight >= 1)
            {
                shouldIncrease_RigWeight = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    public void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabingWeapon(true);
    }

    public void SetBusyGrabingWeapon(bool busy)
    {
        isGrabingWeapon = busy;
        anim.SetBool("BusyGrabbingWeapon", isGrabingWeapon);
    }
    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandIKIncrease() => shouldIncrease_LeftHandIKWeight = true;

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.sideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.sideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.backGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimatorLayer(2);
            PlayWeaponGrabAnimation(GrabType.backGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwitchAnimatorLayer(3);
            PlayWeaponGrabAnimation(GrabType.backGrab);
        }
    }

    private void SwitchOn(Transform GunTransform)
    {
        SwitchOffGuns();
        GunTransform.gameObject.SetActive(true);
        currentGun = GunTransform;
        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunsTransform.Length; i++)
        {
            
            gunsTransform[i].gameObject.SetActive(false);
        }
    }
    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHandIKTarget.localPosition = targetTransform.localPosition;
        leftHandIKTarget.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimatorLayer(int index)
    {
        for(int i= 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(index, 1);
    }
}
public enum GrabType { sideGrab , backGrab}
