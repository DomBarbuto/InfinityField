using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class weaponCreation : ScriptableObject
{
    public float atkRate;
    public int weaponDamage;
    public float weaponDist;
    public Sprite icon = null;
    public GameObject weaponsModel;

}