using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constantRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] Vector3 rotation;
    [SerializeField] Quaternion target;

    private void Update()
    {
        transform.Rotate(rotation, Time.deltaTime * rotateSpeed);
    }
}
