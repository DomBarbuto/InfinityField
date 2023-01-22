using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathButtonFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit(); // Calling this function automatically saves playerPrefs in the background
    }
}
