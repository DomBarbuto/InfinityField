using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerAnimEventFromChild : MonoBehaviour
{
    [SerializeField] GameObject brain;

    public void animEvent_CallParentSlimeDropping()
    {
        if(brain != null)
        {
            brain.GetComponent<BossSlimeInfusedMech>().animEvent_SlimeDropping();

        }
        else
        {
            Debug.Log("The child animator of " + transform.name + " needs to hook up to its parent");
        }
    }

    public void animEvent_CallParentExplodeEnemy()
    {
        if (brain != null)
        {
            brain.GetComponent<BossSlimeInfusedMech>().animEvent_ExplodeEnemy();

        }
        else
        {
            Debug.Log("The child animator of " + transform.name + " needs to hook up to its parent");
        }
    }
}
