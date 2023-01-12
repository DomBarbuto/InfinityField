using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScuttlingSpecimenAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int HP;
    [SerializeField] GameObject plume;

    bool playerInRange;
    int HPMAX;
    // Start is called before the first frame update
    void Start()
    {
        HPMAX = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            movement();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }

    void movement()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    public void explode()
    {
        GameObject newExplosion = Instantiate(plume, transform.position, transform.rotation);
        newExplosion.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            explode();
        }
    }
}
