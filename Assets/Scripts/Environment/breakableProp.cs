using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableProp : MonoBehaviour, IDamage
{
    [Header("---- Prop Components ----")]
    [SerializeField] GameObject unBrokenProp;
    [SerializeField] GameObject brokenProp;
    [SerializeField] GameObject hitFX;
    [SerializeField] Collider parentCollider;

    [Header("---- Prop Stats & Attributes ----")]
    [SerializeField] int HP;
    [SerializeField] int creditsHeld;       //How much loot is inside
    [SerializeField] float hitFXLength;

    [Header("Random Forces Settings")]
    [Range(0, 30)][SerializeField] int randomForceMin;
    [Range(0, 30)][SerializeField] int randomForceMax;


    [Header("Scaling to Destroy Settings")]
    [SerializeField] int waitToBeginScaling;
    [SerializeField] bool hasBegunScalingDown;
    [SerializeField] float scaleDecreaseRate;
    float elapsedTimeSinceScaling;
    [SerializeField] int waitToDestroy;         // This is the time to destroy, STARTING after it has begun scaling down

    // Update is called once per frame
    void Update()
    {
        if(hasBegunScalingDown)
        {
            // Scale down over time, keep track of elapsed time since started scaling
            elapsedTimeSinceScaling += Time.deltaTime;
            brokenProp.transform.localScale -= Time.deltaTime * (brokenProp.transform.localScale / scaleDecreaseRate);

            // Destroy after waitToDestroy time has elapsed
            if (elapsedTimeSinceScaling > waitToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg; //Applies Damage

        StartCoroutine(hitFlash());

        if (HP <= 0) //Checks health
        {
            dropCredits();
            StartCoroutine(waitToBeginScalingDown());   // Turns hasBegunScalingDown to true after timer, begins scaling down
            unBrokenProp.SetActive(false);
            brokenProp.SetActive(true);

            // Adds random forces to all child objects of brokenProp and turns off unBrokenProp collider.
            addRandomForces();
            parentCollider.enabled = false; // Makes everything a bit smoother
        }
    }
    
    // Takes each child of the brokenProp and gives each rigidbody a random impulse force
    private void addRandomForces()
    {
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            Vector3 randomForce = new Vector3(Random.Range(-randomForceMin, randomForceMax), 2f, Random.Range(-randomForceMin, randomForceMax));
            rb.AddForce(randomForce, ForceMode.Impulse);
        }
    }

    private void dropCredits()
    {
        if (unBrokenProp.activeInHierarchy)
        {
            // Instantiate the collectableCredits gameObject as well as pass off this enemy's creditsHeld for the amount of credits it has.
            GameObject collectableCredits = Instantiate(gameManager.instance.collectableCreditsPrefab, transform);
            collectableCredits.GetComponent<collectableCredits>().setCredits(creditsHeld);
        }
    }

    IEnumerator waitToBeginScalingDown()
    {
        yield return new WaitForSeconds(waitToBeginScaling);
        hasBegunScalingDown = true;
    }

    IEnumerator hitFlash()
    {
        hitFX.SetActive(true);
        yield return new WaitForSeconds(hitFXLength);
        hitFX.SetActive(false);
    }

    IEnumerator delayedDestroy()
    {
        yield return new WaitForSeconds(waitToBeginScaling + waitToDestroy);
        Destroy(gameObject);
    }
}
