using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu]
public class weaponCreation : ScriptableObject
{
    public float atkRate;
    public int weaponDamage;
    public float weaponDist;
    public Sprite icon;
    public GameObject weaponsModel;

    [Header("---- Stats only for throwables ----")]

    [SerializeField] public bool isThrowable;
    [SerializeField] public GameObject thrownObject;
    [SerializeField] public float launchForce;
    [SerializeField] public float upLaunchForce;
     
}