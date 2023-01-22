using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomeSecuritySystem : MonoBehaviour, IRoomEntryListener
{
    [SerializeField] GameObject healthBarPrefab;
    BossHealthBar bossHPBarScript;

    [SerializeField] float HP;
    [SerializeField] List<GameObject> FirstWave;
    [SerializeField] List<GameObject> SecondWave;
    [SerializeField] List<GameObject> ThirdWave;
    [SerializeField] int state;
    [SerializeField] bool stateMachineOn;
    [SerializeField] bool stateStarted;
    [SerializeField] bool waveComplete;
    [SerializeField] GameObject exit;
    [SerializeField] AudioSource aud;


    private float MAXHP;


    List<GameObject> nullObjects;
    // Start is called before the first frame update
    public void notify()
    {
        bossHPBarScript = healthBarPrefab.GetComponent<BossHealthBar>();
        healthBarPrefab.gameObject.SetActive(false);
        MAXHP = HP;

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
                {
                    playPhase1Sound();
                    takeDamage();
                }
                wave(FirstWave);
                break;
            case 2:
                if (!stateStarted)
                {
                    playPhase2Sound();
                    takeDamage();
                }
                wave(SecondWave);
                break;
            case 3:
                if (!stateStarted)
                {
                    playPhase3Sound();
                    takeDamage();
                }
                wave(ThirdWave);
                break;
            case 4:
                if (!stateStarted)
                {
                    bossHPBarScript.destroyHealthBar();
                    playDeathSound();
                    exit.SetActive(true);
                    stateStarted = true;
                }
                //End Stuff
                break;
        }

    }

    private void takeDamage()
    {
        float prevHealth = HP;

        if (state == 2 || state == 3)
        {
            if (state == 2)
                bossHPBarScript.turnOffState(1);

            if (state == 3)
                bossHPBarScript.turnOffState(2);

            HP -= MAXHP * (1 / 3);

            // Update health bar
            bossHPBarScript.updateHealthFillAmount(prevHealth, HP, MAXHP);

        }
        else if (state == 4)
        {
            prevHealth = HP;
            HP = 0;

            // Update health bar
            bossHPBarScript.updateHealthFillAmount(prevHealth, HP, MAXHP);
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

    void playDeathSound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSDeath);
    }

    void playIntroSound()
    {
        //aud.PlayOneShot(sfxManager.instance.HSSIntro);
        aud.clip = sfxManager.instance.HSSIntro;
        aud.Play();
    }

    void playPhase1Sound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSPhase1);
    }

    void playPhase2Sound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSPhase2);
    }

    void playPhase3Sound()
    {
        aud.PlayOneShot(sfxManager.instance.HSSPhase3);
    }


}
