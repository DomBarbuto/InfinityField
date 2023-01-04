using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRCDetection : MonoBehaviour
{
    [SerializeField] enemyRCCar brain;

    private void OnTriggerEnter(Collider other)
    {
        brain.TriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        brain.TriggerExit(other);
    }
}
