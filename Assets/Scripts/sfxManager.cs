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
    [Range(0, 1)][SerializeField] public float playerHurtVolMulti;
    [SerializeField] public AudioClip[] playerJump;
    [Range(0, 1)][SerializeField] public float playerJumpVolMulti;
    [SerializeField] public AudioClip[] playerFootstep;
    [Range(0, 1)][SerializeField] public float playerFootstepVolMulti;
    [SerializeField] public AudioClip[] playerSlimedFootstep;
    [Range(0, 1)][SerializeField] public float playerSlimedFootstepVolMulti;
    [SerializeField] public AudioClip[] ricochetSound;
    [Range(0, 1)][SerializeField] public float ricochetSoundVolMulti;

    [Header("---------- RedCC Audio ----------")]
    [SerializeField] public AudioClip[] redCCAlert;
    [Range(0, 1)][SerializeField] public float redCCAlertVolMulti;
    [SerializeField] public AudioClip[] redCCHurt;
    [Range(0, 1)][SerializeField] public float redCCHurtVolMulti;

    [Header("---------- YellowCC Audio ----------")]
    [SerializeField] public AudioClip[] yellowCCAlert;
    [Range(0, 1)][SerializeField] public float yellowCCAlertVolMulti;
    [SerializeField] public AudioClip[] yellowCCHurt;
    [Range(0, 1)][SerializeField] public float yellowCCHurtVolMulti;
    
    [Header("---------- WhiteCC Audio ----------")]
    [SerializeField] public AudioClip[] whiteCCAlert;
    [Range(0, 1)][SerializeField] public float whiteCCAlertVolMulti;
    [SerializeField] public AudioClip[] whiteCCHurt;
    [Range(0, 1)][SerializeField] public float whiteCCHurtVolMulti;

    [Header("---------- BlackCC Audio ----------")]
    [SerializeField] public AudioClip[] blackCCAlert;
    [Range(0, 1)][SerializeField] public float blackCCAlertVolMulti;
    [SerializeField] public AudioClip[] blackCCHurt;
    [Range(0, 1)][SerializeField] public float blackCCHurtVolMulti;

    [Header("---------- Humanoid Specimen Audio ----------")]
    [SerializeField] public AudioClip[] humanoidSpecimenAlert;
    [Range(0, 1)][SerializeField] public float humanoidSpecimenAlertVolMulti;
    [SerializeField] public AudioClip[] humanoidSpecimenAttack;
    [Range(0, 1)][SerializeField] public float humanoidSpecimenAttackVolMulti;
    [SerializeField] public AudioClip[] humanoidSpecimenHurt;
    [Range(0, 1)][SerializeField] public float humanoidSpecimenHurtVolMulti;

    [Header("---------- Exploding Specimen Audio ----------")]
    [SerializeField] public AudioClip[] explodingSpecimenHiss;
    [Range(0, 1)][SerializeField] public float explodingSpecimenHissVolMulti;
    [SerializeField] public AudioClip[] explodingSpecimenMovement;
    [Range(0, 1)][SerializeField] public float explodingSpecimenMovementVolMulti;
    [SerializeField] public AudioClip[] explodingSpecimenExplode;
    [Range(0, 1)][SerializeField] public float explodingSpecimenExplodeVolMulti;

    [Header("---------- Slime Audio ----------")]
    [SerializeField] public AudioClip[] slimeAlert;
    [Range(0, 1)][SerializeField] public float slimeAlertVolMulti;
    [SerializeField] public AudioClip[] slimeAttack;
    [Range(0, 1)][SerializeField] public float slimeAttackVolMulti;
    [SerializeField] public AudioClip[] slimeDeath;
    [Range(0, 1)][SerializeField] public float slimeDeathVolMulti;
    [SerializeField] public AudioClip[] slimeMovement;
    [Range(0, 1)][SerializeField] public float slimeMovementVolMulti;

    [Header("---------- Turret Audio ----------")]
    [SerializeField] public AudioClip[] turretAlert;
    [Range(0, 1)][SerializeField] public float turretAlertVolMulti;
    [SerializeField] public AudioClip[] turretAttack;
    [Range(0, 1)][SerializeField] public float turretAttackVolMulti;
    [SerializeField] public AudioClip[] turretDeath;
    [Range(0, 1)][SerializeField] public float turretDeathVolMulti;

    // ------------------------------------------- Boss -------------------------------------------------------------
    [Header("---------- Slime Mech Audio ----------")]
    [SerializeField] public AudioClip slimeMechDeath;
    [Range(0, 1)][SerializeField] public float slimeMechDeathVolumeMulti;
    [SerializeField] public AudioClip slimeMechIntro;
    [Range(0, 1)][SerializeField] public float slimeMechIntroVolumeMulti;
    [SerializeField] public AudioClip slimeMechForceFieldSound;
    [Range(0, 1)][SerializeField] public float slimeMechForceFieldVolumeMulti;

    [Header("---------- Home Security System Audio ----------")]
    [SerializeField] public AudioClip HSSDeath;
    [Range(0, 1)][SerializeField] public float HSSDeathVolumeMulti;
    [SerializeField] public AudioClip HSSIntro;
    [Range(0, 1)][SerializeField] public float HSSIntroVolumeMulti;
    [SerializeField] public AudioClip HSSPhase1;
    [Range(0, 1)][SerializeField] public float HSSPhase1VolumeMulti;
    [SerializeField] public AudioClip HSSPhase2;
    [Range(0, 1)][SerializeField] public float HSSPhase2VolumeMulti;
    [SerializeField] public AudioClip HSSPhase3;
    [Range(0, 1)][SerializeField] public float HSSPhase3VolumeMulti;

    [Header("---------- Advanced Specimen Audio ----------")]
    [SerializeField] public AudioClip advSpecDeath;
    [Range(0, 1)][SerializeField] public float advSpecDeathVolumeMulti;
    [SerializeField] public AudioClip advSpecIntro;
    [Range(0, 1)][SerializeField] public float advSpecIntroVolumeMulti;
    [SerializeField] public AudioClip advSpecHurt;
    [Range(0, 1)][SerializeField] public float advSpecHurtVolumeMulti;

    // ------------------------------------------ Player weapons -----------------------------------------------------

    [Header("---------- Pistol Audio ----------")]
    public AudioClip[] pistolShootSound;
    [Range(0, 1)] public float pistolShootVolMulti;
    public AudioClip[] pistolReloadSound;
    [Range(0, 1)] public float pistolReloadVolMulti;
    public AudioClip[] pistolPickupSound;
    [Range(0, 1)] public float pistolPickupVolMulti;
    public AudioClip[] pistolEmptySound;
    [Range(0, 1)] public float pistolEmptyVolMulti;

    [Header("---------- Rifle Audio ----------")]
    public AudioClip[] rifleShootSound;
    [Range(0, 1)] public float rifleShootVolMulti;
    public AudioClip[] rifleReloadSound;
    [Range(0, 1)] public float rifleReloadVolMulti;
    public AudioClip[] riflePickupSound;
    [Range(0, 1)] public float riflePickupVolMulti;
    public AudioClip[] rifleEmptySound;
    [Range(0, 1)] public float rifleEmptyVolMulti;

    [Header("---------- GrenadeLauncher Audio ----------")]
    public AudioClip[] glShootSound;
    [Range(0, 1)] public float glShootVolMulti;
    public AudioClip[] glReloadSound;
    [Range(0, 1)] public float glReloadVolMulti;
    public AudioClip[] glPickupSound;
    [Range(0, 1)] public float glPickupVolMulti;
    public AudioClip[] glEmptySound;
    [Range(0, 1)] public float glEmptyVolMulti;

    [Header("---------- ArcGun Audio ----------")]
    public AudioClip[] arcgunShootSound;
    [Range(0, 1)] public float arcgunShootVolMulti;
    public AudioClip[] arcgunReloadSound;
    [Range(0, 1)] public float arcgunReloadVolMulti;
    public AudioClip[] arcgunPickupSound;
    [Range(0, 1)] public float arcgunPickupVolMulti;
    public AudioClip[] arcgunEmptySound;
    [Range(0, 1)] public float arcgunEmptyVolMulti;

    [Header("---------- RailGun Audio ----------")]
    public AudioClip[] railgunShootSound;
    [Range(0, 1)] public float railgunShootVolMulti;
    public AudioClip[] railgunReloadSound;
    [Range(0, 1)] public float railgunReloadVolMulti;
    public AudioClip[] railgunPickupSound;
    [Range(0, 1)] public float railgunPickupVolMulti;
    public AudioClip[] railgunEmptySound;
    [Range(0, 1)] public float railgunEmptyVolMulti;
    public AudioClip[] railgunChargeSound;
    [Range(0, 1)] public float railgunChargeVolMulti;

    // ------------------------------------------ Environment -----------------------------------------------------
    [Header("---------- Environment Audio ----------")]
    public AudioClip[] boxBreak;
    [Range(0, 1)] public float boxBreakVolMulti;
    public AudioClip[] doorOpen;
    [Range(0, 1)] public float doorOpenVolMulti;
    public AudioClip[] doorClose;
    [Range(0, 1)] public float doorCloseVolMulti;
    public AudioClip[] menuClose;
    [Range(0, 1)] public float menuCloseVolMulti;
    public AudioClip[] menuOpen;
    [Range(0, 1)] public float menuOpenVolMulti;
    public AudioClip[] menuHover;
    [Range(0, 1)] public float menuHoverVolMulti;
    public AudioClip[] bulletTimeEnter;
    [Range(0, 1)] public float bulletTimeEnterVolMulti;
    public AudioClip[] bulletTimeExit;
    [Range(0, 1)] public float bulletTimeExitVolMulti;

    private void Start()
    {
        instance = this;
    }

    public void setSFXVolume()
    {
        float sfxSliderValue = sfxVolumeSlider.value;
        aud.volume = sfxSliderValue;
    }
}
