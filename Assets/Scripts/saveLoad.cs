using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour
{

    private void Start()
    {
        loadFromMainMenu();
    }

    public static void saveFromMainMenuOptions()
    {
        PlayerPrefs.SetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxManager.instance.aud.volume);
        PlayerPrefs.SetFloat("MusicVolumeSliderValue", msuicManager.instance.musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", msuicManager.instance.aud.volume);


        PlayerPrefs.Save();
        //Save all permament upgrade checks here

    }

    public static void saveFromPauseMenuOptions()
    {
        PlayerPrefs.SetInt("Character", gameManager.instance.playerController.currCharacter);
        PlayerPrefs.SetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxManager.instance.aud.volume);
        PlayerPrefs.SetFloat("MusicVolumeSliderValue", gameManager.instance.musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", gameManager.instance.composer.speaker.volume);

        PlayerPrefs.Save();
        //Save all permament upgrade checks here

    }

    public static void loadFromMainMenu()
    { 
        if(PlayerPrefs.HasKey("SFXVolumeSliderValue"))
        {
            sfxManager.instance.sfxVolumeSlider.value = (PlayerPrefs.GetFloat("SFXVolumeSliderValue"));
        }
        if (PlayerPrefs.HasKey("SFXVolumeSliderValue"))
        {
            sfxManager.instance.aud.volume = PlayerPrefs.GetFloat("SFXVolume", 0);
        }


        if (PlayerPrefs.HasKey("MusicVolumeSliderValue"))
        {
            msuicManager.instance.musicVolumeSlider.value = (PlayerPrefs.GetFloat("MusicVolumeSliderValue"));
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            msuicManager.instance.aud.volume = PlayerPrefs.GetFloat("MusicVolume", 0);
        }

    }

    public static void loadFromMainGame()
    {
        if(PlayerPrefs.HasKey("Character"))
        {
            gameManager.instance.playerController.currCharacter = 
            PlayerPrefs.GetInt("Character", gameManager.instance.playerController.currCharacter);
        }

        if (PlayerPrefs.HasKey("SFXVolumeSliderValue"))
        {
            sfxManager.instance.sfxVolumeSlider.value =
            PlayerPrefs.GetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        }
            
        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxManager.instance.aud.volume = 
            PlayerPrefs.GetFloat("SFXVolume", sfxManager.instance.aud.volume);
        }

        if(PlayerPrefs.HasKey("MusicVolumeSliderValue"))
        {
            gameManager.instance.musicVolumeSlider.value = 
            PlayerPrefs.GetFloat("MusicVolumeSliderValue", gameManager.instance.musicVolumeSlider.value);
        }

        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            gameManager.instance.composer.speaker.volume =
            PlayerPrefs.GetFloat("MusicVolume", gameManager.instance.composer.speaker.volume);
        }

        if (PlayerPrefs.HasKey("MusicVolumeSliderValue"))
        {
            gameManager.instance.musicVolumeSlider.value =
            PlayerPrefs.GetFloat("MusicVolumeSliderValue", gameManager.instance.musicVolumeSlider.value);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            gameManager.instance.composer.speaker.volume =
            PlayerPrefs.GetFloat("MusicVolume", gameManager.instance.composer.speaker.volume);
        }

    }
}
