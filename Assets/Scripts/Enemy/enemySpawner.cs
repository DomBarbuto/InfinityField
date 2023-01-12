using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] int maxEnemies;
    [SerializeField] int enemyCount;
    [SerializeField] float cooldown;
    [SerializeField] GameObject enemy;
    public bool startContinuous;
    bool canSpawn = true;
    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startContinuous && canSpawn)
        {
            canSpawn = false;
            StartCoroutine(continuousSpawn());
        }
    }

    public void triggerSpawn()
    {
        enemies.Add(Instantiate(enemy, transform));
    }

    public void destroyEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public void setContinuous(bool val)
    {
        startContinuous = val;
    }

    IEnumerator continuousSpawn()
    {
        enemies.Add(Instantiate(enemy, transform, false));
        yield return new WaitForSeconds(cooldown);
        canSpawn = true;
    }
}
