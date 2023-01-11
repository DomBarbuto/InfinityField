using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerController))]
public class PlayerAnimController : MonoBehaviour
{
    [SerializeField] int animTransSpeed;
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // This turns on the players default animation state. 
        // PlayerControllers currentWeaponType should be set to unequipped at startup
        turnOnAnimationState(gameManager.instance.playerController.currentWeaponType);
    }

    private void Update()
    {
        // Animation speed dependent on players move vector
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), gameManager.instance.playerController.move.magnitude, Time.deltaTime * animTransSpeed));
    }

    public void switchAnimState(weaponCreation.WeaponType currentWeaponType, weaponCreation.WeaponType newWeaponType)
    {
        // Turn OFF the current anim state 
        switch (currentWeaponType)
        {
            case weaponCreation.WeaponType.Pistol:
                anim.SetBool("HasPistol", false);
                break;

            case weaponCreation.WeaponType.Rifle:
            case weaponCreation.WeaponType.RailGun:
                anim.SetBool("HasRifleOrRailGun", false);
                break;

            case weaponCreation.WeaponType.GrenadeLauncher:
            case weaponCreation.WeaponType.ArcGun:
                anim.SetBool("HasGrenadeLauncherOrArcGun", false);
                break;

            case weaponCreation.WeaponType.Unequipped:
                anim.SetBool("HasUnequipped", false);
                break;
            default:
                break;
        }

        // Turn ON new weapon animation state
        turnOnAnimationState(newWeaponType);
    }

    private void turnOnAnimationState(weaponCreation.WeaponType newWeaponType)
    {
        // Turn on new anim state
        switch (newWeaponType)
        {
            case weaponCreation.WeaponType.Pistol:
                anim.SetBool("HasPistol", true);
                break;

            case weaponCreation.WeaponType.Rifle:
            case weaponCreation.WeaponType.RailGun:
                anim.SetBool("HasRifleOrRailGun", true);
                break;

            case weaponCreation.WeaponType.GrenadeLauncher:
            case weaponCreation.WeaponType.ArcGun:
                anim.SetBool("HasGrenadeLauncherOrArcGun", true);
                break;

            case weaponCreation.WeaponType.Unequipped:
                anim.SetBool("HasUnequipped", true);
                break;
            default:
                break;
        }
    }

    //Turn on is true. Turn off is false.
    public void switchSprintingState(bool value)
    {
        anim.SetBool("IsSprinting", value);
    }

    public void shootTrigger()
    {
        anim.SetTrigger("Shoot");
    }

    public void reloadTrigger()
    {
        anim.SetTrigger("Reload");
    }

    public void animEvent_Reload()
    {
        gameManager.instance.playerController.reload();
    }

}
