using UnityEngine;
public enum GrabType { sideGrab, backGrab }
public enum  HoldType {commonHold = 1 , lowHold , highHold }
public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public GrabType grabType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;
}
