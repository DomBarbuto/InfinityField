using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---- External Components ----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material damageFX;
    [SerializeField] SphereCollider playerInRangeTrigger;    // For displaying escape range - OnDrawGizmosSelected


    [Header("---- Enemy Stats ----")]
    [SerializeField] int HP;
    [SerializeField] float damageFXLength;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int fieldOfView;       
    [SerializeField] bool remembersPlayer;

    [Header("---- Weapon Stats ----")]
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate;
    [SerializeField] Transform muzzlePoint;

    [Header("---- Editor Debug Gizmos ----")]
    [SerializeField] bool drawFieldOfView;
    [SerializeField] bool drawStoppingDistance;
    [SerializeField] bool drawPlayerInRangeRadius;

    int MAXHP;       // Current max HP
    int creditsHeld; // How many credits the enemy drops
    bool isAttacking;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    float origStoppingDistance;

    void Start()
    {
        // Store current HP as maximum HP
        MAXHP = HP;

        // Store current NavMesh stopping distance as original
        origStoppingDistance = agent.stoppingDistance;

        gameManager.instance.updateEnemyCount(1);
    }

    void Update()
    {
        // If player is in range
        if (playerInRange)
        {
            canSeePlayer();
        }

        //DrawDebugFieldOfView();
    }

    //Player Detection-------------------

    void canSeePlayer() 
    {
        // Find direction and angle to player
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.Log(angleToPlayer);

        Debug.DrawRay(headPos.position, playerDir);

        // If enemy can see player without obstruction
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        { 
            // If what was hit was the player
            if(hit.collider.CompareTag("Player"))
            {
                // If player is within field of view
                if(angleToPlayer <= fieldOfView)
                {
                    // Remember the player is there
                    if (!remembersPlayer)
                        remembersPlayer = true;
                }
            }
           
            if (remembersPlayer)
            {
                // Important for making AI feel smarter
                // Make the enemy still follow the player while within normal stopping distance
                // This is needed because navMeshAgent cannot rotate when remaining distance < stopping distance
                if (agent.remainingDistance < origStoppingDistance)
                {
                    agent.stoppingDistance = 3; 
                }
                else
                {
                    // Return stopping distance to normal
                    agent.stoppingDistance = origStoppingDistance;
                }

                // Follow the player
                agent.SetDestination(gameManager.instance.player.transform.position);

                // Attack if not already attacking
                if (!isAttacking)
                    StartCoroutine(Attack());
                
                // Face the player
                facePlayer(); 
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
        // Reduce enemy health
        HP -= dmg;

        // If player is undetected, within stopping distance, and attacks the enemy - reduce stopping distance to have him attack back
        if(Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < agent.stoppingDistance)
            agent.stoppingDistance = 3;

        // Start moving to where enemy was shot from
        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(flashDamageFX());

        // Check if this caused death
        if(HP <= 0)
        {
            gameManager.instance.addCredits(creditsHeld);
            gameManager.instance.updateEnemyCount(-1);
            Destroy(gameObject);
        }
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

    IEnumerator Attack()
    {
        isAttacking = true;

        // Create bullet, which is added velocity within its start
        Instantiate(projectile, muzzlePoint.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);

        isAttacking = false;
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
            remembersPlayer = false;

            // Display users last known position for a short duration
            StartCoroutine(gameManager.instance.DisplayPlayerLastKnownPosition());
        }
    }

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
}
