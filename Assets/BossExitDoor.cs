using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossExitDoor : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject textBox;
    [SerializeField] string nextScene;

    // Start is called before the first frame update
    public void interact()
    {
        //SceneManager.LoadScene(nextScene);
        Debug.Log("Booba");
        SceneManager.LoadScene(0);
    }

 
    public void showText()
    {
        textBox.SetActive(true);
    }

    public void HideText()
    {
        textBox.SetActive(false);
    }
}
