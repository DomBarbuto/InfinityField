using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] CharacterSelectionShowcase characterSelect;
    public void resume()
    {
        gameManager.instance.unPause();
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        saveLoad.saveFromMainMenuOptions();
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
            characterSelect.characterNumber = 1;
        }   
        else
        {   
            characterSelect.characterNumber = characterSelect.characters.Length - 1;
        }
        characterSelect.changeCharacter();
    }
}
