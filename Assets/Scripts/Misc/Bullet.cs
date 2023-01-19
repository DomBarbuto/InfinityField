using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("---- Bullet Settings ----")]
    [SerializeField] int speed;
    [SerializeField] int damage;
    [SerializeField] int destroyTime;
    [SerializeField] GameObject bulletHitFX;

    private bool hasCollided;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Vector3 playerYDir = (gameManager.instance.player.transform.position - transform.position).normalized;
        rb.velocity = (transform.forward + new Vector3(0f, playerYDir.y, 0f)) * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!hasCollided)
        {
            hasCollided = true;
            Debug.Log("Bullet hit " + other.name);
            if(other.CompareTag("Player"))
            {
                gameManager.instance.playerController.takeDamage(damage);
                GameObject hitFX = Instantiate(bulletHitFX, transform.position, bulletHitFX.transform.rotation, null);
                Destroy(hitFX, 1);
            }
        }

        Destroy(gameObject);
    }
}
