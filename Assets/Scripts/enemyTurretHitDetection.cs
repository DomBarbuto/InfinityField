using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTurretHitDetection : MonoBehaviour
{
    [SerializeField] enemyTurret brain;
    public void takeDamage(int dmg)
    {
        brain.takeDamage(dmg);
    }
}
