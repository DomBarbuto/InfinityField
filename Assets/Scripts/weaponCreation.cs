using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu]
public class weaponCreation : ScriptableObject
{
    public enum WeaponType { pistol, rifle, grenadeLauncher }

    [Header("---- Weapon Transfer Stats ----")]
    public WeaponType weaponMuzzleType;   // Used for selecting which muzzle point on the player to use
    public int weaponDamage;
    public float shootRate;
    public int shootDistance;
    public Sprite icon;
    public GameObject weaponsModel;
    public Vector3 weaponPositionOffset;

    [Header("---- Stats only for throwables ----")]

    [SerializeField] public bool isThrowable;
    [SerializeField] public GameObject thrownObject;
    [SerializeField] public float launchForce;
    [SerializeField] public float upLaunchForce;
     
}