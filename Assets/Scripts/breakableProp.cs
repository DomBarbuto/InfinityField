using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableProp : MonoBehaviour, IDamage
{
    [Header("---- Prop Components ----")]
    [SerializeField] GameObject unBrokenProp;
    [SerializeField] GameObject brokenProp;
    [SerializeField] GameObject hitFX;

    [Header("---- Prop Stats & Attributes ----")]
    [SerializeField] int HP;
    [SerializeField] int creditsHeld; //How much loot is inside

    [SerializeField] float hitFXLength;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg; //Applies Damage

        StartCoroutine(hitFlash());

        if (HP <= 0) //Checks health
        {
            gameManager.instance.addCredits(creditsHeld); //Adds credits
            unBrokenProp.SetActive(false);
            brokenProp.SetActive(true);
        }
    }

    IEnumerator hitFlash()
    {
        hitFX.SetActive(true);
        yield return new WaitForSeconds(hitFXLength);
        hitFX.SetActive(false);
    }
}
