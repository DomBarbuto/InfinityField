using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossForceField : MonoBehaviour
{
    [SerializeField] public GameObject hitEffectVFX;
    [SerializeField] Material damageForceFieldMat;
    [SerializeField] float damageFXLength;
    private Renderer model;
    private Color origColor;

    private void Start()
    {
        model = GetComponent<MeshRenderer>();
        origColor = model.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<playerProjectile>())
        {
            // Play SFX

            // DamageFX
            StartCoroutine(damageFX());

            // VFX impact
            GameObject vfx = Instantiate(hitEffectVFX, transform.position + (gameManager.instance.player.transform.position - transform.position).normalized, Quaternion.Inverse(other.transform.rotation));
            Destroy(vfx, 2);
        }
    }

    
    IEnumerator damageFX()
    {
        model.material.color = damageForceFieldMat.color;
        yield return new WaitForSeconds(damageFXLength);
        model.material.color = origColor;
    }
}
