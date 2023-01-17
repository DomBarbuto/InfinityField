using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeDamageCollider : MonoBehaviour, IDamage
{
    public BossSlimeInfusedMech brain;

    public bool canDamage;

    public void takeDamage(int dmg)
    {
        if (canDamage)
        brain.takeDamage(dmg);
    }


}
