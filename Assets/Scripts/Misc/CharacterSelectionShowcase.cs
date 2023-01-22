using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CharacterSelectionShowcase : MonoBehaviour
{
    [SerializeField] public GameObject[] characters;
    [SerializeField] GameObject currCharacter;
    [SerializeField] public int characterNumber;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] GameObject fadeInOutOBJ;


    [Header("Button Sounds")]
    [SerializeField] AudioMixer mixer; // Main mixer to grab volume from on start (just for this scene)
    [SerializeField] AudioSource aud;  // It's own audio source
    [SerializeField] AudioMixerSnapshot fadedOutAudioSnapshot;
    [SerializeField] public AudioClip buttonEnter;
    [SerializeField] public AudioClip buttonSelect;
    [SerializeField] EventSystem sys;

    private void Start()
    {
        fadeInOutOBJ.SetActive(true);

        // Grab saved volumes from settings (lightweight copy of regular loadOptions() because there are no sliders in this scene)
        loadOptions();


        // Initial character 
        currCharacter = Instantiate(characters[characterNumber], transform.position, transform.rotation);
        switch (characterNumber)
        {
            case 0:
                text.text = "Velocity";
                break;
            case 1:
                text.text = "Tachyon";
                break;
            case 2:
                text.text = "Zero";
                break;
        }
    }

    private void Update()
    {
        // Also hear sound when pressing enter on a button
        if (sys.currentSelectedGameObject != null && Input.GetButtonDown("Submit"))
        {
            aud.PlayOneShot(buttonSelect);
        }
    }

    public void loadOptions()
    {
        // Music Volume
        mixer.SetFloat("Volume_Music", PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        // SFX Volume
        mixer.SetFloat("Volume_SFX", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    public void changeCharacter()
    {
        Destroy(currCharacter);
        currCharacter = Instantiate(characters[characterNumber], transform.position, transform.rotation);
        PlayerPrefs.SetInt("Character", characterNumber);

        switch(characterNumber)
        {
            case 0:
                text.text = "Velocity";
                    break;
            case 1:
                text.text = "Tachyon";
                    break;
            case 2:
                text.text = "Zero";
                    break;
        }
    }


    // ----------------- CHARACTER SELECTION -------------------

    public void next()
    {
        if (characterNumber + 1 <= characters.Length - 1)
        {
            characterNumber += 1;
        }
        else
        {
            characterNumber = 0;
        }
        changeCharacter();
    }

    public void previous()
    {
        if (characterNumber - 1 >= 0)
        {
            characterNumber -= 1;
        }
        else
        {
            characterNumber = characters.Length - 1;
        }
        changeCharacter();
    }

    public void play()
    {
        PlayerPrefs.Save(); // Since we may have switched characters, still need to save

        beginFadeAndLoadNextLevel();
    }

    public void beginFadeAndLoadNextLevel()
    {
        fadeInOutOBJ.GetComponent<Animator>().SetTrigger("FadeOut");
        fadedOutAudioSnapshot.TransitionTo(2);

        StartCoroutine(waitForNextLevel());
    }

    IEnumerator waitForNextLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


}
