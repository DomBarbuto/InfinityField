using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyRCCar : MonoBehaviour, IDamage
{
    [Header("---- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject explosionOBJ;

    [Header("---- Settings -----")]
    [SerializeField] int HP;
    [SerializeField] int creditsHeld;
    [SerializeField] Material damageFX;
    [SerializeField] float damageFXLength;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int fieldOfView;

    [Header("---- Detection -----")]
    [SerializeField] bool playerInRange;
    [SerializeField] bool isPlayerDetected;
    Vector3 playerDir;
    float angleToPlayer;

    [Header("---- Audio -----")]
    [SerializeField] AudioSource aud;
    bool drivePlayed;
    [SerializeField] AudioClip[] rcCarDriveClips;
    [Range(0, 1)][SerializeField] float enemyAlertVol;

    private void Start()
    {
        gameManager.instance.updateEnemyCount(1);

        // Pick random drive clip on start
        aud.clip = rcCarDriveClips[Random.Range(0, rcCarDriveClips.Length)];
    }

    private void Update()
    {
        // If player is in range
        if (playerInRange && HP > 0)
        {
            canSeePlayer();
        }
    }

    private void canSeePlayer()
    {
        // Find direction and angle to player
        playerDir = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        // If enemy can see player without obstruction
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            // If what was hit was the player
            if (hit.collider.CompareTag("Player"))
            {
                // If player is within field of view
                if (angleToPlayer <= fieldOfView)
                {
                    // Remember the player is there
                    if (!isPlayerDetected)
                    {
                        isPlayerDetected = true;

                        // Begin playing drive audio clip
                        aud.Play();
                    }
                }
            }

            if(isPlayerDetected)
            {
                // Face the player
                facePlayer();

                // Follow the player
                agent.SetDestination(gameManager.instance.player.transform.position);

            }

        }
    }

    void facePlayer() 
    {
        // Ignore the players direction's y-component
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed); // Smooth rotation
    }

    private void dropCredits()
    {
        // Instantiate the collectableCredits gameObject as well as pass off this enemy's creditsHeld for the amount of credits it has.
        GameObject collectableCredits = Instantiate(gameManager.instance.collectableCreditsPrefab, headPos.position, transform.rotation);
        collectableCredits.GetComponent<collectableCredits>().setCredits(creditsHeld);
    }

    public void takeDamage(int dmg)
    {
        // Reduce enemy health
        HP -= dmg;

        // Start moving to where enemy was shot from
        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(flashDamageFX());

        // Check if this caused death
        if (HP <= 0)
        {
            //Enemy Explosion VFX
            Instantiate(explosionOBJ, transform.position, transform.rotation);

            //Enemy Explosion SFX
            //TODO : 
            gameManager.instance.updateEnemyCount(-1);

            Destroy(gameObject);
        }
    }

    IEnumerator flashDamageFX()
    {
        // Store original color of material
        Color origColor = model.material.color;

        // Quickly switch materials color to the damage material color then back to original
        model.material.color = damageFX.color;
        yield return new WaitForSeconds(damageFXLength);
        model.material.color = origColor;
    }


    public void OnTriggerEnter(Collider other)
    {
        /*if(other.CompareTag("Player") && agent.remainingDistance <= 1)
        {
            //Enemy Explosion VFX
            Instantiate(explosionOBJ, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }*/

        /*if(other.CompareTag("Player") && agent.remainingDistance <= 1)
        {
            //Enemy Explosion VFX
            Instantiate(explosionOBJ, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }*/
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (isPlayerDetected)
                isPlayerDetected = false;
        }
    }
}
