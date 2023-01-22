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

    // ----------------- PAUSE MENU -------------------
    public void resume()
    {
        gameManager.instance.unPause();
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
        PlayerPrefs.DeleteKey("weaponList");
        PlayerPrefs.DeleteKey("perkList");
        saveCharacterSettings();
        Application.Quit();     // Saving automatically gets done when calling Application.Quit as well
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

    // ----------------- OPTIONS -------------------

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("Volume_Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("Volume_SFX", Mathf.Log10(sliderValue) * 20);
    }
    public void pullUpOptionsMenu()
    {
        // Turn off main menu
        /*pauseMenuObject.SetActive(false);
        optionsMenuObject.SetActive(true);*/
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.optionsMenu);
    }

    public void returnFromOptionsMenu()
    {
        saveOptions();

        // Turn off options menu
        /*optionsMenuObject.SetActive(false);
        pauseMenuObject.SetActive(true);*/

        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.pauseMenu);
    }

    // ----------------- CONTROLS -------------------

    public void pullUpControlsMenu()
    {
        // Turn off main menu
        /*pauseMenuObject.SetActive(false);
        controlsMenuObject.SetActive(true);*/

        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.controlsMenu);
    }

    public void returnFromControlsMenu()
    {
        // Turn off controls menu
        /*controlsMenuObject.SetActive(false);
        pauseMenuObject.SetActive(true);*/
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.pauseMenu);
    }


    // ----------------- CHARACTER SELECTION -------------------

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


    // ----------------- SAVING/LOADING -------------------


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

    public static void saveCharacterSettings()
    {
        PlayerPrefs.SetInt("Character", gameManager.instance.playerController.currCharacter);

        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].perks != null)
        {
            List<perkList> perkList = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].perks;

            string jsonPerkString = JsonUtility.ToJson(perkList);

            PlayerPrefs.SetString("myObjectList", jsonPerkString);
        }

        if (gameManager.instance.playerController.weaponInventory != null)
        {
            List<weaponCreation> weaponList = gameManager.instance.playerController.weaponInventory;

            string jsonWeaponString = JsonUtility.ToJson(weaponList);

            PlayerPrefs.SetString("myObjectList", jsonWeaponString);
        }
        PlayerPrefs.Save();
        //Save all permament upgrade checks here

    }

    public static void loadCharacterSettings()
    {
        if (PlayerPrefs.HasKey("Character"))
        {
            gameManager.instance.playerController.currCharacter =
            PlayerPrefs.GetInt("Character", gameManager.instance.playerController.currCharacter);
        }

        if (PlayerPrefs.HasKey("weaponList"))
        {
            string weaponLoad = PlayerPrefs.GetString("weaponList");

            List<weaponCreation> weaponLoadList = JsonUtility.FromJson<List<weaponCreation>>(weaponLoad);
        }

        if (PlayerPrefs.HasKey("perkList"))
        {
            string perkLoad = PlayerPrefs.GetString("perkList");

            List<perkList> perkLoadList = JsonUtility.FromJson<List<perkList>>(perkLoad);
        }
    }

    // Unused 
    public void play()
    {
        // Commented out because keysare already being deleted on quit

        /*saveLoad.saveFromPauseMenuOptions();
        PlayerPrefs.DeleteKey("weaponList");
        PlayerPrefs.DeleteKey("perkList");*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
