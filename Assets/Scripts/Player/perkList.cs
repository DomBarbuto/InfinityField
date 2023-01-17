using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class perkList : MonoBehaviour
{

    public perkCreation perk;
    public string perkName;
    public enum PerkRarity {common, uncommon, rare, epic, legendary};
    public PerkRarity rarity;

    public perkList(perkCreation _perk, string _perkName, PerkRarity _rarity)
    {
        perk = _perk;
        perkName = _perkName;
        rarity = _rarity;
    }

}
