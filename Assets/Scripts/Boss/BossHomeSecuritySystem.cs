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
    [SerializeField] AudioSource aud;

    List<GameObject> nullObjects;
    // Start is called before the first frame update
    public void notify()
    {
        Debug.Log("Notify");
        playIntroSound();
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
                if (!stateStarted)
                    playPhase1Sound();
                wave(FirstWave);
                break;
            case 2:
                if (!stateStarted)
                    playPhase2Sound();
                wave(SecondWave);
                break;
            case 3:
                if (!stateStarted)
                    playPhase3Sound();
                wave(ThirdWave);
                break;
            case 4:
                if (!stateStarted)
                {
                    playDeathSound();
                    exit.SetActive(true);
                    stateStarted = true;
                }
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

    public void playDeathSound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSDeath);
    }

    public void playIntroSound()
    {
        //aud.PlayOneShot(sfxManager.instance.HSSIntro);
        aud.clip = sfxManager.instance.HSSIntro;
        aud.Play();
    }

    public void playPhase1Sound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSPhase1);
    }

    public void playPhase2Sound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSPhase2);
    }

    public void playPhase3Sound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSPhase3);
    }

}
