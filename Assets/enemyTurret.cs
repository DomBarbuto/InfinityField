using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTurret : MonoBehaviour
{
    [SerializeField] GameObject GunObj;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] GameObject projectile;
    [SerializeField] Animator anim;
    [SerializeField] int HP;
    public bool deployed;

    bool playerInRange;

    //temp variables for coroutine
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && deployed)
        {
            GunObj.transform.LookAt(gameManager.instance.player.transform);
            if (canShoot)
            {
                canShoot = false;
                StartCoroutine(shoot());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
            death();
    }

    void death()
    {

    }

    public void isDeployed()
    {
        deployed = true;
    }

    ////for animation event
    //public void shoot()
    //{
    //    GameObject newProjectile = Instantiate(projectile, muzzlePoint, false);
    //    newProjectile.transform.SetParent(null);
    //}

    IEnumerator shoot()
    {
        GameObject newProjectile = Instantiate(projectile, muzzlePoint, false);
        newProjectile.transform.SetParent(null);
        yield return new WaitForSeconds(1);
        canShoot = true;
    }
}
