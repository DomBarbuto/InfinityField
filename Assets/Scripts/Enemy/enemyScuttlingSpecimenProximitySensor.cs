using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScuttlingSpecimenProximitySensor : MonoBehaviour
{
    [SerializeField] enemyScuttlingSpecimenAI brain;
    [SerializeField] bool hasTriggered;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            brain.triggerExplode();
        }
    }
}
