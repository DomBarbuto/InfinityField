using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveLoad : MonoBehaviour
{
    
    public void save()
    {
        PlayerPrefs.SetInt("Character", gameManager.instance.playerController.character);
        
        //Save all permament upgrade checks here

    }

    public void load()
    {
        PlayerPrefs.GetInt("Character", gameManager.instance.playerController.character);

        //Load all permament upgrade checks here
    }
}
