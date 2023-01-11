using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{
    [SerializeField] public float HP;
    [SerializeField] public float HPMax;
    [Range(3,8)][SerializeField] public float speed;
    [SerializeField] public float energy;
    [SerializeField] public float energyMax;

    public bool isUsingAbility = false;
    public float currSpeed;
    //[SerializeField] //will contain animation and model
    //[SerializeField] //will contain animation and model
    [SerializeField] public int ability;
}
