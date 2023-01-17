using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeInfusedMech : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] BossHealthBar healthBarPrefab;
    [SerializeField] Animator anim;
    [SerializeField] Animator slimeBodyAnim;
    [SerializeField] GameObject[] slimeDroppingPrefabs;
    [SerializeField] GameObject explosionOBJ;
    [SerializeField] Renderer mechSuitModel;
    [SerializeField] Material damageFX;

    [Header("Stats")]
    [SerializeField] float HP;
    [SerializeField] List<enemySpawner> spawners;
    [SerializeField] GameObject body;
    [SerializeField] BossSlimeDamageCollider hitBox;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform leftMuzz;
    [SerializeField] Transform rightMuzz;
    [SerializeField] Transform headPos;
    [SerializeField] float damageFXLength;

    [Header("Behaviour")]
    public bool startStateMachine;

    float MAXHP;
    public int state;
    Vector3 playerDir;
    bool attackState = true;
    bool shooting;

    // Start is called before the first frame update
    void Start()
    {
        MAXHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (startStateMachine)
        {
            stateMachine();
        }
    }

    //On Room entry... 
    public void notify()
    {
        //Make health bar appear


        startStateMachine = true;
        advanceState();
        anim.SetTrigger("TriggerIntro");

    }

    public void SlowLookAt(float speed)
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        body.transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speed); // Smooth rotation
    }


    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (state < 4)
        {
            // If health reaches zero, go to next state
            if (HP <= 0)
            {
                advanceState();
            }
        }


        //TODO: Update health bar
    }

    private void advanceState()
    {
        state++;
        HP = MAXHP;

        Debug.Log("NEW STATE OF " + state);

        //Trigger UI update
        switch (state)
        {
            //Refill health bar

            case 1:
                //Make UI show that we are in 1st state
                break;
            case 2:
                //Make UI show that we are in 2nd state
                break;
            case 3:
                //Make UI show that we are in 3rd state
                break;
            case 4:
                //Make Boss health disappear
                break;

        }
    }

    public void stateMachine()
    {
        switch (state)
        {
            //1st wave happens when under 1/3 health
            case 1:
                if (attackState)
                {
                    if (!shooting)
                    {
                        //Start shooting
                        shooting = true;
                        StartCoroutine(timedShoot(10.0f));
                    }

                    //Start following the player by rotating at a slow speed
                    SlowLookAt(30.0f);
                }
                else
                {
                    //Starts the vulnerability state, boss can be attacked.
                    StartCoroutine(vulnerableState(5.0f));
                }
                break;
            case 2:
                if (attackState)
                {
                    if (!shooting)
                    {
                        //Start shooting
                        shooting = true;
                        StartCoroutine(timedShoot(15.0f));
                    }

                    //Start following the player by rotating at a slow speed
                    SlowLookAt(15.0f);
                }
                else
                {
                    //Starts the vulnerability state, boss can be attacked.
                    StartCoroutine(vulnerableState(5.0f));
                }
                break;
            case 3:
                if (attackState)
                {
                    if (!shooting)
                    {
                        //Start shooting
                        shooting = true;
                        StartCoroutine(timedShoot(20.0f));
                    }

                    //Start following the player by rotating at a slow speed
                    SlowLookAt(1.0f);
                }
                else
                {
                    //Starts the vulnerability state, boss can be attacked.
                    StartCoroutine(vulnerableState(5.0f));
                }
                break;
            case 4:
                //Win State, boss depleted. Insert any necessary code here when the time comes.
                anim.SetBool("Shooting", false);
                slimeBodyAnim.SetBool("IsShooting", false);
                shooting = false;
                attackState = false;
                StopAllCoroutines();
                slimeBodyAnim.SetTrigger("TriggerDeath");

                break;
        }
    }

    //animation event
    public void leftFire()
    {
        Instantiate(projectile, leftMuzz.position, leftMuzz.rotation);
        //add sfx
    }

    public void rightFire()
    {
        Instantiate(projectile, rightMuzz.position, rightMuzz.rotation);
        //add sfx
    }

    public void animEvent_ExplodeEnemy()
    {
        // Explosion object/particle
        GameObject explosion = Instantiate(explosionOBJ, transform.position + new Vector3(0, 1, 0), transform.rotation, null);
        Destroy(explosion, 5);

        // Destroy boss
        Destroy(gameObject);
    }

    public void animEvent_SlimeDropping()
    {
        int randDropping = Random.Range(0, slimeDroppingPrefabs.Length);
        float randYRot = Random.Range(0, 360);
        Instantiate(slimeDroppingPrefabs[randDropping], transform.position, 
                    Quaternion.Euler(slimeDroppingPrefabs[randDropping].transform.rotation.x,
                                    randYRot,
                                    slimeDroppingPrefabs[randDropping].transform.rotation.z));
    }

    public IEnumerator timedShoot(float length)
    {
        hitBox.canDamage = false;
        anim.SetBool("Shooting", true);
        slimeBodyAnim.SetBool("IsShooting", true);

        yield return new WaitForSeconds(length);
        anim.SetBool("Shooting", false);
        shooting = false;
        attackState = false;
    }

    public IEnumerator vulnerableState(float length)
    {
        // Update animation
        slimeBodyAnim.SetBool("IsShooting", false);

        hitBox.canDamage = true;
        yield return new WaitForSeconds(length);
        attackState = true;
    }

    IEnumerator flashDamageFX()
    {
        // Store original color of material
        Color origColor = mechSuitModel.material.color;

        // Quickly switch materials color to the damage material color then back to original
        mechSuitModel.material.color = damageFX.color;
        yield return new WaitForSeconds(damageFXLength);
        mechSuitModel.material.color = origColor;
    }

}
