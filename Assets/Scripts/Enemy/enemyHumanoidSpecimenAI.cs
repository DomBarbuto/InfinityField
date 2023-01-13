using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyHumanoidSpecimenAI : MonoBehaviour , IRagdollDamage
{
    [Header("---- External Components ----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material damageFX;
    [SerializeField] SphereCollider playerInRangeTrigger;    
    [SerializeField] ragdollDeath ragdoll;
    [SerializeField] Animator anim;
    [SerializeField] GameObject hitDetection;

    [Header("---- Enemy Stats ----")]
    [SerializeField] int HP;
    [SerializeField] float hitAnimLength;
    [SerializeField] float damageFXLength;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int fieldOfView;
    [SerializeField] bool isRagdoll;
    [SerializeField] bool isPlayerDetected;
    public int creditsHeld;                 // How many credits the enemy sends over to collectable on death
    [SerializeField] Vector3 pushBack;
    [SerializeField] float pushBackTime;
    [SerializeField] int animTransSpeed;

    [Header("---- Editor Debug Gizmos ----")]
    [SerializeField] bool drawFieldOfView;
    [SerializeField] bool drawStoppingDistance;
    [SerializeField] bool drawPlayerInRangeRadius;

    bool isAttacking;
    public bool playerInRange;
    int HPMAX;
    Vector3 playerDir;
    float angleToPlayer;
    float signedAngleToPlayer;
    float origStoppingDistance;
    float origSpeed;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<enemyDamageHandler>().brain = this;
        HPMAX = HP;
        origSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed));

        if (playerInRange && isAlive)
        {
            canSeePlayer();
            inAttackRange();
        }
    }

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

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        signedAngleToPlayer = Vector3.SignedAngle(playerDir, transform.forward, Vector3.right);

        Debug.Log("angle: " + angleToPlayer);
        Debug.Log("signed angle: " + signedAngleToPlayer);

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
                    /*facePlayer();
                    agent.SetDestination(gameManager.instance.player.transform.position);*/
                }
                else
                {
                    isPlayerDetected = false;
                }
            }

            if(isPlayerDetected)
            {
                facePlayer();
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
        }
    }

    void inAttackRange()
    {
        //TODO: CHANGE THIS TO REMAINING DISTANCE
        //(Vector3.Distance(gameManager.instance.player.transform.position, transform.position)
        if ((agent.remainingDistance < agent.stoppingDistance) && isPlayerDetected && playerInRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(attack());
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

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            dropCredits();

            isAlive = false;
            ragdoll.togglePhysics(true);
            StartCoroutine(timedDeath());

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
        anim.SetTrigger("AttackSwipe");
        yield return new WaitForSeconds(hitAnimLength);
        agent.speed = origSpeed;
        isAttacking = false;
    }

    IEnumerator timedDeath()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    // Gizmos

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
}
