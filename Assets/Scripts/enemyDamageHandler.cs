using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamageHandler : MonoBehaviour
{
    [SerializeField] enemyAI brain;

    [Tooltip("Multiplies the damage recieved by colliders marked head")][Range(0, 5)] [SerializeField] int headMod;
    [Tooltip("Divides the damage recieved by colliders marked limb")] [Range(0, 5)] [SerializeField] int limbMod;
    [Tooltip("Multiplies the damage recieved by colliders marked torso")] [Range(0, 5)] [SerializeField] int torsoMod;

    public enum DamageGroup
    {
        Limb,
        Head,
        Torso
    }

    public void takeDamage(int dmg, DamageGroup group)
    {
        switch (group)
        {
            case (DamageGroup.Head):
                brain.takeDamage(headMod * dmg);
                break;
            case (DamageGroup.Limb):
                brain.takeDamage(dmg / headMod);
                break;
            case (DamageGroup.Torso):
                brain.takeDamage(limbMod * dmg);
                break;

        }
    }
}
