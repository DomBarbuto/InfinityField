using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimatedMeleeAttack : MonoBehaviour
{
    [SerializeField] public GameObject leftHitBox;
    [SerializeField] public GameObject rightHitBox;
    [SerializeField] public GameObject currentHitBox;
    [SerializeField] bool isColliderEnabled = false;

    public void HitBoxToggleOn()
    {
        currentHitBox.SetActive(true);
        isColliderEnabled = true;
    }
    public void HitBoxToggleOff()
    {
        currentHitBox.SetActive(false);
        isColliderEnabled = false;
    }



}
