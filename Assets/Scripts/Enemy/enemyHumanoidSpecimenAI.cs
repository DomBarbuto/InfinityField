using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyHumanoidSpecimenAI : MonoBehaviour , IRagdollDamage
{
    [Header("---- External Components ----")]
    [SerializeField] enemyAnimatedMeleeAttack meleeComponent;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Material damageFX;
    [SerializeField] SphereCollider playerInRangeTrigger;    
    [SerializeField] ragdollDeath ragdoll;
    [SerializeField] Animator anim;

    [Header("---- Enemy Stats ----")]
    [SerializeField] int HP;
    [SerializeField] float hitAnimLength;
    [SerializeField] float damageFXLength;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int fieldOfView;
    [SerializeField] bool isPlayerDetected;
    public int creditsHeld;                 // How many credits the enemy sends over to collectable on death
    [SerializeField] int animTransSpeed;

    [Header("---- Editor Debug Gizmos ----")]
    [SerializeField] bool drawFieldOfView;
    [SerializeField] bool drawStoppingDistance;
    [SerializeField] bool drawPlayerInRangeRadius;

    bool isAttacking;
    public bool playerInRange;
    int HPMAX;
    Vector3 playerDir;
    bool alertPlayed;
    float angleToPlayer;
    float origSpeed;
    bool isAlive = true;
    LayerMask rayMask;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<enemyDamageHandler>().brain = this;
        rayMask = ~LayerMask.GetMask("EnemyBody");
        
        HPMAX = HP;
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

        Debug.DrawRay(headPos.position, playerDir);

        // If enemy can see player without obstruction
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit, Mathf.Infinity, rayMask))
        {
            // If what was hit was the player
            if (hit.collider.CompareTag("Player"))
            {
                // If player is within field of view
                if (angleToPlayer <= fieldOfView)
                {
                    if (!alertPlayed)
                    {
                        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.humanoidSpecimenAlert[Random.Range(0, sfxManager.instance.humanoidSpecimenAlert.Length)], sfxManager.instance.humanoidSpecimenAlertVolMulti);
                        alertPlayed = true;
                    }
                    isPlayerDetected = true;
                    facePlayer();
                    agent.SetDestination(gameManager.instance.player.transform.position);
                }
                else
                {
                    isPlayerDetected = false;
                    alertPlayed = false;
                }
            }
        }
    }

    void inAttackRange()
    {
         if(agent.remainingDistance < agent.stoppingDistance  && (agent.velocity.magnitude < 0.1f) && isPlayerDetected && playerInRange)
         {
            if (!isAttacking)
            {
                isAttacking = true;

                // Random bool (0 or 1) is brought into the attack coroutine, to decide which animation to play and
                // which arm damage collider to use.
                StartCoroutine(attack(decideWhichArm()));
            }
        }

    }

    bool decideWhichArm()
    {
        int randomNum = Random.Range(0, 2); // Return 0 or a 1. Min inclusive, max exclusive
        if (randomNum == 0)
            return true;
        else
            return false;
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
        if(isAlive)
        {
            HP -= dmg;
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.humanoidSpecimenHurt[Random.Range(0, sfxManager.instance.humanoidSpecimenHurt.Length)], sfxManager.instance.humanoidSpecimenHurtVolMulti);
            // Animation Hit Reaction
            anim.SetTrigger("HitReaction");

            // Damage FX
            StartCoroutine(flashDamageFX());

        }

        if (HP <= 0 && isAlive)
        {
            agent.enabled = false;
            dropCredits();
            isAlive = false;
            ragdoll.togglePhysics(true);
            StartCoroutine(timedDeath());

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

    private void dropCredits()
    {
        // Instantiate the collectableCredits gameObject as well as pass off this enemy's creditsHeld for the amount of credits it has.
        GameObject collectableCredits = Instantiate(gameManager.instance.collectableCreditsPrefab, headPos.position, transform.rotation);
        collectableCredits.GetComponent<collectableCredits>().setCredits(creditsHeld);
    }

    IEnumerator attack(bool left)
    {
        if (left)
        {
            meleeComponent.currentHitBox = meleeComponent.leftHitBox;
            anim.SetTrigger("AttackSwipeLeft");
        }
        else
        {
            meleeComponent.currentHitBox = meleeComponent.rightHitBox;
            anim.SetTrigger("AttackSwipeRight");
            
        }

        AudioSource.PlayClipAtPoint(sfxManager.instance.humanoidSpecimenAttack[Random.Range(0, sfxManager.instance.humanoidSpecimenAttack.Length)], transform.position, sfxManager.instance.aud.volume * sfxManager.instance.humanoidSpecimenAttackVolMulti);
        yield return new WaitForSeconds(hitAnimLength);
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
