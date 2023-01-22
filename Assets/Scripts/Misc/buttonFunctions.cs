using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] public AudioMixer mixer;

    [SerializeField] CharacterSelectionShowcase characterSelect;
    public void resume()
    {
        gameManager.instance.unPause();
    }

    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("Volume_Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMasterSFXVolume(float sliderValue)
    {
        mixer.SetFloat("Volume_SFX", Mathf.Log10(sliderValue) * 20);
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void quit()
    {
        /*saveLoad.saveFromPauseMenuOptions();
        PlayerPrefs.DeleteKey("weaponList");
        PlayerPrefs.DeleteKey("perkList");*/
        Application.Quit();
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void respawnPlayer()
    {
        gameManager.instance.playerController.resetPlayerHP();
        gameManager.instance.updatePlayerHPBar();
        gameManager.instance.unPause();
        gameManager.instance.playerController.setPlayerPos();
    }

    public void pullUpOptionsMenu()
    {
        // Turn off main menu
        gameManager.instance.activeMenu.SetActive(false);

        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.optionsMenu);
    }

    public void returnToCorrectMenu()
    {
        // Turn off options menu
        gameManager.instance.activeMenu.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            //Return to main menu
            gameManager.instance.SetActiveMenu(gameManager.UIMENUS.mainMenu);
        }
        else
        {
            //Return to pause menu
            gameManager.instance.SetActiveMenu(gameManager.UIMENUS.pauseMenu);
        }

    }
    public void next()
    {
        if (characterSelect.characterNumber + 1 <= characterSelect.characters.Length - 1)
        {
            characterSelect.characterNumber += 1;
        }
        else
        {
            characterSelect.characterNumber = 0;
        }
        characterSelect.changeCharacter();
    }

    public void previous()
    {
        if (characterSelect.characterNumber - 1 >= 0)
        {   
            characterSelect.characterNumber -= 1;
        }   
        else
        {   
            characterSelect.characterNumber = characterSelect.characters.Length - 1;
        }
        characterSelect.changeCharacter();
    }
    public void play()
    {
        /*saveLoad.saveFromPauseMenuOptions();
        PlayerPrefs.DeleteKey("weaponList");
        PlayerPrefs.DeleteKey("perkList");*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
