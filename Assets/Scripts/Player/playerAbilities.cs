using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

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
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.bulletTimeEnter[Random.Range(0, sfxManager.instance.bulletTimeEnter.Length)], sfxManager.instance.bulletTimeEnterVol);
            Time.timeScale = gameManager.instance.timeScaleOrig / 10;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed * 12;
            gameManager.instance.playerController.GetComponent<Animator>().speed = gameManager.instance.playerController.GetComponent<Animator>().speed * 8;
        }
        else if(!gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.bulletTimeExit[Random.Range(0, sfxManager.instance.bulletTimeExit.Length)], sfxManager.instance.bulletTimeExitVol);
            Time.timeScale = gameManager.instance.timeScaleOrig;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed / 12;
            gameManager.instance.playerController.GetComponent<Animator>().speed = gameManager.instance.playerController.GetComponent<Animator>().speed / 8;
        }
    }

    public void freezeObjects()
    {
        if (Input.GetButtonDown("Ability"))
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
                        else if(!collider.GetComponent<Rigidbody>())
                        {
                            collider.transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y, collider.transform.position.z);
                        }
                            Transform parent = collider.transform;
                        while (parent.parent != null)
                        {
                            parent = parent.parent;
                        }
                        if (parent.GetComponent<Animator>())
                        {
                            parent.GetComponent<Animator>().enabled = false;
                        }
                        if (parent.GetComponent<enemyAI>())
                        {
                            parent.GetComponent<enemyAI>().enabled = false;
                        }
                        if(parent.GetComponent<enemySlimeAI>())
                        {
                            parent.GetComponent<enemySlimeAI>().enabled = false;
                        }
                        if(parent.GetComponent<enemyHumanoidSpecimenAI>())
                        {
                            parent.GetComponent<enemyHumanoidSpecimenAI>().enabled = false;
                        }
                        if(parent.GetComponent<enemyScuttlingSpecimenAI>())
                        {
                            parent.GetComponent<enemyScuttlingSpecimenAI>().enabled = false;
                        }
                        if(parent.GetComponent<enemyRCCar>())
                        {
                            parent.GetComponent<enemyRCCar>().enabled = false;
                        }
                        if (parent.GetComponent<NavMeshAgent>())
                        {
                            parent.GetComponent<NavMeshAgent>().enabled = false;
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

        Rigidbody rb = collider.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        Transform parent = rb.transform;
        while (parent.parent != null)
        {
            parent = parent.parent;
        }
        if (parent.GetComponent<Animator>())
        {
            parent.GetComponent<Animator>().enabled = true;
        }
        if (parent.GetComponent<enemyAI>())
        {
            parent.GetComponent<enemyAI>().enabled = true;
        }
        if (parent.GetComponent<enemySlimeAI>())
        {
            parent.GetComponent<enemySlimeAI>().enabled = true;
        }
        if (parent.GetComponent<enemyHumanoidSpecimenAI>())
        {
            parent.GetComponent<enemyHumanoidSpecimenAI>().enabled = true;
        }
        if (parent.GetComponent<enemyScuttlingSpecimenAI>())
        {
            parent.GetComponent<enemyScuttlingSpecimenAI>().enabled = true;
        }
        if (parent.GetComponent<enemyRCCar>())
        {
            parent.GetComponent<enemyRCCar>().enabled = true;
        }
        if (parent.GetComponent<NavMeshAgent>())
        {
            parent.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
