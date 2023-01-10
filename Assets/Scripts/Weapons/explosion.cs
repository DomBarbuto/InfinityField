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
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] explosionSound;
    [Range(0, 1)][SerializeField] float explosionVol;

    public bool cameFromPlayer;
    [SerializeField] bool canDamagePlayer = true;
    bool canDamageEnemy = true;
    bool damagedEnemy;

    // Start is called before the first frame update

    private void Start()
    {
        if (cameFromPlayer)
        Debug.Log("Explosion created");
        Collider[] objects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider inZone in objects)
        {
            //if (inZone.GetComponent<IDamage>() != null)
            //{
            //    if (cameFromPlayer)
            //        inZone.GetComponent<IDamage>().takeDamage(explosionDamage);
            //    else
            //        inZone.GetComponent<IDamage>().takeDamage(explosionDamage/4);
            //}
        }
            aud.PlayOneShot(explosionSound[Random.Range(0, explosionSound.Length)], explosionVol);
        Destroy(gameObject, explosionTimer);

    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody RigBdy = other.GetComponent<Rigidbody>();

        if (other.gameObject.CompareTag("Player"))
        {
            // Apply different damage if came from enemy
            if (!cameFromPlayer && canDamagePlayer)
            {
                StartCoroutine(givePlayerDamage());

            }

            float forceMultiplier = (explosionRadius - Vector3.Distance(transform.position, other.transform.position)) / 10;
            explosionPushBack = ((other.transform.position - transform.position) * 25) * forceMultiplier;
            gameManager.instance.playerController.pushBackInput(explosionPushBack);
        }

        else if (RigBdy)
        {
            RigBdy.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        if (other.GetComponent<IDamage>() != null && canDamageEnemy)
        {
            canDamageEnemy = false;
            StartCoroutine(giveEnemyDamage(other.GetComponent<IDamage>()));
        }

    }
    IEnumerator giveEnemyDamage(IDamage enemy)
    {
        Debug.Log("Damage Dealt");
        
        if (cameFromPlayer)
            enemy.takeDamage(explosionDamage);
        else
            enemy.takeDamage(explosionDamage / 4);
        yield return new WaitForSeconds(explosionTimer);
        canDamageEnemy = true;
    }
    IEnumerator givePlayerDamage()
    {
        canDamagePlayer = false;
        int playerDamage = explosionDamage / 2;
        gameManager.instance.playerController.takeDamage(playerDamage);
        yield return new WaitForSeconds(explosionTimer);
    }
}
