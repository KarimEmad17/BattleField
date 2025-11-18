using Unity.VisualScripting;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunsTransform;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;
    private Transform currentGun;
    [Header("Left hand IK")]
    [SerializeField] private Transform leftHandIKTarget;
    private void Start()
    {
        SwitchOn(pistol);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
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
}
