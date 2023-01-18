using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class proceduralGeneration : MonoBehaviour
{
    [SerializeField] public GameObject[] rooms;
    [SerializeField] public GameObject bossRoom;
    [SerializeField] public int maxRooms = 10;
    [SerializeField] public GameObject startingRoom;
    public GameObject currentRoom;
    private int roomCount;
    private bool bossSpawned = false;
    private List<GameObject> spawnedRooms = new List<GameObject>();
    public Vector3 roomOffset;
    private void Start()
    {
        roomCount = 1;
        currentRoom = startingRoom;

        if(currentRoom.GetComponent<room>().OnEnter != null)
        {
            Debug.Log("checking");
            currentRoom.GetComponent<room>().OnEnter.AddListener(SpawnRoom);
            currentRoom.GetComponent<room>().OnEnter.AddListener(DestroyRoom);
        }
        spawnedRooms.Add(currentRoom);
    }

    public void SpawnRoom()
    {
        if (!bossSpawned)
        {
            if (roomCount >= maxRooms)
            {
                if(bossRoom != null)
                {
                    SpawnBossRoom();
                    bossSpawned = true;
                }
                
                return;
            }
            else
            {
                roomCount++;
                foreach(Transform exit in currentRoom.GetComponent<room>().exits)
                {
                    Transform currExit = exit;
                    int rand = Random.Range(0, rooms.Length);
                    Quaternion newRoomRotation = currExit.transform.rotation;
                    GameObject newRoom = Instantiate(rooms[rand], currExit.transform.position, newRoomRotation);  //Add in the change for roomOffset for the room that is being instantiated
                    newRoom.GetComponent<room>().OnEnter.AddListener(MakeCurrentRoom);
                    newRoom.GetComponent<room>().OnEnter.AddListener(SpawnRoom);
                    newRoom.GetComponent<room>().OnEnter.AddListener(DestroyRoom);
                    BakeNavMesh(newRoom);
                    spawnedRooms.Add(newRoom);
                }
            }
        }
    }
    public void BakeNavMesh(GameObject room)
    {
        Debug.Log("Made to Bake");
        NavMeshSurface navMeshSurface = room.GetComponent<room>().nav;
        if (navMeshSurface && navMeshSurface.gameObject.tag == "Ground")
        {
            Debug.Log("Made Surface");
            navMeshSurface.BuildNavMesh();
        }
    }
    public void MakeCurrentRoom()
    {
        currentRoom = this.gameObject;
    }

    private void SpawnBossRoom()
    {
        foreach (Transform exit in currentRoom.GetComponent<room>().exits)
        {
            Transform currExit = exit;
            Vector3 newRoomPosition = currExit.transform.position;
            GameObject newBossRoom = Instantiate(bossRoom, newRoomPosition, exit.rotation);
        }
    }
    public void DestroyRoom()
    {
        //Destroy all previous rooms that haven't been entered
        foreach (GameObject room in spawnedRooms)
        {
            if (room != currentRoom)
                Destroy(room);
        }
        //Clear the list of spawned rooms
        spawnedRooms.Clear();
        //Add the current room to the list
        spawnedRooms.Add(currentRoom);
    }
}