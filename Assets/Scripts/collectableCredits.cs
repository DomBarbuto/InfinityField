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
    [SerializeField] float damageFXLength;
    [SerializeField] int destroyTimer;
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
            collect();
        }
    }

    public void collect()
    { 
        gameManager.instance.addCredits(credits);

        // TODO: Add SFX

        // TODO: Add VFX

        // GameManager collectable screen FX
        StartCoroutine(playCreditsFX());    // Destroy in inside
    }

    IEnumerator playCreditsFX()
    {
        gameManager.instance.collectedCreditsFX.SetActive(true);
        yield return new WaitForSeconds(damageFXLength);
        gameManager.instance.collectedCreditsFX.SetActive(false);
        Destroy(gameObject);
    }
}
