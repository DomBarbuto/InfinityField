using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScuttlingSpecimenProximitySensor : MonoBehaviour
{
    [SerializeField] enemyScuttlingSpecimenAI brain;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            brain.explode();
    }
}
