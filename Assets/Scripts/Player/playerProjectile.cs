using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage;
    [SerializeField] float projectileLifeTime;
    [SerializeField] bool isPiercing;
    [SerializeField] GameObject secondaryProjectile;
    bool hasHit;


    Rigidbody rb;

    private void Start()
    {
        hasHit = false;
        rb = gameObject.GetComponent<Rigidbody>();

        RaycastHit hit;

        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0;
        direction = direction.normalized;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit))
        {
            direction = (hit.point - gameManager.instance.playerController.currentMuzzlePoint.position).normalized;
        }

        Vector3 force = direction * projectileSpeed;

        rb.AddForce(force, ForceMode.Impulse);

    }
    private void Update()
    {
        projectileLifeTime -= Time.deltaTime * 10;

        if(projectileLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (!hasHit)
        {
            hasHit = true;

            Destroy(gameObject);

            // VFX at impact point
            Instantiate(gameManager.instance.playerController.weaponInventory[gameManager.instance.playerController.currentWeapon].hitFX, gameObject.transform.position, transform.rotation);

            // SFX at impact point
            AudioSource.PlayClipAtPoint(sfxManager.instance.ricochetSound[0], other.transform.position, sfxManager.instance.ricochetSoundVol);

            if (other.GetComponent<IDamage>() != null)
            {

                doDamage(other.GetComponent<IDamage>());
                Debug.Log("collided with " + other.gameObject.name);

                if(secondaryProjectile != null)
                {
                    Instantiate(secondaryProjectile, transform.position, transform.rotation);
                }

            }
            else
            {
                Debug.Log("collided with " + other.gameObject.name);
            }
            Destroy(gameObject);
        }
    }

    private void doDamage(IDamage enemy)
    {
        enemy.takeDamage(projectileDamage);

    }




}
