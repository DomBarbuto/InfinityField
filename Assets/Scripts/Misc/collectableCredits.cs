using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class collectableCredits : MonoBehaviour, ICollectable
{
    // This is the default credits that will be given to gameManager on collection (for the case of it being a findable in scene).
    // If this collectable was dropped by enemy or breakable prop, credits will be overwritten via the entity dropping it (setCredits).
    [SerializeField] int credits;
    [SerializeField] int throwSpeed;
    [SerializeField] float UIFXLength;
    [SerializeField] int destroyTimer;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] GameObject modelOBJ;

    private bool hasCollected;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Will get thrown up and forward on instantiation
        rb.velocity = transform.forward + (transform.up * throwSpeed);
        rb.AddTorque(new Vector3(Random.Range(0,4), 0f, 0f));

        // Will be destroyed after set time unless already collected by player
        Destroy(gameObject, destroyTimer);
    }

    // Called via the dropper on instantiation of this object
    public void setCredits(int amount)
    {
        credits = amount;
    }

    // Call collect on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!hasCollected)
                collect();
        }
    }

    public void collect()
    {
        hasCollected = true;

        // UI
        gameManager.instance.addCredits(credits);
        gameManager.instance.updateCreditUI();
        gameManager.instance.startCollectableUIFX(UIFXLength, 0);

        // SFX
        aud.PlayOneShot(pickupSound);

        // Turn off pickup collider and hide the mesh
        // Not destroying mesh right away because sound is still playing
        modelOBJ.SetActive(false);

        Destroy(gameObject, 2);
    }

    
}
