    using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            case 2:
                freezeObjects();
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
        if(gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            Time.timeScale = gameManager.instance.timeScaleOrig / 10;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed * 12;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed * 8;
        }
        else if(!gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            Time.timeScale = gameManager.instance.timeScaleOrig;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed / 12;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].GetComponent<Animator>().speed / 8;
        }
    }

    public void freezeObjects()
    {
        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            Debug.Log("Freezing objects");
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit))
            {
                Collider[] objects = Physics.OverlapSphere(hit.point, 5);

                foreach(Collider collider in objects)
                {
                    if(collider.tag != "Player")
                    {
                        if (collider.GetComponent<Rigidbody>() != null)
                        {
                            Debug.Log("Freezing " + collider.name);
                            Rigidbody rb = collider.GetComponent<Rigidbody>();
                            rb.constraints = RigidbodyConstraints.FreezeAll;

                        }
                        if (collider.GetComponent<Animator>())
                        {
                            collider.GetComponent<Animator>().enabled = false;
                        }
                        if (collider.GetComponent<enemyAI>())
                        {
                            collider.GetComponent<enemyAI>().enabled = false;
                        }
                        StartCoroutine(unfreezeDelay(collider));
                    }
                }
            }
        }
    }

    public IEnumerator unfreezeDelay(Collider collider)
    {
        Debug.Log("Starting unfreeze on " + collider.name);
        yield return new WaitForSeconds(10);
        if(collider.GetComponent<Rigidbody>())
        {
            collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        if (collider.GetComponent<Collider>().GetComponent<Animator>())
        {   
            collider.GetComponent<Collider>().GetComponent<Animator>().enabled = true;
        }   
        if (collider.GetComponent<Collider>().GetComponent<enemyAI>())
        {  
            collider.GetComponent<Collider>().GetComponent<enemyAI>().enabled = true;
        }

    }
}
