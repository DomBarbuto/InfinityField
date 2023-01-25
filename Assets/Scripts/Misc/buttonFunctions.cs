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
/*    [SerializeField] public AudioMixerSnapshot fadeInSnapShot;
    [SerializeField] public AudioMixerSnapshot fadedOutAudioSnapshot;*/


    private void Start()
    {
        gameManager.instance.fadeInSnapShot.TransitionTo(2);

        loadOptions();

        //Character loadin is done in gamemanaeger
    }

    // ----------------- PAUSE MENU -------------------
    public void resume()
    {
        gameManager.instance.unPause();
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.playerController.resetPlayerHP();
        gameManager.instance.updatePlayerHPBar();
        gameManager.instance.playerController.resetPlayerEnergy();
        gameManager.instance.updatePlayerEnergyBar();
    }
    public void mainMenu()
    {
        Time.timeScale = gameManager.instance.timeScaleOrig;
        SceneManager.LoadScene(0);
    }
    public void quit()
    {
        gameManager.instance.fadedOutSnapshot.TransitionTo(2);

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

    public void backtoMainMenu()
    {
        gameManager.instance.fadeInSnapShot.TransitionTo(2);
        SceneManager.LoadScene(0);
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
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.optionsMenu);
    }

    public void returnFromOptionsMenu()
    {
        saveOptions();

        // Turn off options menu
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.pauseMenu);
    }

    // ----------------- CONTROLS -------------------

    public void pullUpControlsMenu()
    {
        // Turn off main menu
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.controlsMenu);
    }

    public void returnFromControlsMenu()
    {
        // Turn off controls menu
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.pauseMenu);
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

    
}
