using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomeSecuritySystem : MonoBehaviour, IRoomEntryListener
{
    [SerializeField] List<enemySpawner> FirstWave;
    [SerializeField] List<enemySpawner> SecondWave;
    [SerializeField] List<enemySpawner> ThirdWave;
    [SerializeField] int state;
    [SerializeField] bool stateMachineOn;
    [SerializeField] bool stateStarted;
    [SerializeField] bool waveComplete;
    // Start is called before the first frame update
    public void notify()
    {
        state = 1;
        stateMachineOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachineOn)
        {
            stateMachine();
        }
    }

    void stateMachine()
    {
        switch (state)
        {
            case 1:
                wave(FirstWave);
                break;
            case 2:
                wave(SecondWave);
                break;
            case 3:
                wave(ThirdWave);
                break;
            case 4:
                //End Stuff
                break;
        }

    }

    void wave(List<enemySpawner> _wave)
    {
        if (!stateStarted)
        {
            foreach (enemySpawner spawner in _wave)
            {
                spawner.triggerSpawn();
                stateStarted = true;
            }
        }
        else
        {
            foreach (enemySpawner spawner in _wave)
            {
                if (spawner.enemies.Count == 0)
                    waveComplete = true;
                else
                    waveComplete = false;
            }
            if (waveComplete)
            {
                state++;
            }
        }
    }
}
