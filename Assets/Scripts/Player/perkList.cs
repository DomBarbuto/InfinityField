using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class perkList
{

    public perkCreation perk;
    public string perkName;
    public Sprite icon;
    public enum PerkRarity {common, uncommon, rare, epic, legendary};
    public PerkRarity rarity;

    public perkList(perkCreation _perk, string _perkName,Sprite _icon, PerkRarity _rarity)
    {
        perk = _perk;
        perkName = _perkName;
        icon = _icon;
        rarity = _rarity;
    }

}
