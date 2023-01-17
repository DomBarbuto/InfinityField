using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class room : MonoBehaviour, IRoomEntryListener
{
    [SerializeField] public Transform entrance;
    [SerializeField] public List<Transform> exits = new List<Transform>();
    [SerializeField] public GameObject roomEntry;
    [SerializeField] public GameObject parentGeneration;
    [SerializeField] public proceduralGeneration Generation;
    [SerializeField] public bool testing = false;

    public UnityEvent OnEnter;

    private void Start()
    {
        OnEnter = new UnityEvent();

        if(parentGeneration.GetComponent<proceduralGeneration>() != null)
        {
            Generation = parentGeneration.GetComponent<proceduralGeneration>();
        }

    }

    private void Update()
    {
        if (testing)
        {
            if (Generation.GetComponent<proceduralGeneration>() != null)
            {
                testing = false;
                Generation.currentRoom = this.gameObject;
                Generation.SpawnRoom();
            }
        }

    }

    public void notify()
    {
        OnEnter.Invoke();
    }
}