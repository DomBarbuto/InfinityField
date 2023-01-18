using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> perks = new List<GameObject>();
    private void Start()
    {
        int rand = Random.Range(0, perks.Count - 1);

        Vector3 spawnPosition = transform.position + new Vector3(0f, 1f, 0f);

        Instantiate(perks[rand], spawnPosition, transform.rotation);
    }
}
