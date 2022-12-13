using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHitDetection : MonoBehaviour, IDamage
{
    [SerializeField] enemyAI brain;
    // Start is called before the first frame update
    public void takeDamage(int dmg)
    {
        brain.takeDamage(dmg);
    }
}
