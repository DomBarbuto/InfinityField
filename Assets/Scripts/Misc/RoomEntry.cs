using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] List<RoomEntryListener> listeners;
    [SerializeField] bool hasEntered;


    private void OnTriggerEnter(Collider other)
    {
        if(!hasEntered)
        {
            hasEntered = true;
            playerEnterRoom();
        }
    }

    public void playerEnterRoom()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].notify();
        }
    }

}
