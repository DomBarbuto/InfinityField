using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] List<RoomEntryListener> listeners;

    public void playerEnterRoom()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].notify();
        }
    }

}
