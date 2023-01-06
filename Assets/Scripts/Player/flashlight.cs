using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    [SerializeField] GameObject spotlight;
    [SerializeField] bool isTurnedOn;

    private void Update()
    {
        if (Input.GetButtonDown("FlashLight"))
        {
            isTurnedOn = !isTurnedOn;
            spotlight.SetActive(isTurnedOn);
        }
    }
}
