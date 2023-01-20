//using UnityEditor.Experimental.Rendering;
//using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu]
public class weaponCreation : ScriptableObject
{
    public enum WeaponType { Pistol, Rifle, GrenadeLauncher, ArcGun, RailGun, Unequipped }  //Keep in this order

    [Header("---- Weapon Transfer Stats ----")]
    public WeaponType weaponType;  
    public int weaponDamage;
    public float shootRate;
    public int shootDistance;
    public Sprite icon;
    public GameObject hitFX;
    public GameObject flashFX;
    [SerializeField] public playerProjectile weaponProjectile;
    public GameObject actualWeaponProjectile;
    public float shootAfterReloadTime;
    public int magazineMax;
    public int magazineCurrent;
    public int maxAmmoPool;
    public int currentAmmoPool;

    [Header("---- Toggleables for weapons ----")]
    [SerializeField] public bool chargeable;
    [SerializeField] public float chargeTime;
    [SerializeField] public float charge;



    private void Awake()
    {
        magazineCurrent = magazineMax;
        currentAmmoPool = maxAmmoPool / 4;
        charge = 0;
    }
}