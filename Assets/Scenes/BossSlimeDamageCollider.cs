using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeDamageCollider : MonoBehaviour, IDamage
{
    public BossSlimeInfusedMech brain;
    // Start is called before the first frame update
    public void takeDamage(int dmg)
    {
        brain.takeDamage(dmg);
    }
}
