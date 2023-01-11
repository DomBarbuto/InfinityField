using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimatedMeleeAttack : MonoBehaviour
{
    [SerializeField] GameObject hitBox;
    [SerializeField] int animLength;
    bool isOn = false;

    public void HitBoxToggleOn()
    {
        isOn = true;
        hitBox.SetActive(true);
        StartCoroutine(timedOff());
    }
    public void HitBoxToggleOff()
    {
        isOn = false;
        hitBox.SetActive(false);
        StopCoroutine(timedOff());
    }

    IEnumerator timedOff()
    {
        yield return new WaitForSeconds(animLength);
        if (isOn == false)
        {
            hitBox.SetActive(false);
        }
    }
}
