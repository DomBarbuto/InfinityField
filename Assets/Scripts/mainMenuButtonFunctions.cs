using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class mainMenuButtonFunctions : MonoBehaviour
{
    [SerializeField] GameObject mainMenuObject;
    [SerializeField] GameObject optionsMenuObject;
    [SerializeField] GameObject controlsMenuObject;
    [SerializeField] GameObject creditsMenuObject;
    [SerializeField] GameObject observationsMenuObject;

    // Settings
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioMixer mixer;

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("Volume_Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("Volume_SFX", Mathf.Log10(sliderValue) * 20);
    }

    private void Start()
    {
        loadOptions();
    }


    // Options
    public void pullUpOptionsMenu()
    {
        // Turn off main menu
        mainMenuObject.SetActive(false);
        optionsMenuObject.SetActive(true); 
    }

    public void returnFromOptionsMenu()
    {
        // Turn off options menu
        optionsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);

        saveOptions();
    }

    // Controls
    public void pullUpControlsMenu()
    {
        // Turn off main menu
        mainMenuObject.SetActive(false);
        controlsMenuObject.SetActive(true);
    }

    public void returnFromControlsMenu()
    {
        // Turn off controls menu
        controlsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    // Observations

    public void pullUpObservationsMenu()
    {
        // Turn off main menu
        mainMenuObject.SetActive(false);
        observationsMenuObject.SetActive(true);
    }

    public void returnFromObservationsMenu()
    {
        // Turn off controls menu
        observationsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    // Credits
    public void pullUpCreditsMenu()
    {
        // Turn off main menu
        mainMenuObject.SetActive(false);
        creditsMenuObject.SetActive(true);
    }

    public void returnFromCreditsMenu()
    {
        creditsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void saveOptions()
    {
        // Music volume
        float mixerMusicVolume; 
        mixer.GetFloat("Volume_Music", out mixerMusicVolume);
        PlayerPrefs.SetFloat("MusicVolume", mixerMusicVolume);

        //Music slider value
        float musicSliderValue = musicSlider.value;
        PlayerPrefs.SetFloat("MusicSliderValue", musicSliderValue);

        // SFX volume
        float mixerSFXVolume;
        mixer.GetFloat("Volume_SFX", out mixerSFXVolume);
        PlayerPrefs.SetFloat("SFXVolume", mixerSFXVolume);

        //SFX slider value
        float sfxSliderValue = sfxSlider.value;
        PlayerPrefs.SetFloat("sfxSliderValue", sfxSliderValue);


        PlayerPrefs.Save();
    }



    public void loadOptions()
    {
        // Music Volume
        mixer.SetFloat("Volume_Music", PlayerPrefs.GetFloat("MusicVolume", 0.5f));

        // Music slider value
        float musicSliderValue = PlayerPrefs.GetFloat("MusicSliderValue", 0.5f);
        musicSlider.value = musicSliderValue;

        // SFX Volume
        mixer.SetFloat("Volume_SFX", PlayerPrefs.GetFloat("SFXVolume", 0.5f));

        // SFX slider value
        float sfxSliderValue = PlayerPrefs.GetFloat("sfxSliderValue", 0.5f);
        sfxSlider.value = sfxSliderValue;
    }
}
