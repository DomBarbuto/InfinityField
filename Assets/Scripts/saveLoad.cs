using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour
{
    
    public static void save()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
            PlayerPrefs.SetInt("Character", gameManager.instance.playerController.currCharacter);
        PlayerPrefs.SetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxManager.instance.aud.volume);
        //Save all permament upgrade checks here

    }

    public static void load()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
            PlayerPrefs.GetInt("Character", gameManager.instance.playerController.currCharacter);
        PlayerPrefs.GetFloat("SFXVolumeSliderValue", sfxManager.instance.sfxVolumeSlider.value);
        PlayerPrefs.GetFloat("SFXVolume", sfxManager.instance.aud.volume);
        //Load all permament upgrade checks here
    }
}
