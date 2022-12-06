using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollDeath : MonoBehaviour
{
    [SerializeField] List<Rigidbody> skeleton;

    public void togglePhysics(bool value)
    {
        foreach (Rigidbody bone in skeleton)
        {
            bone.isKinematic = !value;
        }
    }
}
