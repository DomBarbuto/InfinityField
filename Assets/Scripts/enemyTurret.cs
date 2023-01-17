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
    [SerializeField] float shootSpeed;

    public bool deployed;
    bool playerInRange;

    //temp variables for coroutine
    bool canShoot = true;

    private void Start()
    {
        anim.SetBool("IsIdle", true);
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
            deployed = true;
            anim.SetBool("IsIdle", false);
            anim.SetTrigger("TriggerEngage");

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
        anim.enabled = true;
        anim.SetTrigger("TriggerDeath");
    }

    public void animEvent_animatorOff()
    {
        anim.enabled = false;
    }

    IEnumerator shoot()
    {
        GameObject newProjectile = Instantiate(projectile, muzzlePoint.position, muzzlePoint.rotation, null);
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }
}
