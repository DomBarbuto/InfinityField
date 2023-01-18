using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class sfxManager : MonoBehaviour
{
    public static sfxManager instance;

    [Header("---------- Components ---------")]
    [SerializeField] public AudioSource aud;
    [SerializeField] public Slider sfxVolumeSlider;

    [Header("---------- Player Audio ----------")]
    [SerializeField] public AudioClip[] playerHurt;
    [Range(0, 1)][SerializeField] public float playerHurtVol;
    [SerializeField] public AudioClip[] playerJump;
    [Range(0, 1)][SerializeField] public float playerJumpVol;
    [SerializeField] public AudioClip[] playerFootstep;
    [Range(0, 1)][SerializeField] public float playerFootstepVol;
    [SerializeField] public AudioClip[] playerSlimedFootstep;
    [Range(0, 1)][SerializeField] public float playerSlimedFootstepVol;
    [SerializeField] public AudioClip[] ricochetSound;
    [Range(0, 1)][SerializeField] public float ricochetSoundVol;

    [Header("---------- RedCC Audio ----------")]
    [SerializeField] public AudioClip[] redCCAlert;
    [Range(0, 1)][SerializeField] public float redCCAlertVol;
    [SerializeField] public AudioClip[] redCCHurt;
    [Range(0, 1)][SerializeField] public float redCCHurtVol;

    [Header("---------- YellowCC Audio ----------")]
    [SerializeField] public AudioClip[] yellowCCAlert;
    [Range(0, 1)][SerializeField] public float yellowCCAlertVol;
    [SerializeField] public AudioClip[] yellowCCHurt;
    [Range(0, 1)][SerializeField] public float yellowCCHurtVol;
    
    [Header("---------- WhiteCC Audio ----------")]
    [SerializeField] public AudioClip[] whiteCCAlert;
    [Range(0, 1)][SerializeField] public float whiteCCAlertVol;
    [SerializeField] public AudioClip[] whiteCCHurt;
    [Range(0, 1)][SerializeField] public float whiteCCHurtVol;

    [Header("---------- BlackCC Audio ----------")]
    [SerializeField] public AudioClip[] blackCCAlert;
    [Range(0, 1)][SerializeField] public float blackCCAlertVol;
    [SerializeField] public AudioClip[] blackCCHurt;
    [Range(0, 1)][SerializeField] public float blackCCHurtVol;

    [Header("---------- Humanoid Specimen Audio ----------")]
    [SerializeField] public AudioClip[] humanoidSpecimenAlert;
    [Range(0, 1)][SerializeField] public float humanoidSpecimenAlertVol;
    [SerializeField] public AudioClip[] humanoidSpecimenAttack;
    [Range(0, 1)][SerializeField] public float humanoidSpecimenAttackVol;
    [SerializeField] public AudioClip[] humanoidSpecimenHurt;
    [Range(0, 1)][SerializeField] public float humanoidSpecimenHurtVol;

    [Header("---------- Exploding Specimen Audio ----------")]
    [SerializeField] public AudioClip[] explodingSpecimenHiss;
    [Range(0, 1)][SerializeField] public float explodingSpecimenHissVol;
    [SerializeField] public AudioClip[] explodingSpecimenMovement;
    [Range(0, 1)][SerializeField] public float explodingSpecimenMovementVol;
    [SerializeField] public AudioClip[] explodingSpecimenExplode;
    [Range(0, 1)][SerializeField] public float explodingSpecimenExplodeVol;

    [Header("---------- Slime Audio ----------")]
    [SerializeField] public AudioClip[] slimeAlert;
    [Range(0, 1)][SerializeField] public float slimeAlertVol;
    [SerializeField] public AudioClip[] slimeAttack;
    [Range(0, 1)][SerializeField] public float slimeAttackVol;
    [SerializeField] public AudioClip[] slimeDeath;
    [Range(0, 1)][SerializeField] public float slimeDeathVol;
    [SerializeField] public AudioClip[] slimeMovement;
    [Range(0, 1)][SerializeField] public float slimeMovementVol;

    [Header("---------- Turret Audio ----------")]
    [SerializeField] public AudioClip[] turretAlert;
    [Range(0, 1)][SerializeField] public float turretAlertVol;
    [SerializeField] public AudioClip[] turretAttack;
    [Range(0, 1)][SerializeField] public float turretAttackVol;
    [SerializeField] public AudioClip[] turretDeath;
    [Range(0, 1)][SerializeField] public float turretDeathVol;

    // ------------------------------------------- Boss -------------------------------------------------------------
    [Header("---------- Slime Mech Audio ----------")]
    [SerializeField] public AudioClip slimeMechDeath;
    [SerializeField] public float slimeMechDeathVolume;
    [SerializeField] public AudioClip slimeMechIntro;
    [SerializeField] public float slimeMechIntroVolume;
    [SerializeField] public AudioClip slimeMechForceFieldSound;
    [SerializeField] public float slimeMechForceFieldVolume;

    [Header("---------- Home Security System Audio ----------")]
    [SerializeField] public AudioClip HSSDeath;
    [SerializeField] public float HSSDeathVolume;
    [SerializeField] public AudioClip HSSIntro;
    [SerializeField] public float HSSIntroVolume;
    [SerializeField] public AudioClip HSSPhase1;
    [SerializeField] public float HSSPhase1Volume;
    [SerializeField] public AudioClip HSSPhase2;
    [SerializeField] public float HSSPhase2Volume;
    [SerializeField] public AudioClip HSSPhase3;
    [SerializeField] public float HSSPhase3Volume;

    [Header("---------- Advanced Specimen Audio ----------")]
    [SerializeField] public AudioClip advSpecDeath;
    [SerializeField] public float advSpecDeathVolume;
    [SerializeField] public AudioClip advSpecIntro;
    [SerializeField] public float advSpecIntroVolume;
    [SerializeField] public AudioClip advSpecHurt;
    [SerializeField] public float advSpecHurtVolume;

    // ------------------------------------------ Player weapons -----------------------------------------------------

    [Header("---------- Pistol Audio ----------")]
    public AudioClip[] pistolShootSound;
    [Range(0, 1)] public float pistolShootVol;
    public AudioClip[] pistolReloadSound;
    [Range(0, 1)] public float pistolReloadVol;
    public AudioClip[] pistolPickupSound;
    [Range(0, 1)] public float pistolPickupVol;
    public AudioClip[] pistolEmptySound;
    [Range(0, 1)] public float pistolEmptyVol;

    [Header("---------- Rifle Audio ----------")]
    public AudioClip[] rifleShootSound;
    [Range(0, 1)] public float rifleShootVol;
    public AudioClip[] rifleReloadSound;
    [Range(0, 1)] public float rifleReloadVol;
    public AudioClip[] riflePickupSound;
    [Range(0, 1)] public float riflePickupVol;
    public AudioClip[] rifleEmptySound;
    [Range(0, 1)] public float rifleEmptyVol;

    [Header("---------- GrenadeLauncher Audio ----------")]
    public AudioClip[] glShootSound;
    [Range(0, 1)] public float glShootVol;
    public AudioClip[] glReloadSound;
    [Range(0, 1)] public float glReloadVol;
    public AudioClip[] glPickupSound;
    [Range(0, 1)] public float glPickupVol;
    public AudioClip[] glEmptySound;
    [Range(0, 1)] public float glEmptyVol;

    [Header("---------- ArcGun Audio ----------")]
    public AudioClip[] arcgunShootSound;
    [Range(0, 1)] public float arcgunShootVol;
    public AudioClip[] arcgunReloadSound;
    [Range(0, 1)] public float arcgunReloadVol;
    public AudioClip[] arcgunPickupSound;
    [Range(0, 1)] public float arcgunPickupVol;
    public AudioClip[] arcgunEmptySound;
    [Range(0, 1)] public float arcgunEmptyVol;

    [Header("---------- RailGun Audio ----------")]
    public AudioClip[] railgunShootSound;
    [Range(0, 1)] public float railgunShootVol;
    public AudioClip[] railgunReloadSound;
    [Range(0, 1)] public float railgunReloadVol;
    public AudioClip[] railgunPickupSound;
    [Range(0, 1)] public float railgunPickupVol;
    public AudioClip[] railgunEmptySound;
    [Range(0, 1)] public float railgunEmptyVol;
    public AudioClip[] railgunChargeSound;
    [Range(0, 1)] public float railgunChargeVol;

    // ------------------------------------------ Environment -----------------------------------------------------
    [Header("---------- Environment Audio ----------")]
    public AudioClip[] boxBreak;
    [Range(0, 1)] public float boxBreakVol;
    public AudioClip[] doorOpen;
    [Range(0, 1)] public float doorOpenVol;
    public AudioClip[] doorClose;
    [Range(0, 1)] public float doorCloseVol;
    public AudioClip[] menuClose;
    [Range(0, 1)] public float menuCloseVol;
    public AudioClip[] menuOpen;
    [Range(0, 1)] public float menuOpenVol;
    public AudioClip[] menuHover;
    [Range(0, 1)] public float menuHoverVol;
    public AudioClip[] bulletTimeEnter;
    [Range(0, 1)] public float bulletTimeEnterVol;
    public AudioClip[] bulletTimeExit;
    [Range(0, 1)] public float bulletTimeExitVol;

    private void Awake()
    {
        instance = this;
    }

    public void setSFXVolume()
    {
        float sfxSliderValue = sfxVolumeSlider.value;
        aud.volume = sfxSliderValue;
    }
}
