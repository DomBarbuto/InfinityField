using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeInfusedMech : MonoBehaviour, IRoomEntryListener
{
    [SerializeField] int HP;
    [SerializeField] Animator anim;
    [SerializeField] List<enemySpawner> spawners;
    [SerializeField] GameObject body;
    [SerializeField] BossSlimeDamageCollider hitBox;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform leftMuzz;
    [SerializeField] Transform rightMuzz;

    int MAXHP;
    bool startStateMachine;
    public int state = 0;
    Vector3 playerDir;
    bool attackState;
    bool shooting;
    bool vulnerable;

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
        state = 1;
    }

    public void SlowLookAt(float speed)
    {
        playerDir = gameManager.instance.player.transform.position;
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        body.transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speed); // Smooth rotation
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP < ((MAXHP / 3) * 2))
        {
            state = 2;
        }
        if (HP < (MAXHP / 3))
        {
            state = 3;
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
                    SlowLookAt(1);
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
                    SlowLookAt(2);
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
                    SlowLookAt(3);
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

    public IEnumerator timedShoot(float length)
    {
        hitBox.enabled = false;
        anim.SetBool("Shooting", true);
        yield return new WaitForSeconds(length);
        anim.SetBool("Shooting", false);
        shooting = false;
        attackState = false;
    }

    public IEnumerator vulnerableState(float length)
    {
        //play charging effect/animation here
        hitBox.enabled = true;
        yield return new WaitForSeconds(length);
        attackState = true;
    }
}
