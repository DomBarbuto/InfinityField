using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickUp : MonoBehaviour
{
    [SerializeField] weaponCreation gun;
    [SerializeField] bool hasBeenPickedUp;

    public void OnTriggerEnter(Collider other)
    {
        if (!hasBeenPickedUp && other.CompareTag("Player"))
        {
            hasBeenPickedUp = true;
            gameManager.instance.playerController.weaponPickUp(gun);
            Destroy(gameObject);
        }
    }
}