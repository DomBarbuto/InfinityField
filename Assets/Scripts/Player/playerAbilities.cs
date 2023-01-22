using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class playerAbilities : MonoBehaviour
{

    public AudioSource aud;

    public void useAbility()
    {
        switch (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].ability)
        {
            case playerCharacter.abilityList.sprint:
                sprint();
                break;
            case playerCharacter.abilityList.bulletTime:
                bulletTime();
                break;
            case playerCharacter.abilityList.freeze:
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
        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            Time.timeScale = gameManager.instance.timeScaleOrig / 10;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed * 12;
            gameManager.instance.playerController.GetComponent<Animator>().speed = gameManager.instance.playerController.GetComponent<Animator>().speed * 8;
             aud.PlayOneShot(sfxManager.instance.bulletTimeEnter[Random.Range(0, sfxManager.instance.bulletTimeEnter.Length)]);
        }
        else if (!gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].isUsingAbility)
        {
            Time.timeScale = gameManager.instance.timeScaleOrig;
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].speed / 12;
            gameManager.instance.playerController.GetComponent<Animator>().speed = gameManager.instance.playerController.GetComponent<Animator>().speed / 8;
            aud.PlayOneShot(sfxManager.instance.bulletTimeExit[Random.Range(0, sfxManager.instance.bulletTimeExit.Length)]);
        }
    }

    public void freezeObjects()
    {
        if (Input.GetButtonDown("Ability"))
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit))
            {
                Collider[] objects = Physics.OverlapSphere(hit.point, 5);


                foreach (Collider collider in objects)
                {
                    if (collider.tag != "Player")
                    {
                        if (collider.GetComponent<Rigidbody>() != null)
                        {
                            Debug.Log("Freezing " + collider.name);
                            Rigidbody rb = collider.GetComponent<Rigidbody>();
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else if (!collider.GetComponent<Rigidbody>())
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
                        if (parent.GetComponent<enemySlimeAI>())
                        {
                            parent.GetComponent<enemySlimeAI>().enabled = false;
                        }
                        if (parent.GetComponent<enemyHumanoidSpecimenAI>())
                        {
                            parent.GetComponent<enemyHumanoidSpecimenAI>().enabled = false;
                        }
                        if (parent.GetComponent<enemyScuttlingSpecimenAI>())
                        {
                            parent.GetComponent<enemyScuttlingSpecimenAI>().enabled = false;
                        }
                        if (parent.GetComponent<enemyRCCar>())
                        {
                            parent.GetComponent<enemyRCCar>().enabled = false;
                        }
                        if (parent.GetComponent<NavMeshAgent>())
                        {
                            parent.GetComponent<NavMeshAgent>().enabled = false;
                        }
                        if (parent.GetComponent<enemyTurret>())
                        {
                            parent.GetComponent<enemyTurret>().enabled = false;
                        }
                        StartCoroutine(unfreezeDelay(collider));

                    }
                }
            }
        }
    }

    public IEnumerator unfreezeDelay(Collider collider)
    {
        yield return new WaitForSeconds(10);
        if (collider != null)
        {
            if (collider.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
            }
            Transform parent = collider.transform;
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
            if(parent.GetComponent<enemyTurret>())
            {
                parent.GetComponent<enemyTurret>().enabled = true;
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
