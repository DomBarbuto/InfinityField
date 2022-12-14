using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageCollider : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] float cooldown;

    public bool inCooldown;
    private void OnEnable()
    {
        inCooldown = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !inCooldown)
        {
            StartCoroutine(damage());
        }
    }

    IEnumerator damage()
    {
        inCooldown = true;
        gameManager.instance.playerController.takeDamage(dmg);
        yield return new WaitForSeconds(cooldown);
        inCooldown = false;

    }
}
