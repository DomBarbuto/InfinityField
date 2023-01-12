using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnSystem : MonoBehaviour, IRoomEntryListener
{
    public List<enemySpawner> spawners;
    [SerializeField] bool continuousOn;

    // Start is called before the first frame update
    void Start()
    {
        if (continuousOn)
            setAllContinuous(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setAllContinuous(bool val)
    {
        foreach (enemySpawner spawner in spawners)
        {
            spawner.setContinuous(val);
        }
    }

    public void notify()
    {
        foreach (enemySpawner spawner in spawners)
        {
            spawner.triggerSpawn();
        }
    }

}