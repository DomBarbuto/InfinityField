using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBack : MonoBehaviour
{
    [SerializeField] int pushBackScalar;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerDir = (gameManager.instance.player.transform.position - transform.position).normalized;
            playerDir.y = playerDir.y * 2;
            playerDir.z = playerDir.z * 3;
            gameManager.instance.playerController.pushBackInput(playerDir * pushBackScalar);
        }
    }
}
