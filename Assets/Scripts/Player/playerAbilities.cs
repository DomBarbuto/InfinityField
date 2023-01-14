    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAbilities : MonoBehaviour
{

    public void useAbility()
    {
        switch (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].ability)
        {
            case 0:
                sprint();
                break;
            case 1:
                bulletTime();
                break;
            default:
                Debug.Log("This character currently has no ability or is broken");
                break;
        }
    }


    //These will all be seperate functions for different abilities of characters (May also contain ability upgrades)

    public void sprint()
    {
        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            gameManager.instance.playerController.isSprinting = true;
            gameManager.instance.playerController.animController.switchSprintingState(true);
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].currSpeed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed * 1.7f;
        }
        
        
    }
    
    public void bulletTime()
    {
        if(Time.timeScale == gameManager.instance.timeScaleOrig)
        {
            Time.timeScale = gameManager.instance.timeScaleOrig / 10;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed * 12;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed * 8;
        }
        else if(Time.timeScale != gameManager.instance.timeScaleOrig)
        {
            Time.timeScale = gameManager.instance.timeScaleOrig;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed / 12;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed / 8;
        }
    }
}
