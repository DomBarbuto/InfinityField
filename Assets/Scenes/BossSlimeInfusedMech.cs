using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeInfusedMech : MonoBehaviour, IRoomEntryListener
{
    [SerializeField] int HP;
    [SerializeField] Animator anim;
    [SerializeField] List<enemySpawner> spawners;
    [SerializeField] GameObject body;

    int MAXHP;
    bool startStateMachine;
    int state;
    Vector3 playerDir;
    bool attackState;
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

    }

    public void SlowLookAt(int speed)
    {
        playerDir = gameManager.instance.player.transform.position;
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        body.transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speed); // Smooth rotation
    }

    public void stateMachine()
    {
        switch (state)
        {
            //1st wave happens when under 1/3 health
            case 1:
                if (attackState)
                {
                    //Start shooting
                    
                    //Start following the player by rotating at a slow speed
                    SlowLookAt(1);
                    //Start the end of the 
                }
                break;
            case 2:

                break;
            case 3:

                break;
        }
    }

    public IEnumerator timedShoot(int length)
    {
        anim.SetBool("Shooting", true);
        return new 
    }
}
