using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launchedGrenade : MonoBehaviour
{
    [Header("---- Bullet Settings ----")]
    [SerializeField] int speed;
    [SerializeField] int damage;
    [SerializeField] float upLaunchForce;
    [SerializeField] int destroyTime;
    [SerializeField] GameObject explosionOBJ;
    [SerializeField] AudioSource aud;
    [SerializeField] bool cameFromPlayer;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce((transform.forward + (transform.up * upLaunchForce)) * speed, ForceMode.Impulse);
        StartCoroutine(timedExplosion());
    }

    void explode()
    {
        GameObject newExplosion = Instantiate(explosionOBJ, transform.position, transform.rotation);
        newExplosion.GetComponent<explosion>().cameFromPlayer = cameFromPlayer;
        newExplosion.transform.SetParent(null);
        Destroy(gameObject);
    }

    void OnTriggerEnter()
    {
        explode();
    }

    IEnumerator timedExplosion()
    {
        yield return new WaitForSeconds(destroyTime);
        explode();
    }

}
