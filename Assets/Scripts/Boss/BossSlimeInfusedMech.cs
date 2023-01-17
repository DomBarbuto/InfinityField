using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeInfusedMech : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator slimeBodyAnim;
    [SerializeField] GameObject[] slimeDroppingPrefabs;
    [SerializeField] GameObject explosionOBJ;

    [Header("Stats")]
    [SerializeField] int HP;
    [SerializeField] Animator anim;
    [SerializeField] List<enemySpawner> spawners;
    [SerializeField] GameObject body;
    [SerializeField] BossSlimeDamageCollider hitBox;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform leftMuzz;
    [SerializeField] Transform rightMuzz;
    [SerializeField] Transform headPos;

    [Header("Behaviour")]
    public bool startStateMachine;

    int MAXHP;
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
        startStateMachine = true;
        state = 1;
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
        /*if (HP <= ((MAXHP / 3) * 2))
        {
            state = 2;
        }
        else if (HP <= (MAXHP / 3))
        {
            state = 3;
        }
        else if (HP <= 0)
        {

        }*/

        if (HP <= (MAXHP * (2/3)))
        {
            state = 2;
        }
        else if (HP <= (MAXHP * (1 / 3)))
        {
            state = 3;
        }
        else if (HP <= 0)
        {
            state = 4; // which triggers the death in state machine function
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
                    SlowLookAt(1f);
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
                    SlowLookAt(.05f);
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
                        StartCoroutine(timedShoot(30.0f));
                    }

                    //Start following the player by rotating at a slow speed
                    SlowLookAt(.01f);
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

}
