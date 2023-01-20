using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomeSecuritySystem : MonoBehaviour, IRoomEntryListener
{
    [SerializeField] List<GameObject> FirstWave;
    [SerializeField] List<GameObject> SecondWave;
    [SerializeField] List<GameObject> ThirdWave;
    [SerializeField] int state;
    [SerializeField] bool stateMachineOn;
    [SerializeField] bool stateStarted;
    [SerializeField] bool waveComplete;
    [SerializeField] GameObject exit;

    List<GameObject> nullObjects;
    // Start is called before the first frame update
    public void notify()
    {
        AudioSource.PlayClipAtPoint(sfxManager.instance.HSSIntro, transform.position, sfxManager.instance.HSSIntroVolumeMulti);
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
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSPhase1, transform.position, sfxManager.instance.HSSPhase1VolumeMulti);
                wave(FirstWave);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSPhase2, transform.position, sfxManager.instance.HSSPhase2VolumeMulti);
                wave(SecondWave);
                break;
            case 3:
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSPhase3, transform.position, sfxManager.instance.HSSPhase3VolumeMulti);
                wave(ThirdWave);
                break;
            case 4:
                if (!stateStarted)
                {
                    exit.SetActive(true);
                    stateStarted = true;
                }
                AudioSource.PlayClipAtPoint(sfxManager.instance.HSSDeath, transform.position, sfxManager.instance.HSSDeathVolumeMulti);
                //End Stuff
                break;
        }

    }

    void wave(List<GameObject> _wave)
    {
        if (!stateStarted)
        {
            foreach (GameObject enemy in _wave)
            {
                enemy.SetActive(true);
                stateStarted = true;
            }
        }
        else
        {
            nullObjects = _wave;
            foreach (GameObject enemy in _wave)
            {
                if (enemy == null)
                    nullObjects.Remove(enemy);
            }
            _wave = nullObjects;

            if (_wave.Count == 0)
                waveComplete = true;

            if (waveComplete)
            {
                waveComplete = false;
                stateStarted = false;
                state++;
            }
        }
    }
}
