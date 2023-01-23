using UnityEngine;

[System.Serializable]
public abstract class perkCreation
{

    public abstract string giveName();
    public abstract Sprite giveIcon();
    public virtual void update(playerController player, perkList.PerkRarity rarity)
    {

    }
    public virtual void onHit(playerController player, IDamage enemy, perkList.PerkRarity rarity)
    {

    }
    public virtual void onDeathEnemy(playerController player, IDamage enemy, perkList.PerkRarity rarity)
    {

    }
    public virtual void onUseAbility(playerController player, perkList.PerkRarity rarity)
    {

    }
    public virtual void onJump(playerController player, perkList.PerkRarity rarity)
    {

    }
}

//Chance to deal double damage
public class criticalStrike : perkCreation
{
    public override string giveName()
    {
        return "Precision Synchronizer";
    }
    public override Sprite giveIcon()
    {
        Sprite icon = (Sprite)Resources.Load("PerkIcons/CriticalStrikeSprite", typeof(Sprite));
        return icon;
    }

    public override void onHit(playerController player, IDamage enemy, perkList.PerkRarity rarity)
    {
        float random = Random.Range(0, 100);
        float chance = 5 + (7 * (int)rarity) + player.characterList[player.currCharacter].criticalChance;
        if (random < chance)
        {
            gameManager.instance.playerController.weaponInventory[gameManager.instance.playerController.currentWeapon].weaponProjectile.doDamage(enemy);
        }
    }
}

//Kill an enemy and gain back energy
public class Adrenaline : perkCreation
{
    public override string giveName()
    {
        return "Adrenaline";
    }
    public override Sprite giveIcon()
    {
        Sprite icon = (Sprite)Resources.Load("PerkIcons/adrenaline", typeof(Sprite));
        return icon;
    }

    public override void onDeathEnemy(playerController player, IDamage enemy, perkList.PerkRarity rarity)
    {
        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energy < gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energyMax)
        {
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energy += 3 + (5 * (int)rarity);
            if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energy > gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energyMax)
            {
                gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energy = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].energyMax;
            }
        }
        gameManager.instance.updatePlayerEnergyBar();
    }
}

public class TimeHealsWounds : perkCreation

{
    public override string giveName()
    {
        return "Time Heals Wounds";
    }
    public override Sprite giveIcon()
    {
        Sprite icon = (Sprite)Resources.Load("PerkIcons/TimeHealsWound", typeof(Sprite));
        return icon;
    }

    public override void update(playerController player, perkList.PerkRarity rarity)
    {
        Debug.Log("Healing player");
        player.characterList[player.currCharacter].HP += 0.2f + (0.5f * (int)rarity);
        if (player.characterList[player.currCharacter].HP > player.characterList[player.currCharacter].HPMax)
        {
            player.characterList[player.currCharacter].HP = player.characterList[player.currCharacter].HPMax;
        }
    }
}

public class Rejuvination : perkCreation
{
    public override string giveName()
    {
        return "Rejuvination";
    }
    public override Sprite giveIcon()
    {
        Sprite icon = (Sprite)Resources.Load("PerkIcons/Rejuvination", typeof(Sprite));
        return icon;
    }

    public override void onUseAbility(playerController player, perkList.PerkRarity rarity)
    {
        gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].HP += 15f + (10f * (int)rarity);
        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].HP > gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].HPMax)
        {
            gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].HP = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].HPMax;
        }

    }
}

public class RocketMan : perkCreation
{
    GameObject explosionObject;
    float internalCooldown;
    public override string giveName()
    {
        return "Rocket Man";
    }
    public override Sprite giveIcon()
    {
        Sprite icon = (Sprite)Resources.Load("PerkIcons/RocketManSprite", typeof(Sprite));
        return icon;
    }

    public override void update(playerController player, perkList.PerkRarity rarity)
    {
        internalCooldown -= 1;
        Debug.Log(internalCooldown);
    }
    public override void onJump(playerController player, perkList.PerkRarity rarity)
    {
        if (internalCooldown <= 0)
        {
            if (explosionObject == null)
            {
                Vector3 explosion = gameManager.instance.playerController.transform.position;
                explosion.y = explosion.y - 0.5f;
                explosionObject = (GameObject)Resources.Load("PerkEffects/RocketManExplosion", typeof(GameObject));
                if (explosionObject != null)
                {
                    GameObject rocketManExplostion = GameObject.Instantiate(explosionObject, explosion, gameManager.instance.playerController.transform.rotation);
                }
            }
            internalCooldown = 10 - (1.5f * (int)rarity);
        }
    }
}