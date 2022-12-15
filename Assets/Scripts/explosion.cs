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

    // Start is called before the first frame update

    private void Start()
    {
        aud.PlayOneShot(explosionSound[Random.Range(0, explosionSound.Length)], explosionVol);
        Destroy(gameObject, explosionTimer);
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody RigBdy = other.GetComponent<Rigidbody>();

        if (other.gameObject.CompareTag("Player"))
        {
            float forceMultiplier = (explosionRadius - Vector3.Distance(transform.position, other.transform.position)) / 10;
            explosionPushBack = ((other.transform.position - transform.position) * 25) * forceMultiplier;
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
