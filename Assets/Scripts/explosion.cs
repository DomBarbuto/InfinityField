using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] float explosionTimer;
    [SerializeField] int explosionDamage;
    [SerializeField] Vector3 explosionPushBack;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    // Start is called before the first frame update

    private void Start()
    {
        Destroy(gameObject, explosionTimer);
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody RigBdy = other.GetComponent<Rigidbody>();

        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.playerController.pushBackInput(explosionPushBack);
        }

        else if (RigBdy)
        {
            RigBdy.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        if (other.GetComponent<IDamage>() != null)
        {
            other.GetComponent<IDamage>().takeDamage(explosionDamage);
        }

    }
}
