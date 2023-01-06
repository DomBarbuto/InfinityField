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
    bool isPlayerGrenade;


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
            else if(inZone.CompareTag("Player"))
            {
                Vector3 pushVector;
                pushVector.x = (1 * explosionPushBack) / ((transform.position.x + inZone.transform.position.x) / explosionPushBack);
                pushVector.y = (1 * explosionPushBack) / ((transform.position.y + inZone.transform.position.y) / explosionPushBack);
                pushVector.z = (1 * explosionPushBack) / ((transform.position.z + inZone.transform.position.z) / explosionPushBack);

                gameManager.instance.playerController.pushBackInput(pushVector);
            }
            else if(inZone.CompareTag("Enemy"))
            {
                
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
