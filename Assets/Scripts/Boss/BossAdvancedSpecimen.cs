using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAdvancedSpecimen : MonoBehaviour, IRoomEntryListener
{
    [Header("---------- Components/Prefabs ----------")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject[] scuttlingEnemies;
    [SerializeField] GameObject[] humanoidEnemies;
    [SerializeField] GameObject spawnParticlePrefab;    // Instantiated via anim event on enemy spawn
    [SerializeField] GameObject explosionOBJ;           // Instantiated cia anim event at end of boss death

    [Header("---------- Stats ----------")]
    [SerializeField] float HP;
    [SerializeField] Transform[] spawnerPositions;

    /*
     * This boss regarding the spawning of enemies is unique because the spawning of the enemies
     * continues on indefinitely at a CURRENT interval rate (boss fight state-dependent) until the player hits
     * all buttons to progress to the next state.
     * 
     * So for each state, when spawning - a random animation trigger (spawn left, right, or back) is triggered, in which
     * the spawning of the random enemy is done through that animations corresponding animation event. A random enemy type
     * is chosen to spawn, and digging deeper - a random variation of that enemy type is chosen.
     * 
     * NOTE: A random current intial spawn interval is chosen at start.
     * As each boss state advances, the current spawn interval is divided by the chosen nextStateSpawnIntervalDivisor
     */
    [Header("---------- Behaviour ----------")]
    [SerializeField] bool startsFromRoomEntry;
    [SerializeField] bool startStateMachine;
    [SerializeField] int state;
    [SerializeField] float minInitialSpawnInterval;
    [SerializeField] float maxInitialSpawnInterval;
    [SerializeField] float currentSpawnInterval;
    [SerializeField] int nextStateSpawnIntervalDivisor = 2; // If 1, current spawn interval will remain the same when going into next state

    [Header("---------- Platforms and Buttons ----------")]
    [SerializeField] PlatformSection[] platformSections;
    [SerializeField] GameObject[] buttons;            
    [SerializeField] int buttonsHitThisStage;
    [SerializeField] public int numberOfButtons;    // Incremented from each buttons start

    //Misc
    float MAXHP;
    bool isSpawning;

    private void Start()
    {
        MAXHP = HP;
        buttonsHitThisStage = 0;
        currentSpawnInterval = getRandomIntialSpawnInterval();
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    private void Update()
    {
        // With this boss, state is incremented once all buttons have been pushed during current state
        if(startStateMachine)
        {
            stateMachine();
        }
    }

    // [DESIGNERS CHOICE] On Room entry... if boss is set to activate on room entry / other option is to trigger the boss on trigger enter
    public void notify()
    {
        if (!startsFromRoomEntry)
            return;

        Debug.Log("In notify");
        startStateMachine = true;
        state = 1;
        AudioSource.PlayClipAtPoint(sfxManager.instance.advSpecIntro, transform.position, sfxManager.instance.advSpecIntroVolume);
        anim.SetTrigger("TriggerIntro");
    }

    // [DESIGNERS CHOICE] Only for the case of the boss starting the fight upon player entering a trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Start from room entry is the other option, so do nothing if this boss fight begins upon room entry
        if (startsFromRoomEntry)
            return;

        if(other.CompareTag("Player"))
        {
            Debug.Log("In trigger enter");
            // Only begin state machine once
            if (!startStateMachine)
            {
                startStateMachine = true;
                state = 1;
                anim.SetTrigger("TriggerIntro");
            }
        }
    }

    public void stateMachine()
    {
        switch (state)
        {
            case 1:
            case 2:
            case 3:
                // Spawn random enemy at random spawn position
                if (!isSpawning)
                {
                    isSpawning = true;
                    StartCoroutine(beginSpawnEnemy());
                }
                break;

            // If wondering why theres nothing here, check out advanceState() and takeDamage()
            case 4:
                break;

        }
    }


    // Advancing into state 1 is done by room entry or player proximity (designers choice with bool startsFromRoomEntry)
    // Advance state is only called going into the 2nd, 3rd, and 4th state (death)
    public void advanceState()
    {
        // Increment state
        state++;

        // Decrease spawn interval
        currentSpawnInterval /= nextStateSpawnIntervalDivisor;

        // Take damage
        takeDamage();

    }

    public void takeDamage()
    {
        // State 1 isn't checked here obviously because the boss won't be taking damage GOING INTO state 1

        // If GOING INTO 2nd or 3rd state, trigger damage animation and decrement health by a third each time
        if (state == 2 || state == 3)
        {
            AudioSource.PlayClipAtPoint(sfxManager.instance.advSpecHurt, transform.position, sfxManager.instance.advSpecHurtVolume);
            //Animation - set triggerdamage
            anim.SetTrigger("TriggerTakeDamage");

            HP -= MAXHP * (1 / 3);
            //Drop Platforms
            if (state == 2)
                platformSections[0].DropPlatformsAtOnce();
            if (state == 3)
                platformSections[1].DropPlatformsAtOnce();
        }
        
        // Else if completed 3rd state, trigger death animation 
        else if (state == 4)
        {
            AudioSource.PlayClipAtPoint(sfxManager.instance.advSpecDeath, transform.position, sfxManager.instance.advSpecDeathVolume);
            // Animation - set triggerdeath
            anim.SetTrigger("TriggerDeath");

            HP = 0;
            //Drop Platforms
            platformSections[2].DropPlatformsAtOnce();
        }

    }

    // ------------------------- Spawn enemy functions ------------------------------

    float getRandomIntialSpawnInterval()
    {
        return Random.Range(minInitialSpawnInterval, maxInitialSpawnInterval);
    }

    // This decides the random spawn to spawn from, which picks the corresponding animation.
    IEnumerator beginSpawnEnemy()
    {
        int randomSpawnerIndex = Random.Range(0, spawnerPositions.Length);

        switch (randomSpawnerIndex)
        {
            case 0:
                anim.SetTrigger("TriggerSpawnLeft");
                break;

            case 1:
                anim.SetTrigger("TriggerSpawnRight");
                break;

            case 2:
                anim.SetTrigger("TriggerSpawnBack");
                break;
        }

        yield return new WaitForSeconds(currentSpawnInterval);
        isSpawning = false;

    }

    public void animEvent_SpawnEnemyLeft()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int randEnemyType = Random.Range(0, 2); // Only 2 main enemy types (scuttling and humanoid)
        int spawnPos = 0;

        Debug.Log("Spawn Left " + randEnemyType);
        switch(randEnemyType)
        {
            // Spawn a scuttling
            case 0:
                spawnRandomScuttling(spawnPos);
                break;
            
            //Spawn a humanoid
            case 1:
                spawnRandomHumanoid(spawnPos);
                break;

        }
    }

    public void animEvent_SpawnEnemyRight()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int randEnemyType = Random.Range(0, 2); // Only 2 main enemy types (scuttling and humanoid)
        int spawnPos = 1;

        Debug.Log("Spawn Right " + randEnemyType);
        switch (randEnemyType)
        {
            // Spawn a scuttling
            case 0:
                spawnRandomScuttling(spawnPos);
                break;

            //Spawn a humanoid
            case 1:
                spawnRandomHumanoid(spawnPos);
                break;

        }
    }

    public void animEvent_SpawnEnemyBack()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int randEnemyType = Random.Range(0, 1); // Only 2 main enemy types (scuttling and humanoid)
        int spawnPos = 2;

        Debug.Log("Spawn Right " + randEnemyType);
        switch (randEnemyType)
        {
            // Spawn a scuttling
            case 0:
                spawnRandomScuttling(spawnPos);
                break;

            //Spawn a humanoid
            case 1:
                spawnRandomHumanoid(spawnPos);
                break;

        }
    }

    public void animEvent_ExplodeEnemy()
    {
        // Explosion object/particle
        GameObject explosion = Instantiate(explosionOBJ, transform.position + new Vector3(0, 1, 0), transform.rotation);
        Destroy(explosion, 10);

        // Destroy boss
        Destroy(gameObject, 10);
    }

    void spawnRandomScuttling(int spawnPos)
    {
        // Pick a random variation of scuttling
        int randomScuttlingVariation = Random.Range(0, scuttlingEnemies.Length);

        // Instantiate at the randomly chosen spawn postion
        GameObject scuttling = Instantiate(scuttlingEnemies[randomScuttlingVariation], spawnerPositions[spawnPos].position, spawnerPositions[spawnPos].rotation);
        GameObject spawnParticle = Instantiate(spawnParticlePrefab, spawnerPositions[spawnPos].position, spawnerPositions[spawnPos].rotation);

        // Quick fix for varying each humanoids speed when its a boss fight
        NavMeshAgent agent = scuttling.GetComponent<NavMeshAgent>();
        float speed = agent.speed;
        int randInt = Random.Range(0, 3);
        agent.speed = speed - randInt;
        Destroy(spawnParticle, 5);


    }

    void spawnRandomHumanoid(int spawnPos)
    {
        // Pick a random variation of humanoid
        int randomHumanoidVariation = Random.Range(0, humanoidEnemies.Length);

        // Instantiate at the randomly chosen spawn postion
        GameObject humanoid = Instantiate(humanoidEnemies[randomHumanoidVariation], spawnerPositions[spawnPos].position, spawnerPositions[spawnPos].rotation);
        GameObject spawnParticle = Instantiate(spawnParticlePrefab, spawnerPositions[spawnPos].position, spawnerPositions[spawnPos].rotation);

        // Quick fix for humanoid not seeing and chasing player when spawned
        humanoid.GetComponent<SphereCollider>().radius *= 5;

        // Quick fix for varying each humanoids speed when its a boss fight
        NavMeshAgent agent = humanoid.GetComponent<NavMeshAgent>();
        float speed = agent.speed;
        int randInt = Random.Range(0, 6);
        agent.speed = speed - randInt;

        Destroy(spawnParticle, 5);
    }


    // ------------------- Boss button functions -------------------------------

    // This is called from the button that was pushed
    // Once all buttons in this stage have been pushed, advance state and reset the buttons
    public void incrementButtonsHit()
    {
        buttonsHitThisStage++;

        if(buttonsHitThisStage == numberOfButtons)
        {
            // Handles incrementing state, taking damage, and changing spawn interval
            advanceState();
            
            // Handles resetting number of buttons hit and re-allows interaction for the buttons
            resetButtons();
        }
    }

    // After each boss state, reset the number of current buttons hit
    private void resetButtons()
    {
        // Reset current number of buttons hit
        buttonsHitThisStage = 0;

        // Allow interaction with all of the buttons again
        allowAllButtons();
    }

    // After each boss state, re-allow interaction with all buttons
    private void allowAllButtons()
    {
        foreach(GameObject btn in buttons)
        {
            btn.GetComponent<BossButton>().isButtonAllowed = true;
        }
    }

}
