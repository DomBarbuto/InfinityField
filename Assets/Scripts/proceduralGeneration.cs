/*using UnityEngine;
using System.Collections.Generic;

public class proceduralGeneration : MonoBehaviour
{
    [SerializeField] public GameObject[] rooms;
    [SerializeField] public GameObject bossRoom;
    [SerializeField] public int maxRooms = 10;
    [SerializeField] public GameObject startingRoom;
    private GameObject currentRoom;
    private int roomCount;
    private bool bossSpawned = false;
    private List<GameObject> spawnedRooms = new List<GameObject>();
    public Vector3 roomOffset;
    private void Start()
    {
        roomCount = 1;
        currentRoom = Instantiate(startingRoom, transform.position, Quaternion.identity);
        currentRoom.GetComponent<Room>().OnExit.AddListener(SpawnRoom);
        currentRoom.GetComponent<Room>().OnExit.AddListener(DestroyRoom);
        currentRoom.GetComponent<Room>().OnEnter.AddListener(MakeCurrentRoom);
        spawnedRooms.Add(currentRoom);
    }

    private void SpawnRoom()
    {
        if (!bossSpawned)
        {
            if (roomCount >= maxRooms)
            {
                SpawnBossRoom();
                bossSpawned = true;
                return;
            }
            else
            {
                roomCount++;
                GameObject[] exits = currentRoom.GetComponent<Room>().exits;
                for (int i = 0; i < exits.Length; i++)
                {
                    GameObject exit = exits[i];
                    int rand = Random.Range(0, rooms.Length);
                    GameObject newRoom = Instantiate(rooms[rand], exit.transform.position + roomOffset, Quaternion.identity);
                    newRoom.GetComponent<Room>().OnExit.AddListener(SpawnRoom);
                    newRoom.GetComponent<Room>().OnEnter.AddListener(MakeCurrentRoom);
                    newRoom.GetComponent<Room>().OnEnter.AddListener(DestroyRoom);
                    Vector3 newEntrance = newRoom.GetComponent<Room>().entrance.transform.position;
                    Vector3 exitPos = exit.transform.position;
                    Quaternion newRoomRotation = Quaternion.FromToRotation(newEntrance - exitPos, exitPos - newEntrance);
                    newRoom.transform.rotation = newRoomRotation;
                    spawnedRooms.Add(newRoom);
                }
            }
        }
    }
    private void MakeCurrentRoom()
    {
        currentRoom = this.gameObject;
    }

    private void SpawnBossRoom()
    {
        GameObject[] exits = currentRoom.GetComponent<Room>().exits;
        for (int i = 0; i < exits.Length; i++)
        {
            GameObject exit = exits[i];
            Vector3 newRoomPosition = exit.transform.position + roomOffset;
            GameObject newBossRoom = Instantiate(bossRoom, newRoomPosition, Quaternion.identity);
        }
    }
    private void DestroyRoom()
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
}*/