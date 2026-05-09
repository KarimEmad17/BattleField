using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;
    private bool isGrabingWeapon = false;
   

    [SerializeField] private WeaponModel[] weaponsModel;

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
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        weaponsModel = GetComponentsInChildren<WeaponModel>(true);
    }
    private void Start()
    {
      
       
    }

    private void Update()
    {
        CheckWeaponSwitch();
        

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        foreach (WeaponModel model in weaponsModel)
        {
            if (model.weaponType == weaponType)
            {
                weaponModel = model;
                break;
            }
        }
        return weaponModel;
    }

    public void PlayReloadAnimation()
    {
        if(isGrabingWeapon) return;
        anim.SetTrigger("Reload");
        ReduceRigWeight();
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
            SwitchOn();
            SwitchAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.sideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn();
            SwitchAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.sideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn();
            SwitchAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.backGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn();
            SwitchAnimatorLayer(2);
            PlayWeaponGrabAnimation(GrabType.backGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn();
            SwitchAnimatorLayer(3);
            PlayWeaponGrabAnimation(GrabType.backGrab);
        }
    }

    private void SwitchOn()
    {
        SwitchOffWeaponModel();
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }

    private void SwitchOffWeaponModel()
    {
        for (int i = 0; i < weaponsModel.Length; i++)
        {
            
            weaponsModel[i].gameObject.SetActive(false);
        }
    }
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;
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

