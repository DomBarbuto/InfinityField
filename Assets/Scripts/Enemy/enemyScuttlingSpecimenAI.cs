using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyScuttlingSpecimenAI : MonoBehaviour
{
    [Header("----- External Components -----")]
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Stats -----")]
    [SerializeField] int HP;
    [SerializeField] GameObject plume;
    [SerializeField] int animTransSpeed;
    
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
        // Animation - set blend tree speed
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed));

        // Move
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(playSteps());
    }

    IEnumerator playSteps()
    {
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.explodingSpecimenMovement[Random.Range(0, sfxManager.instance.explodingSpecimenMovement.Length)], sfxManager.instance.explodingSpecimenMovementVolMulti);
        yield return new WaitForSeconds(0.5f);
    }

    // This is triggered to cause the death animation. 
    // Explosion occurs via animation event.
    public void triggerExplode()
    {
        // Update animation, leading to animation event
        anim.SetTrigger("Explode");
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.explodingSpecimenHiss[Random.Range(0, sfxManager.instance.explodingSpecimenHiss.Length)], sfxManager.instance.explodingSpecimenHissVolMulti);
        // Switch to animation event
        /*GameObject newExplosion = Instantiate(plume, transform.position, transform.rotation);
        newExplosion.transform.SetParent(null);
        Destroy(gameObject);*/
    }

    public void animEvent_Explode()
    {
        Debug.Log("Anim Event");
        GameObject newExplosion = Instantiate(plume, transform.position, transform.rotation);
        newExplosion.transform.SetParent(null);
        Destroy(gameObject);
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.explodingSpecimenExplode[Random.Range(0, sfxManager.instance.explodingSpecimenExplode.Length)], sfxManager.instance.explodingSpecimenExplodeVolMulti);
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            animEvent_Explode();
        }
    }
}
