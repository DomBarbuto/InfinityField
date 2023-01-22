using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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
    [SerializeField] public NavMeshSurface nav;

    public UnityEvent OnEnter;

    private void Start()
    {
        OnEnter = new UnityEvent();

        if (parentGeneration.GetComponent<proceduralGeneration>() != null)
        {
            Generation = parentGeneration.GetComponent<proceduralGeneration>();
        }
        if (gameObject.GetComponentInChildren<NavMeshSurface>() != null)
        {
            nav = gameObject.GetComponentInChildren<NavMeshSurface>();
            BakeNavMesh();
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

    public void BakeNavMesh()
    {
        Debug.Log("Made to Bake");

        nav.BuildNavMesh();

    }

    public void notify()
    {
        OnEnter.Invoke();
        if (Generation.GetComponent<proceduralGeneration>() != null)
        {
            testing = false;
            Generation.currentRoom = this.gameObject;
            if (Generation.currentRoom != Generation.startingRoom)
            {
                Generation.DestroyRoom();
            }
            Generation.SpawnRoom();

        }
    }
}
