using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntryListener : MonoBehaviour, IRoomEntryListener 
{
    public void notify()
    {
        Debug.LogError(gameObject.name);
    }
}
