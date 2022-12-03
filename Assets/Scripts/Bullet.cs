using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class bullet : MonoBehaviour
{
    [Header("---- Bullet Settings ----")]
    [SerializeField] int speed;
    [SerializeField] int damage;
    [SerializeField] int destroyTime;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit " + other.name);
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerController.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
