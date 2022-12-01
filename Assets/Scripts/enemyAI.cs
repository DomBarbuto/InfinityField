using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---- External Components ----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Material damageFX;
    

    [Header("---- Enemy Stats ----")]
    [SerializeField] int HP;

    [SerializeField] float damageFXLength;

    [SerializeField] int rotSpeed;
    [SerializeField] int lookAngle;

    [SerializeField] Transform headPos;

    [Header("---- Weapon Stats ----")]
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate;
    [SerializeField] Transform muzzlePoint;

    //Private Variables------------------
    int MAXHP;
    int creditsHeld; //How many credits the enemy drops

    bool isShooting;
    bool playerInRange;

    Vector3 playerDir;

    float angleToPlayer;
    //-----------------------------------

        //------- Functions --------

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Player Detection-------------------

    void canSeePlayer() { }

    void facePlayer() { }

    //Enemy Statitistic Modification-----

    public void takeDamage(int dmg)
    {

    }

    //Coroutines-------------------------

    IEnumerator flashDamageFX()
    {
        yield return new WaitForSeconds(damageFXLength);
    }

    IEnumerator fire()
    {
        yield return new WaitForSeconds(fireRate);
    }

    //Trigger----------------------------

    public void OnTriggerEnter(Collider other)
    {
    
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}
