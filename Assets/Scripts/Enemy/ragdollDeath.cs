using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollDeath : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] List<Rigidbody> skeleton;
    //[SerializeField] bool isRagdolling;

    public void togglePhysics(bool value)
    {
        // If true is passed in, which only happens on enemyDeath
        // Dont want to call turnOffAnimator when its false, which is on enemy start
        if (value)
            turnOffAnimator();

        foreach (Rigidbody bone in skeleton)
        {
            bone.isKinematic = !value;
            if (bone.GetComponent<enemyHitDetection>() != null && value)
                bone.GetComponent<enemyHitDetection>().alive = !value;
        }
    }

    private void turnOffAnimator()
    {
        anim.enabled = false;
    }
}
