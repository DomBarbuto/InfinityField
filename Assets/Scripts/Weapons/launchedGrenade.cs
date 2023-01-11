using JetBrains.Annotations;
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
    float time; 
    [SerializeField] float durationOfLerp;

    Rigidbody rb;
    Vector3 origScale;
    Vector3 smallInitial = new Vector3(0.01f, 0.01f, 0.01f);
    bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody>();
        origScale = rb.transform.localScale;
        rb.transform.localScale = smallInitial;
        rb.AddForce((transform.forward + (transform.up * upLaunchForce)) * speed, ForceMode.Impulse);
        StartCoroutine(timedExplosion());
       
    }

    public void Update()
    {
        time += Time.deltaTime;

        float t = time / durationOfLerp;

        t = Mathf.Clamp01(t);

        rb.transform.localScale = Vector3.Lerp(rb.transform.localScale, origScale, t);
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
        if (!exploded)
        {
            exploded = true;
            explode();
        }
    }

    IEnumerator timedExplosion()
    {
        yield return new WaitForSeconds(destroyTime);
        if (!exploded)
            explode();
    }

}
