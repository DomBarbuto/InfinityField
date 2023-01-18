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
    [SerializeField] GameObject exit;
    // Start is called before the first frame update
    public void notify()
    {
        AudioSource.PlayClipAtPoint(sfxManager.instance.HSSIntro, transform.position, sfxManager.instance.HSSIntroVolume);
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
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSPhase1, transform.position, sfxManager.instance.HSSPhase1Volume);
                wave(FirstWave);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSPhase2, transform.position, sfxManager.instance.HSSPhase2Volume);
                wave(SecondWave);
                break;
            case 3:
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSPhase3, transform.position, sfxManager.instance.HSSPhase3Volume);
                wave(ThirdWave);
                break;
            case 4:
                exit.SetActive(true);
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSDeath, transform.position, sfxManager.instance.HSSDeathVolume);
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
