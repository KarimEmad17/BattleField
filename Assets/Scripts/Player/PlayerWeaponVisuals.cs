using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;
    private bool isGrabingWeapon = false;
   

    [SerializeField] private WeaponModel[] weaponsModel;
    [SerializeField] private BackUpWeaponModel[] backUpWeaponModels;    

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
        backUpWeaponModels = GetComponentsInChildren<BackUpWeaponModel>(true);
    }
    private void Start()
    {
      
       
    }

    private void Update()
    {
        //CheckWeaponSwitch();
        

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

   

    public void PlayReloadAnimation()
    {
        if(isGrabingWeapon) return;

        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;
        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }
    
    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipType;
        float equipmentSpeed = player.weapon.CurrentWeapon().equipSpeed;
        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetFloat("EquipType", (float)equipType);
        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipSpeed", equipmentSpeed);

        SetBusyGrabingWeapon(true);
    }

    public void SetBusyGrabingWeapon(bool busy)
    {
        isGrabingWeapon = busy;
        anim.SetBool("BusyEquipingWeapon", isGrabingWeapon);
    }
    

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            SwitchAnimatorLayer(1);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            SwitchAnimatorLayer(1);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
            SwitchAnimatorLayer(1);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
            SwitchAnimatorLayer(2);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
           
            SwitchAnimatorLayer(3);
            
        }
    }

    public void SwitchOnCurrentWeaponModel()
    {
       // SwitchOffWeaponModel();

        SwitchOffBackUpWeaponModel();
        if(!player.weapon.HasOnlyOneWeapon())
            SwitchOnBackUpWeaponModel();

        HoldType holdType = CurrentWeaponModel().holdType;
        SwitchAnimatorLayer((int)holdType);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }

    public void SwitchOffWeaponModel()
    {
        for (int i = 0; i < weaponsModel.Length; i++)
        {
            
            weaponsModel[i].gameObject.SetActive(false);
        }
    }

    public void SwitchOffBackUpWeaponModel()
    {
        for (int i = 0; i < backUpWeaponModels.Length; i++)
        {
            backUpWeaponModels[i].gameObject.SetActive(false);
        }
    }

    public void SwitchOnBackUpWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackUpWeapon().weaponType;
        foreach (BackUpWeaponModel model in backUpWeaponModels)
        {
            if (model.weaponType == weaponType)
            {
                model.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void SwitchAnimatorLayer(int index)
    {
        for(int i= 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(index, 1);
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

    #region Animation Rigging Methods
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;
        leftHandIKTarget.localPosition = targetTransform.localPosition;
        leftHandIKTarget.localRotation = targetTransform.localRotation;
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
    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandIKIncrease() => shouldIncrease_LeftHandIKWeight = true;
    #endregion
}

