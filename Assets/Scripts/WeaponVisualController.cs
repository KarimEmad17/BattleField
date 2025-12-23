using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform[] gunsTransform;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;
    private Transform currentGun;
    [Header("Rig")]
    [SerializeField] private float rigincreaseSpeed = 2f;
    private bool rigShouldIncrease = false;
    [Header("Left hand IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private float leftHandIKWeightSpeed = 2f;
    private bool leftHandIKShouldIncrease = false;

    private Rig rig;
    private bool busyGrabingWeapon = false;
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
        if (Input.GetKeyDown(KeyCode.R) && busyGrabingWeapon == false)
        {
            anim.SetTrigger("Reload");
            PauseRig();

        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (leftHandIKShouldIncrease)
        {
            leftHandIK.weight += leftHandIKWeightSpeed * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                leftHandIKShouldIncrease = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (rigShouldIncrease)
        {
            rig.weight += rigincreaseSpeed * Time.deltaTime;
            if (rig.weight >= 1)
            {
                rigShouldIncrease = false;
            }
        }
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    public void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        PauseRig();
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabingWeapon(true);
    }

    public void SetBusyGrabingWeapon(bool busy)
    {
        busyGrabingWeapon = busy;
        anim.SetBool("BusyGrabbingWeapon", busyGrabingWeapon);
    }
    public void EnableRigIncrease() => rigShouldIncrease = true;
    public void EnableLeftHandIKIncrease() => leftHandIKShouldIncrease = true;

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
