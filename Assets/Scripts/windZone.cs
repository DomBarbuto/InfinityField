using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int pushBack;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            gameManager.instance.playerController.pushBackInput(transform.forward * pushBack);
    }
}
