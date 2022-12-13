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
        gameManager.instance.playerController.addPlayerEnergy(energyToAdd);
        gameManager.instance.updatePlayerEnergyBar();

        // TODO: Add SFX

        // TODO: Add VFX

        // GameManager collectable screen FX
        StartCoroutine(playUIFX());    // Destroy in inside
    }

    IEnumerator playUIFX()
    {
        gameManager.instance.collectedCreditsFX.SetActive(true);
        yield return new WaitForSeconds(UIFXLength);
        gameManager.instance.collectedCreditsFX.SetActive(false);
        Destroy(gameObject);
    }

}
