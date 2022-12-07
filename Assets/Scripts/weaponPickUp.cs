using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickUp : MonoBehaviour
{
    [SerializeField] weaponCreation gun;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerController.weaponPickUp(gun);
            Destroy(gameObject);
        }
    }
}