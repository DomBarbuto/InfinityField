using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScuttlingSpecimenHitDetection : MonoBehaviour, IDamage
{
    [SerializeField] enemyScuttlingSpecimenAI brain;

    public void takeDamage(int dmg)
    {
        brain.takeDamage(dmg);
    }
}
