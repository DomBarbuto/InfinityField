using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableProp : MonoBehaviour, IDamage
{
    [Header("---- Prop Components ----")]
    [SerializeField] GameObject propBase;
    [SerializeField] GameObject propLid; // Has a rigidbody
    //[SerializeField] GameObject hitFX;
    [SerializeField] AudioSource aud;

    [Header("---- Prop Stats & Attributes ----")]
    [SerializeField] int HP;
    [SerializeField] int randCreditsMin;
    [SerializeField] int randCreditsMax;
    int creditsHeld;       //How much loot is inside
    [SerializeField] int launchForce;

    bool hasBroken;

    private void Start()
    {
        // Random number of credits passed off to the collectable
        creditsHeld = Random.Range(randCreditsMin, randCreditsMax);
        aud.enabled = false;
    }

    public void takeDamage(int dmg)
    {
        if(!hasBroken)
        {
            HP -= dmg; //Applies Damage

            if (HP <= 0) //Checks health
            {
                hasBroken = true;

                // Play breaking sound
                /*aud.PlayOneShot(sfxManager.instance.boxBreak[Random.Range(0, sfxManager.instance.boxBreak.Length)]);*/
                aud.enabled = true;

                // launch lid
                propLid.GetComponent<Rigidbody>().AddForce((transform.up + transform.forward) * launchForce);

                dropCredits();

            }

        }
        
    }

    private void dropCredits()
    {
        // Instantiate the collectableCredits gameObject as well as pass off this enemy's creditsHeld for the amount of credits it has.
        GameObject collectableCredits = Instantiate(gameManager.instance.collectableCreditsPrefab, transform);
        collectableCredits.GetComponent<collectableCredits>().setCredits(creditsHeld);
    }

   /* IEnumerator hitFlash()
    {
        hitFX.SetActive(true);
        yield return new WaitForSeconds(hitFXLength);
        hitFX.SetActive(false);
    }*/

}
