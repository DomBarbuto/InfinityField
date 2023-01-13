using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimatedMeleeAttack : MonoBehaviour
{
    [SerializeField] public GameObject leftHitBox;
    [SerializeField] public GameObject rightHitBox;
    [SerializeField] public GameObject currentHitBox;

    [SerializeField] int animLength;
    bool isOn = false;


    public void HitBoxToggleOn()
    {
        isOn = true;
        currentHitBox.SetActive(true);
        StartCoroutine(timedOff());
    }
    public void HitBoxToggleOff()
    {
        isOn = false;
        currentHitBox.SetActive(false);
        StopCoroutine(timedOff());
    }

    IEnumerator timedOff()
    {
        yield return new WaitForSeconds(animLength);
        if (isOn == false)
        {
            currentHitBox.SetActive(false);
        }
    }


}
