using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntryListener : MonoBehaviour, IRoomEntryListener 
{
    [SerializeField] BossSlimeInfusedMech brain;

    public void notify()
    {
        brain.notify();
    }
}
