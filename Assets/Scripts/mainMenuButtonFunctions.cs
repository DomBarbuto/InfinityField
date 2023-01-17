using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuButtonFunctions : MonoBehaviour
{
    [SerializeField] GameObject mainMenuObject;
    [SerializeField] GameObject optionsMenuObject;
    [SerializeField] GameObject controlsMenuObject;
    [SerializeField] GameObject creditsMenuObject;
    [SerializeField] GameObject observationsMenuObject;
    private void Start()
    {
        saveLoad.loadFromMainGame();
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
        saveLoad.saveFromMainMenuOptions();

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

    public void leftObservationBUtton()
    {

    }

    public void rightObservationButton()
    {

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
        saveLoad.saveFromMainMenuOptions();
        Application.Quit();
    }
}
