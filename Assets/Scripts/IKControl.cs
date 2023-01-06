using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    private Animator animator;
    public bool ikActive = false;
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // A callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            // If the IK is active, assign IK weights
            if (ikActive)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot.position);
                //animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot.position);*/
            }

            // Else if the IK is not active, set the position and rotation back to the originals
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            }
        }
    }
}