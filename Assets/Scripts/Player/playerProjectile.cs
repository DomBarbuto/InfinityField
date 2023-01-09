using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;
    [SerializeField] bool isPiercing;


    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        RaycastHit hit;

        Vector3 direction = Camera.main.transform.forward;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit))
        {
            direction = (hit.point - gameManager.instance.playerController.currMuzzlePoint.position).normalized;
        }

        Vector3 force = direction * projectileSpeed;

        rb.AddForce(force, ForceMode.Impulse);

    }

}
