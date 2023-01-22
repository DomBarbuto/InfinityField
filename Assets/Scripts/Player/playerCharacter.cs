using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] public float HP;
    [SerializeField] public float HPMax;
    [Range(3,8)][SerializeField] public float speed;
    [SerializeField] public float energy;
    [SerializeField] public float energyMax;
    [SerializeField] public float energyUseRate;
    [SerializeField] public bool rechargable;
    [SerializeField] public float criticalChance;
    [SerializeField] public float passiveTickRate;           //This value should always be default 1;
    [SerializeField] public List<perkList> perks = new List<perkList>();
    [SerializeField] public Material material;
    public bool isUsingAbility = false;
    public float currSpeed;
    [SerializeField] public abilityList ability;
    bool isRunning = false;

    public void Start()
    {
        StartCoroutine(callPerkOnUpdate());

    }
    public IEnumerator callPerkOnUpdate()
    {
        if (!isRunning)
        {
            isRunning = true;
            if (this.HP > 0)
            {
                foreach (perkList _perk in perks)
                {
                    _perk.perk.update(gameManager.instance.playerController, _perk.rarity);
                }
                yield return new WaitForSeconds(1);
                isRunning = false;
                StartCoroutine(callPerkOnUpdate());
                
            }
        }
    }

    public void callIPerkOnHit(IDamage enemy)
    {
        foreach(perkList _perk in perks)
        {
            _perk.perk.onHit(gameManager.instance.playerController, enemy, _perk.rarity);
        }
    }

    public void callPerkOnDeathEnemy(IDamage enemy)
    {
        foreach (perkList _perk in perks)
        {
            _perk.perk.onDeathEnemy(gameManager.instance.playerController, enemy, _perk.rarity);
        }
        gameManager.instance.updatePlayerEnergyBar();
    }

    public void callPerkOnUseAbility()
    {
        foreach (perkList _perk in perks)
        {
            _perk.perk.onUseAbility(gameManager.instance.playerController, _perk.rarity);
        }
    }

    public void callIPerkOnJump()
    {
        Debug.Log(perks.Count);
        foreach (perkList _perk in this.perks)
        {
            _perk.perk.onJump(gameManager.instance.playerController, _perk.rarity);
        }
    }

    public enum abilityList
    {
        sprint,
        bulletTime,
        freeze
    }
}
