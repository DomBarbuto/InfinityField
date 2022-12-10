using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] float explosionTimer;
    [SerializeField] int explostionDamage;
    [SerializeField] float explosionPushBack;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionRadius;
    float timeOfCreation;
    float timeofExplosion;


    private void Awake()
    {
        Debug.Log("GrenadeCreated");
        StartCoroutine(explodeTime(explosionTimer));
    }


   /* private void OnCollisionEnter(Collision collision)
    {
        explodeTime(explosionTimer);
        
        //Instantiate(explosion, transform.position, transform.rotation);
        //Destroy(explosion, 5);
        
    }*/



        
  

    private void explosionPush()
    {
        Debug.Log("Explode");
        Collider[] objects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider inZone in objects)
        {
            Rigidbody RigBdy = inZone.GetComponent<Rigidbody>();
            if(RigBdy!= null )
            {
                RigBdy.AddExplosionForce(explosionPushBack, transform.position, explosionRadius);
            }
        }
    }

    IEnumerator explodeTime(float time)
    {
        yield return new WaitForSeconds(time);

        explosionPush();
        Destroy(gameObject);

    }
}
