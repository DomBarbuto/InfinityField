using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour
{
    
    public static void saveFromMainMenuOptions()
    {
        PlayerPrefs.SetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxManager.instance.aud.volume);
        //Save all permament upgrade checks here

    }

    public static void saveFromPauseMenuOptions()
    {
        PlayerPrefs.SetInt("Character", gameManager.instance.playerController.currCharacter);
        PlayerPrefs.SetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxManager.instance.aud.volume);
        PlayerPrefs.SetFloat("MusicVolumeSliderValue", gameManager.instance.musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", gameManager.instance.composer.speaker.volume);
        //Save all permament upgrade checks here

    }

    public static void loadFromMainMenu()
    { 
        sfxManager.instance.sfxVolumeSlider.value =
        PlayerPrefs.GetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);

        sfxManager.instance.aud.volume =
        PlayerPrefs.GetFloat("SFXVolume", sfxManager.instance.aud.volume);
    }

    public static void loadFromMainGame()
    {
        gameManager.instance.playerController.currCharacter = 
        PlayerPrefs.GetInt("Character", gameManager.instance.playerController.currCharacter);

        sfxManager.instance.sfxVolumeSlider.value = 
        PlayerPrefs.GetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);

        sfxManager.instance.aud.volume = 
        PlayerPrefs.GetFloat("SFXVolume", sfxManager.instance.aud.volume);

        gameManager.instance.musicVolumeSlider.value = 
        PlayerPrefs.GetFloat("MusicVolumeSliderValue", gameManager.instance.musicVolumeSlider.value);

        gameManager.instance.composer.speaker.volume =
        PlayerPrefs.GetFloat("MusicVolume", gameManager.instance.composer.speaker.volume);
    }
}
