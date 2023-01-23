using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class perkPickUp : MonoBehaviour
{
    perkCreation perk;
    [SerializeField] Perks perkDrop;

    private void Start()
    {
        perk = assignPerk(perkDrop);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            addPerk(gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter]);
            Destroy(gameObject);
        }
    }

    public void addPerk(playerCharacter player)
    {
        foreach(perkList i in gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].perks)
        {
            if(i.perkName == perk.giveName())
            {
                if (i.rarity >= perkList.PerkRarity.legendary)
                {
                    return;
                }
                else
                {
                    i.rarity += 1;
                    return;
                }
            }
        }
        int randRarity = Random.Range(0, 5);
        gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].perks.Add(new perkList(perk, perk.giveName(), perk.giveIcon(), (perkList.PerkRarity)randRarity));
    }

    public perkCreation assignPerk(Perks perkAssign)            //For the perk to be able to be created and dropped, add it into the switch statement
    {
        switch(perkAssign)
        {
            case Perks.criticalStrike:
                return new criticalStrike();
            case Perks.Adrenaline:
                return new Adrenaline();
            case Perks.TimeHealsWounds:
                return new TimeHealsWounds();
            case Perks.Rejuvination:
                return new Rejuvination();
            case Perks.RocketMan:
                return new RocketMan();




            default:
                return new criticalStrike();
        }
    }
}
public enum Perks //List all perks here, do not put the same perk on here with a different rarity, that is handled somewhere else
{
    criticalStrike,
    Adrenaline,
    TimeHealsWounds,
    Rejuvination,
    RocketMan
}