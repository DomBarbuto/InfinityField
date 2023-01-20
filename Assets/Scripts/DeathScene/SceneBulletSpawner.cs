using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBulletSpawner : MonoBehaviour
{
    [SerializeField] float SpawnTime;
    float duration = 0.75f;
    [SerializeField] float StopTime;
    [SerializeField] GameObject bullet;
    bool isSho0t = true;
    // Start is called before the first frame update
    void Start()
    {
        
           
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < StopTime)
        {
            if (SpawnTime < Time.time)
            {
                if(isSho0t)
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire()
    {
        isSho0t = false;
        Instantiate(bullet,transform);
        yield return new WaitForSeconds(duration);
        isSho0t = true;
    }
}
