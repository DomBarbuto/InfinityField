using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class collectableEnergy : MonoBehaviour, ICollectable
{
    [SerializeField] float energyPickupRatio;  // Tunable Percentage of maxEnergy that gets added on collect
    [SerializeField] int throwSpeed;
    [SerializeField] float UIFXLength;
    [SerializeField] int destroyTimer;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] GameObject modelOBJ;

    float energyToAdd;
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

        // Will be destroyed after set time unless already collected by player
        Destroy(gameObject, destroyTimer);
    }

    // Call collect on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasCollected)
                collect();
        }
    }

    public void collect()
    {
        energyToAdd = gameManager.instance.playerController.getMAXEnergy() * energyPickupRatio;
        hasCollected = true;

        // UI
        gameManager.instance.playerController.addPlayerEnergy(energyToAdd);
        gameManager.instance.updatePlayerEnergyBar();
        gameManager.instance.startCollectableUIFX(UIFXLength, 2);

        // SFX
        aud.PlayOneShot(pickupSound);

        // Turn off pickup collider and hide the mesh
        // Not destroying mesh right away because sound is still playing
        modelOBJ.SetActive(false);

        Destroy(gameObject, 2); // not destroying 
    }

   

}
