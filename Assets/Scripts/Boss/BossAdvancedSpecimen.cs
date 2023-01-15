using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAdvancedSpecimen : MonoBehaviour, IRoomEntryListener
{
    [Header("---------- Components/Prefabs ----------")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject[] scuttlingEnemies;
    [SerializeField] GameObject[] humanoidEnemies;

    [Header("---------- Stats ----------")]
    [SerializeField] int HP;
    [SerializeField] List<Transform> spawnerPositions;

    /*
     * This boss regarding the spawning of enemies is unique because the spawning of the enemies
     * continues on indefinitely at a specific rate (boss fight state-dependent) until the player hits
     * all buttons to progress to the next state. So for each state, random enemies will spawn from random 
     * spawn positions, causing the corresponding animation (spawn left, spawn right, or spawn back). 
     * As the states progess, the rate at which an enemy spawns is incresed.
     */
    [Header("---------- Behaviour ----------")]
    [SerializeField] bool startsFromRoomEntry;
    [SerializeField] bool startStateMachine;
    [SerializeField] int state;

    [Header("---------- Platforms and Buttons ----------")]
    [SerializeField] PlatformSection[] platformSections;
    [SerializeField] GameObject[] buttons;            
    [SerializeField] int buttonsHitThisStage;
    [SerializeField] public int numberOfButtons;    // Incremented from each buttons start

    //Misc
    int MAXHP;

    private void Start()
    {
        MAXHP = HP;
        buttonsHitThisStage = 0;

    }

    private void Update()
    {
        if(startStateMachine)
        {
            stateMachine();
        }
    }



    // On Room entry... if boss is set to activate on room entry / other option is to trigger the boss on trigger enter
    public void notify()
    {
        if (!startsFromRoomEntry)
            return;

        startStateMachine = true;
        state = 1;
    }

    public void stateMachine()
    {
        switch (state)
        {
            //1st wave happens when under 1/3 health
            case 1:
                
                break;
            case 2:
                
                break;
            case 3:
                
                break;
            case 4:
                break;
                /*//Win State, boss depleted. Insert any necessary code here when the time comes.
                anim.SetBool("Shooting", false);
                shooting = false;
                attackState = false;
                break;*/
        }
    }

    // Only for the case of the boss starting the fight upon player entering a trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Start from room entry is the other option, so do nothing if this boss fight begins upon room entry
        if (startsFromRoomEntry)
            return;

        // Only begin state machine once
        if(!startStateMachine)
            startStateMachine = true;
    }

    public void takeDamage(int dmg)
    {
        /*HP -= dmg;
        if (HP <= ((MAXHP / 3) * 2))
        {
            state = 2;
        }
        if (HP <= (MAXHP / 3))
        {
            state = 3;
        }*/
    }

    public void incrementButtonsHit()
    {
        buttonsHitThisStage++;
    }

    // After each boss state, reset the number of current buttons hit
    private void resetButtonsHit()
    {
        buttonsHitThisStage = 0;
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
