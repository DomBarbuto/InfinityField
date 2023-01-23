using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] int maxEnemies;
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
        List<GameObject> copyList = enemies;
        /*foreach (GameObject baddie in enemies)
        {
            if (baddie == null)
                copyList.Remove(baddie);
        }*/

        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
                copyList.Remove(enemies[i]);
        }
        enemies = copyList;
        if (startContinuous && canSpawn && enemies.Count < maxEnemies)
        {
            canSpawn = false;
            StartCoroutine(continuousSpawn());
        }
    }

    public void triggerSpawn()
    {
        if (enemies.Count < maxEnemies)
        enemies.Add(Instantiate(enemy, transform));
    }

    public void destroyEnemies()
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
