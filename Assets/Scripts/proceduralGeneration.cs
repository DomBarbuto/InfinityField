using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject bossRoomPrefab;
    public int maxRooms = 10;
    public int maxEnemiesPerRoom = 3;
    public float branchProbability = 0.5f;
    public int maxBranches = 3;
    public int roomsBeforeBoss = 5;
    public bool bossHasSpawned = false;
    public float roomPadding = 1.0f;
    public bool checkClipping = true;
    public Vector3 roomScale = new Vector3(10, 10, 10);

    private List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private int roomCount = 0;

    void Start()
    {
        GenerateDungeon(Vector3.zero);
    }

    void GenerateDungeon(Vector3 position)
    {
        if (roomCount >= maxRooms && bossHasSpawned)
        {
            return;
        }

        GameObject room;
        Vector3 spawnPos = FindSpawnPosition(position);
        if (spawnPos == Vector3.negativeInfinity)
        {
            return;
        }

        if (roomCount < roomsBeforeBoss && Random.value > 0.5f)
        {
            room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], spawnPos, Quaternion.identity);
        }
        else
        {
            room = Instantiate(bossRoomPrefab, spawnPos, Quaternion.identity);
        }
        rooms.Add(room);
        roomCount++;
        GenerateEnemies(room);
        GenerateDoors(room);
    }

    Vector3 FindSpawnPosition(Vector3 position)
    {
        int iterations = 0;
        int maxIterations = 10;
        while (IsClipping(position) && iterations < maxIterations)
        {
            position += new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            iterations++;
        }
        if (iterations >= maxIterations)
        {
            return Vector3.negativeInfinity;
        }
        return position;
    }
    void GenerateEnemies(GameObject room)
    {
        int enemyCount = Random.Range(0, maxEnemiesPerRoom);
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 enemyPos = new Vector3(Random.Range(-roomScale.x / 2, roomScale.x / 2), 0, Random.Range(-roomScale.z / 2, roomScale.z / 2));
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], room.transform.position + enemyPos, Quaternion.identity);
            enemies.Add(enemy);
        }
    }

    void GenerateDoors(GameObject room)
    {
        int doorCount = Random.Range(1, maxBranches + 1);
        for (int i = 0; i < doorCount; i++)
        {
            if (Random.value < branchProbability)
            {
                Transform door = room.transform.GetChild(i);
                Vector3 nextPosition = door.position + door.forward * roomPadding;
                GenerateDungeon(nextPosition);
            }
        }
    }

    bool IsClipping(Vector3 position)
    {
        Vector3 extents = roomScale / 2;
        Collider[] colliders = Physics.OverlapBox(position, extents, Quaternion.identity, ~0);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.CompareTag("Room"))
            {
                return true;
            }
        }
        return false;
    }
}


