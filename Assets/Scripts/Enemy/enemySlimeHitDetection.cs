using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySlimeHitDetection : MonoBehaviour, IDamage
{
    [SerializeField] enemySlimeAI brain;

    public void takeDamage(int dmg)
    {
        brain.takeDamage(dmg);
    }
}
