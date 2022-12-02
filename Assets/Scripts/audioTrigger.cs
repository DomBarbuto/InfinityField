using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTrigger : MonoBehaviour
{
    [Header("---- Audio Trigger Settings ----")]
    [SerializeField] int trackID; //index of track to play

    public void OnTriggerEnter(Collider other)
    {
        gameManager.instance.composer.setTrack(trackID);
    }
}
