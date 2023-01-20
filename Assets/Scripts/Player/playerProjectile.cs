using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage;
    [SerializeField] float projectileLifeTime;
    [SerializeField] bool isPiercing;
    [SerializeField] GameObject secondaryProjectile;
    bool hasHit;
    RaycastHit hit;


    Rigidbody rb;

    private void Start()
    {
        hasHit = false;
        rb = gameObject.GetComponent<Rigidbody>();

        Vector3 direction = Camera.main.transform.forward;
        direction = direction.normalized;

        LayerMask rayMask = ~LayerMask.GetMask("Explosion");

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, Mathf.Infinity, rayMask))
        {
            Debug.Log("Ray hit the collider of " + hit.collider.name);
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
            Instantiate(gameManager.instance.playerController.weaponInventory[gameManager.instance.playerController.currentWeapon].hitFX, hit.point, transform.rotation);
            // SFX at impact point
            AudioSource.PlayClipAtPoint(sfxManager.instance.ricochetSound[0], other.transform.position, sfxManager.instance.ricochetSoundVolMulti);

            if (other.GetComponent<IDamage>() != null)
            {

                doDamage(other.GetComponent<IDamage>());
                Debug.Log("collided with " + other.gameObject.name);

                if (secondaryProjectile != null)
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

    public void doDamage(IDamage enemy)
    {
        enemy.takeDamage(projectileDamage);
        gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].callIPerkOnHit(enemy);

    }




}
