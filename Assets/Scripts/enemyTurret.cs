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
    bool alerted = false;

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
            if (!alerted)
            {
                sfxManager.instance.aud.PlayOneShot(sfxManager.instance.turretAlert[Random.Range(0, sfxManager.instance.turretAlert.Length)], sfxManager.instance.turretAlertVol);
                alerted = true;
            }
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
        canShoot = false;
        playerInRange = false;
        anim.SetTrigger("TriggerDeath");
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.turretDeath[Random.Range(0, sfxManager.instance.turretDeath.Length)], sfxManager.instance.turretDeathVol);
        StartCoroutine(timeToDestroy());
    }

    public void animEvent_animatorOff()
    {
        anim.enabled = false;
    }

    IEnumerator shoot()
    {
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.turretAttack[Random.Range(0, sfxManager.instance.turretAttack.Length)], sfxManager.instance.turretAttackVol);
        GameObject newProjectile = Instantiate(projectile, muzzlePoint.position, muzzlePoint.rotation, null);
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }

    IEnumerator timeToDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
