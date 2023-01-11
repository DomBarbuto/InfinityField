using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuButtonFunctions : MonoBehaviour
{
    [SerializeField] GameObject mainMenuObject;
    [SerializeField] GameObject optionsMenuObject;
    [SerializeField] GameObject creditsMenuObject;


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
        saveLoad.save();

    }

    /*public void pullUpCreditsMenu()
    {
        // Turn off main menu
        mainMenuObject.SetActive(false);

        gameManager.instance.SetActiveMenu(gameManager.UIMENUS.optionsMenu);
    }*/

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quit()
    {
        saveLoad.save();
        Application.Quit();
    }
}
