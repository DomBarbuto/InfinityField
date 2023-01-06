using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHitDetection : MonoBehaviour, IDamage
{
    [SerializeField] enemyDamageHandler brain;
    [SerializeField] enemyDamageHandler.DamageGroup group;
    public bool alive = true;

    private void Start()
    {
        alive = true;
    }
    // Start is called before the first frame update
    public void takeDamage(int dmg)
    {
        if (alive)
            brain.takeDamage(dmg, group);
    }
}
