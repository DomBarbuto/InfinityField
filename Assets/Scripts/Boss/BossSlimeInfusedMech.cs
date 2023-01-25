using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeInfusedMech : MonoBehaviour, IRoomEntryListener
{
    [Header("Components")]
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Animator anim;
    [SerializeField] Animator slimeBodyAnim;
    [SerializeField] GameObject[] slimeDroppingPrefabs;
    [SerializeField] GameObject explosionOBJ;
    [SerializeField] Renderer mechSuitModel;
    [SerializeField] Material damageFX;
    [SerializeField] GameObject forceFieldOBJ;
    [SerializeField] AudioSource aud;

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
    [SerializeField] float introAnimationDuration;
    [SerializeField] enemySpawnSystem spawnSystem;
    [SerializeField] GameObject exit;

    [Header("Behaviour")]
    public bool startStateMachine;

    BossHealthBar bossHPBarScript;
    float MAXHP;
    public int state;
    Vector3 playerDir;
    bool attackState = true;
    bool shooting;

    // Start is called before the first frame update
    void Start()
    {
        bossHPBarScript = healthBarPrefab.GetComponent<BossHealthBar>();
        MAXHP = HP;
        healthBarPrefab.gameObject.SetActive(false);
        forceFieldOBJ.SetActive(false);
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
        anim.SetTrigger("TriggerIntro");
        healthBarPrefab.SetActive(true);

        // Turn on force field
        forceFieldOBJ.SetActive(true);
        playForceFieldUpSound();

        anim.SetTrigger("TriggerIntro");

        // Intro audio
        playIntroSound();
        StartCoroutine(waitForIntroToAdvance());

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
        float prevHealth = HP;
        StartCoroutine(flashDamageFX());

        HP -= dmg;

        // Update health bar
        bossHPBarScript.updateHealthFillAmount(prevHealth, HP, MAXHP);

        // DAMAGE FX

        if (state < 4)
        {
            // If health reaches zero, go to next state
            if (HP <= 0)
            {
                advanceState();
            }
        }
    }

    private void advanceState()
    {
        state++;

        // We also want to go into attack (in-vulnerable) state every time they advance states
        attackState = true;

        // Only do this when going into state 2 and 3 
        if (state == 2 || state == 3)
        {
            // Refill health and health bar
            HP = MAXHP;
            StartCoroutine(bossHPBarScript.refillHealthBar());
        }

        Debug.Log("NEW STATE OF " + state);

        //Trigger UI update
        switch (state)
        {
            case 1:
                // This is already set on start, don't put anytihng here
                break;
            case 2:
                //Make UI show that we are in 2nd state
                bossHPBarScript.turnOffState(1);
                break;
            case 3:
                //Make UI show that we are in 3rd state
                bossHPBarScript.turnOffState(2);
                break;
            case 4:
                // Turn off 3rd state UI and Make Boss health disappear
                bossHPBarScript.turnOffState(3);
                bossHPBarScript.destroyHealthBar();
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
                    SlowLookAt(45.0f);
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
                    SlowLookAt(20.0f);
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
                    SlowLookAt(10.0f);
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
                spawnSystem.setAllContinuous(false);
                slimeBodyAnim.SetTrigger("TriggerDeath");

                break;
        }
    }

    //animation event
    public void leftFire()
    {
        Instantiate(projectile, leftMuzz.position, leftMuzz.rotation);

        playShootSound();
    }

    public void rightFire()
    {
        Instantiate(projectile, rightMuzz.position, rightMuzz.rotation);

        playShootSound();
    }

    public void animEvent_ExplodeEnemy()
    {
        // Explosion object/particle
        GameObject explosion = Instantiate(explosionOBJ, transform.position + new Vector3(0, 1, 0), transform.rotation, null);
        Destroy(explosion, 5);

        // Death explosion
        playDeathSound();

        //Reveal Exit
        exit.SetActive(true);
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
                                    slimeDroppingPrefabs[randDropping].transform.rotation.z), null);
    }

    public IEnumerator timedShoot(float length)
    {
        hitBox.canDamage = false;
        anim.SetBool("Shooting", true);
        slimeBodyAnim.SetBool("IsShooting", true);

        // Turn on forcefield
        forceFieldOBJ.SetActive(true);
        playForceFieldUpSound();

        yield return new WaitForSeconds(length);
        anim.SetBool("Shooting", false);
        shooting = false;
        attackState = false;

        // Turn off force field
        forceFieldOBJ.SetActive(false);
    }

    public IEnumerator vulnerableState(float length)
    {
        playForceFieldDownSound();

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

    IEnumerator waitForIntroToAdvance()
    {
        yield return new WaitForSeconds(introAnimationDuration);
        anim.SetBool("IsIntroDone", true);
        startStateMachine = true;
        advanceState();
    }
    //Audio

    public void playIntroSound()
    {
        aud.PlayOneShot(sfxManager.instance.slimeMechIntro);
    }

    public void playShootSound()
    {
        aud.PlayOneShot(sfxManager.instance.arcgunShootSound[Random.Range(0, sfxManager.instance.arcgunShootSound.Length)]);
    }
    
    public void playForceFieldUpSound()
    {
        aud.PlayOneShot(sfxManager.instance.slimeMechForceFieldUpSound);
    }

    public void playForceFieldDownSound()
    {
        aud.PlayOneShot(sfxManager.instance.slimeMechForceFieldDownSound);
    }

    public void playDeathSound()
    {
        aud.PlayOneShot(sfxManager.instance.slimeMechDeath);
    }
}
