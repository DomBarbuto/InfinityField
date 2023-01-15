using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class electricArc : MonoBehaviour
{

    public GameObject[] enemies;
    [SerializeField] public float arcSpeed;
    [SerializeField] public float distance;
    [SerializeField] public float radius;
    [SerializeField] public int damage;

    private int currentEnemy = 0;
    private Vector3 targetPos;
    public AnimationCurve poistionCurve;
    private List<GameObject> enemyList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider thing in colliders)
        {
            if (thing.GetComponent<enemyHitDetection>() != null)
            {
                if (thing.GetComponent<enemyHitDetection>().group == enemyDamageHandler.DamageGroup.Head)
                {
                    float tempDist = Vector3.Distance(thing.transform.position, transform.position);
                    if(tempDist > 1)
                    {
                        enemyList.Add(thing.gameObject);
                    }
                    
                }
            }
        }
        if (enemyList.Count > 0)
        {
            enemies = enemyList.ToArray();
            

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies[currentEnemy] != null)
        {
            targetPos = enemies[currentEnemy].transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, arcSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) <= 0.2f)
            {
                if (enemies.Length > 0)
                {
                    if (currentEnemy < enemies.Length - 1)
                    {
                        enemies[currentEnemy].GetComponent<IDamage>().takeDamage(damage);
                        currentEnemy++;

                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }


}
