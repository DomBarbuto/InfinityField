using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBulletSpawner : MonoBehaviour
{
    [SerializeField] float SpawnTime;
    float duration = 0.75f;
    [SerializeField] float StopTime;
    [SerializeField] GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        while (Time.time < StopTime)
            if(SpawnTime<Time.time)
            StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fire()
    {
        Instantiate(bullet);
        yield return new WaitForSeconds(duration);
    }
}
