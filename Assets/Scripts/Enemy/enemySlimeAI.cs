using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemySlimeAI : MonoBehaviour
{
    [Header("---- External Components ----")]
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject meleeCollider;
    [SerializeField] Renderer slimeModel;
    [SerializeField] GameObject eyeballModel;
    [SerializeField] GameObject eyeballPopOutOBJ;
    [SerializeField] Transform eyeballPopOutPos;
    [SerializeField] GameObject[] slimeDroppingPrefabs;
    [SerializeField] GameObject[] slimeFootprintPrefabs;
    [SerializeField] Transform headPos;
    [SerializeField] Material damageFX;
    [SerializeField] SphereCollider playerInRangeTrigger;

    [Header("---- Enemy Stats ----")]
    [SerializeField] int HP;
    [SerializeField] int creditsHeld;
    [SerializeField] bool dropsSlimeFootprints;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int fieldOfView;
    [SerializeField] bool isPlayerDetected;
    [SerializeField] float damageFXLength;
    [SerializeField] int eyeballPopPushForce;
    [SerializeField] int eyeballPopTorqueForce;

    [Header("--- Animation ---")]
    [SerializeField] int animTransSpeed;
    [SerializeField] float attackAnimLength;

    [Header("---- Editor Debug Gizmos ----")]
    [SerializeField] bool drawFieldOfView;
    [SerializeField] bool drawStoppingDistance;
    [SerializeField] bool drawPlayerInRangeRadius;

    // Misc
    bool isAttacking;
    bool playerInRange;
    int HPMAX;
    Vector3 playerDir;
    float angleToPlayer;
    bool isAlive = true;

    private void Start()
    {
        HPMAX = HP;
        /*origSpeed = agent.speed;
        currentSpeed = origSpeed;*/
    }

    private void Update()
    {
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed));
        StartCoroutine(playSteps());

        if (playerInRange && isAlive)
        {
            canSeePlayer();
            inAttackRange();
        }
    }

    IEnumerator playSteps()
    {
        //sfxManager.instance.aud.PlayOneShot(sfxManager.instance.slimeMovement[Random.Range(0, sfxManager.instance.slimeMovement.Length)], sfxManager.instance.slimeMovementVol);
        yield return new WaitForSeconds(5.0f);
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isPlayerDetected = false;
        }
    }
    #endregion

    #region Animation Events
  
    public void animEvent_turnOnMelee()
    {
        meleeCollider.SetActive(true);
        StartCoroutine(timedMeleeOff());    // Just in case 2nd trigger doesn't hit
    }

    public void animEvent_turnOffMelee()
    {
        meleeCollider.SetActive(false);
    }

    public void animEvent_EyeballPopOut()
    {
        GameObject eyeball = Instantiate(eyeballPopOutOBJ, eyeballPopOutPos.transform.position, eyeballPopOutPos.rotation, null);
        eyeballModel.SetActive(false);
        eyeball.GetComponent<Rigidbody>().AddForce((eyeball.transform.forward * eyeballPopPushForce) + new Vector3(0,1,0), ForceMode.Impulse);
        eyeball.GetComponent<Rigidbody>().AddTorque(Vector3.one * eyeballPopTorqueForce, ForceMode.Impulse);
        Destroy(eyeball, 10);
    }

    public void animEvent_slimeDropping()
    {
        // Pick a random dropping prefab and instantiate with a random rotation on the y axis
        int randDropping = Random.Range(0, slimeDroppingPrefabs.Length);
        float randYRot = Random.Range(0, 360);

        Instantiate(slimeDroppingPrefabs[randDropping], transform.position,
                    Quaternion.Euler(slimeDroppingPrefabs[randDropping].transform.rotation.x,
                                    randYRot,
                                    slimeDroppingPrefabs[randDropping].transform.rotation.z));
    }

    public void animEvent_slimeFootprint()
    {
        if(dropsSlimeFootprints)
        {
            // Pick a random dropping prefab and instantiate with a random rotation on the y axis
            int randDropping = Random.Range(0, slimeFootprintPrefabs.Length);
            float randYRot = Random.Range(0, 360);

            Instantiate(slimeFootprintPrefabs[randDropping], transform.position,
                        Quaternion.Euler(slimeFootprintPrefabs[randDropping].transform.rotation.x,
                                        randYRot,
                                        slimeFootprintPrefabs[randDropping].transform.rotation.z));
        }
    }

    public void animEvent_Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator timedMeleeOff()
    {
        yield return new WaitForSeconds(attackAnimLength);
        if (meleeCollider.activeSelf)
        {
            meleeCollider.SetActive(false);
        }
    }

    #endregion

    void facePlayer()
    {
        // Ignore the players direction's y-component
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed); // Smooth rotation
    }

    private void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

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
                    isPlayerDetected = true; 
                    facePlayer();

                    if(!isAttacking && agent.updatePosition)
                        agent.SetDestination(gameManager.instance.player.transform.position);
                }
                else
                {
                    isPlayerDetected = false;
                }
            }
        }
    }

    private void inAttackRange()
    {
        if (agent.remainingDistance < agent.stoppingDistance && (agent.velocity.magnitude < 0.1f) && isPlayerDetected && playerInRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(attack());
            }
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashDamageFX());

        if (HP <= 0)
        {
            //agent.updatePosition = false;
            agent.speed = 0;
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.slimeDeath[Random.Range(0, sfxManager.instance.slimeDeath.Length)], sfxManager.instance.slimeDeathVol);
            anim.SetTrigger("TriggerDeath");
        }
    }


    private void dropCredits()
    {
        // Instantiate the collectableCredits gameObject as well as pass off this enemy's creditsHeld for the amount of credits it has.
        GameObject collectableCredits = Instantiate(gameManager.instance.collectableCreditsPrefab, headPos.position, transform.rotation);
        collectableCredits.GetComponent<collectableCredits>().setCredits(creditsHeld);
    }

    

    IEnumerator attack()
    {
        anim.SetTrigger("TriggerAttack");
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.slimeAttack[Random.Range(0, sfxManager.instance.slimeAttack.Length)], sfxManager.instance.slimeAttackVol);

        // Turn off the connection betwen agent's simulated position and transform position
        agent.updatePosition = false;   
        
        yield return new WaitForSeconds(attackAnimLength);
        isAttacking = false;

        // Move simulated agent's position to transform position, and turn agent back on
        agent.nextPosition = transform.position;
        agent.updatePosition = true;
    }

    IEnumerator flashDamageFX()
    {
        // Store original color of material
        Color origColor = slimeModel.material.color;

        // Quickly switch materials color to the damage material color then back to original
        slimeModel.material.color = damageFX.color;
        yield return new WaitForSeconds(damageFXLength);
        slimeModel.material.color = origColor;
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (drawStoppingDistance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
        }

        if (drawPlayerInRangeRadius)
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

    #endregion
}
