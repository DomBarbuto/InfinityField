using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelExit : MonoBehaviour
{
    [SerializeField] int exitCost;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.credits >= exitCost)
        {
            gameManager.instance.pause();
            gameManager.instance.SetActiveMenu(gameManager.UIMENUS.winMenu);
        }
    }
}
