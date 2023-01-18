using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] List<GameObject> listeners;
    [SerializeField] bool hasEntered;


    private void OnTriggerEnter(Collider other)
    {
        if(!hasEntered)
        {
            if(other.tag == "Player")
            {
                 hasEntered = true;
                playerEnterRoom();
            }
           
        }
    }

    public void playerEnterRoom()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].GetComponent<IRoomEntryListener>().notify();
        }
    }

}
