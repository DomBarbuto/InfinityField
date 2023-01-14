using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour , IRagdollDamage
{
    [Header("---- External Components ----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material damageFX;
    [SerializeField] SphereCollider playerInRangeTrigger;    // For displaying escape range - OnDrawGizmosSelected
    [SerializeField] ragdollDeath ragdoll;
    [SerializeField] Animator anim;
    [SerializeField] GameObject hitDetection;

    [Header("---- Enemy Stats ----")]
    [SerializeField] int HP;
    [SerializeField] float damageFXLength;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int fieldOfView;
    [SerializeField] bool waitUntilShoot;   // GrenadeLauncher enemy is intended to not wait to shoot
    [SerializeField] int requiredShootAngle;
    [SerializeField] bool isRagdoll;
    [SerializeField] bool isPlayerDetected;
    public int creditsHeld;                 // How many credits the enemy sends over to collectable on death
    [SerializeField] Vector3 pushBack;
    [SerializeField] float pushBackTime;

    [Header("---- Weapon Stats ----")]
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] int animTransSpeed;
    [SerializeField] weaponCreation.WeaponType thisEnemyWeaponType;

    [Header("---- Editor Debug Gizmos ----")]
    [SerializeField] bool drawFieldOfView;
    [SerializeField] bool drawStoppingDistance;
    [SerializeField] bool drawPlayerInRangeRadius;

    /*[Header("---- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] enemyHurt;
    [Range(0, 1)][SerializeField] float enemyHurtVol;
    [SerializeField] AudioClip[] enemyAlert;
    [Range(0, 1)][SerializeField] float enemyAlertVol;*/

    int MAXHP;       // Current max HP
    bool isAttacking;
    bool alertPlayed;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    float signedAngleToPlayer;
    float origStoppingDistance;

    void Start()
    {
        this.GetComponent<enemyDamageHandler>().brain = this;
        // Set animation set. Each animator bool depicts what blend tree to use.
        switch (thisEnemyWeaponType)
        {
            case weaponCreation.WeaponType.Pistol:
                anim.SetBool("HasPistol", true);
                break;

            case weaponCreation.WeaponType.Rifle:
            case weaponCreation.WeaponType.RailGun:
                anim.SetBool("HasRifleOrRailGun", true);
                break;

            case weaponCreation.WeaponType.GrenadeLauncher:
                anim.SetBool("HasGrenadeLauncher", true);
                break;
            default:
                break;
        }
        
        // Store current HP as maximum HP
        MAXHP = HP;

        // Store current NavMesh stopping distance as original
        origStoppingDistance = agent.stoppingDistance;

        gameManager.instance.updateEnemyCount(1);

        if (isRagdoll)
            ragdoll.togglePhysics(false);
    }

    void Update()
    {
        // Animation
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed));

        // If player is in range
        if (playerInRange && HP > 0)
        {
            canSeePlayer();
        }
    }

    //Player Detection-------------------

    void canSeePlayer()
    {
        // Find direction and angle to player
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer =  Vector3.Angle(playerDir, transform.forward);
        signedAngleToPlayer = Vector3.SignedAngle(playerDir, transform.forward, Vector3.right);

        /*Debug.Log("angle: " + angleToPlayer);
        Debug.Log("signed angle: " + signedAngleToPlayer);*/

        Debug.DrawRay(headPos.position, playerDir);

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
                        detectPlayer();
                }
            }

            // Return stopping distance to original if player has already attacked enemy from up close and has gone out of regular stopping distance
            if (agent.remainingDistance > origStoppingDistance && agent.stoppingDistance != origStoppingDistance)
            {
                // Return stopping distance to normal
                agent.stoppingDistance = origStoppingDistance;
            }

            if (isPlayerDetected)
            {
                //Plays alert sound once when player is spotted
                if(!alertPlayed)
                {

                    sfxManager.instance.aud.PlayOneShot(sfxManager.instance.enemyAlert[Random.Range(0, sfxManager.instance.enemyAlert.Length)], sfxManager.instance.enemyAlertVol);
                    alertPlayed = true;
                }

                // Face the player
                facePlayer();

                // Follow the player
                agent.SetDestination(gameManager.instance.player.transform.position);

                // Either wait until player is close enough to shoot or not
                if (!waitUntilShoot)
                {
                    // Only attempt to shoot when player is below or above enemy OR when aiming near player
                    if (signedAngleToPlayer < requiredShootAngle * 10 || angleToPlayer <= requiredShootAngle)
                    {
                        // Only Attack if not already attacking
                        if (!isAttacking)
                        {
                            isAttacking = true;
                            StartCoroutine(attack());
                        }
                    }
                }
                else
                {
                    // Only attempt to shoot when aiming near player OR when player is below or above enemy
                    if (signedAngleToPlayer < requiredShootAngle * 10 || angleToPlayer <= requiredShootAngle)
                    {
                        // Also only when less than stopping distance.
                        if (Vector3.Distance(gameManager.instance.player.transform.position, transform.position) < agent.stoppingDistance)
                        {
                            // Only Attack if not already attacking
                            if (!isAttacking)
                            {
                                isAttacking = true;
                                StartCoroutine(attack());
                            }
                        }
                    }
                }
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

    //Enemy Statitistic Modification-----

    public void takeDamage(int dmg)
    {
        // Makes enemy chase player more closely if shot from up close - until player goes back outside of original stopping distance (this is reset in canSeePlayer)
        // If player is undetected, within stopping distance, and attacks the enemy - reduce stopping distance
        //NOTE: The +1 in condition ensures enemy chases in the case of player being right on the stoppingDistance line.
        if (!isPlayerDetected && Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < agent.stoppingDistance + 1f)
        {
            isPlayerDetected = true;
            agent.stoppingDistance = 3;
        }

        // Reduce enemy health
        HP -= dmg;
        int playCheck = Random.Range(0, 2);
        if(playCheck == 0)
        {
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.enemyHurt[Random.Range(0, sfxManager.instance.enemyHurt.Length)], sfxManager.instance.enemyHurtVol);
        }
       
        // Start moving to where enemy was shot from
        agent.SetDestination(gameManager.instance.player.transform.position);
        
        StartCoroutine(flashDamageFX());

        // Check if this caused death
        if(HP <= 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            dropCredits();
            gameManager.instance.updateEnemyCount(-1);
            agent.enabled = false;
            if (isRagdoll)
            {
                hitDetection.SetActive(false);
                ragdoll.togglePhysics(true);
                StartCoroutine(timedDeath());
            }
            else
                Destroy(gameObject);
        }
    }

    private void detectPlayer()
    {
        isPlayerDetected = true;
    }

    private void undetectPlayer()
    {
        isPlayerDetected = false;
        alertPlayed = false;
    }

    private void dropCredits()
    {
        // Instantiate the collectableCredits gameObject as well as pass off this enemy's creditsHeld for the amount of credits it has.
        GameObject collectableCredits = Instantiate(gameManager.instance.collectableCreditsPrefab, headPos.position, transform.rotation);
        collectableCredits.GetComponent<collectableCredits>().setCredits(creditsHeld);
    }

    //Coroutines-------------------------

    IEnumerator flashDamageFX()
    {
        // TODO: Replace with Blood Splatter Effect

        // Store original color of material
        Color origColor = model.material.color;

        // Quickly switch materials color to the damage material color then back to original
        model.material.color = damageFX.color;
        yield return new WaitForSeconds(damageFXLength);
        model.material.color = origColor;
    }

    IEnumerator attack()
    {
        // Animation - Trigger shoot
        anim.SetTrigger("Shoot");

        // Create bullet, which is added velocity within its start
        Instantiate(projectile, muzzlePoint.position, headPos.transform.rotation);
        yield return new WaitForSeconds(fireRate);

        isAttacking = false;
    }

    IEnumerator timedDeath()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    //Trigger----------------------------

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (isPlayerDetected)
                undetectPlayer();
        }
    }

    //Gizmos----------------------------

    private void OnDrawGizmos()
    {
        if(drawStoppingDistance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
        }

        if(drawPlayerInRangeRadius)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, playerInRangeTrigger.radius);
        }

        if (drawFieldOfView)
            DrawDebugFieldOfView();

    }

    private void DrawDebugFieldOfView()
    {
        // Taking into account the fieldOfView as well as the radius of the playerInRangeTrigger, draw the boundaries of vision
        // Trigonometry at work
        Vector3 leftVisionEdge = transform.TransformDirection(new Vector3(Mathf.Sin((fieldOfView * -1 / 2) * Mathf.Deg2Rad),
                                                                                     0, 
                                                                                     Mathf.Cos((fieldOfView) * Mathf.Deg2Rad))).normalized;

        Vector3 rightVisionEdge = transform.TransformDirection(new Vector3(Mathf.Sin((fieldOfView * 1 / 2) * Mathf.Deg2Rad),
                                                                                      0,
                                                                                      Mathf.Cos((fieldOfView) * Mathf.Deg2Rad))).normalized;

        // Draw Vision Boundary 
        Debug.DrawRay(headPos.position, leftVisionEdge * playerInRangeTrigger.radius, Color.red);
        Debug.DrawRay(headPos.position, rightVisionEdge * playerInRangeTrigger.radius, Color.red);
    }
    public void pushBackInput(Vector3 dir)
    {
        pushBack = dir;
    }


}
