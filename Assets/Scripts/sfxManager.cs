using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class sfxManager : MonoBehaviour
{
    public static sfxManager instance;

    [Header("---------- Components ---------")]
    [SerializeField] public AudioSource aud;
    [SerializeField] public Slider sfxVolumeSlider;
    [SerializeField] public AudioMixer mixer;
    
    [Header("---------- Player Audio ----------")]
    [SerializeField] public AudioClip[] playerHurt;
    [SerializeField] public AudioClip[] playerJump;
    [SerializeField] public AudioClip[] playerFootstep;
    [SerializeField] public AudioClip[] playerSlimedFootstep;
    [SerializeField] public AudioClip[] ricochetSound;

    [Header("---------- RedCC Audio ----------")]
    [SerializeField] public AudioClip[] redCCAlert;
    [SerializeField] public AudioClip[] redCCHurt;

    [Header("---------- YellowCC Audio ----------")]
    [SerializeField] public AudioClip[] yellowCCAlert;
    [SerializeField] public AudioClip[] yellowCCHurt;
    
    [Header("---------- WhiteCC Audio ----------")]
    [SerializeField] public AudioClip[] whiteCCAlert;
    [SerializeField] public AudioClip[] whiteCCHurt;

    [Header("---------- BlackCC Audio ----------")]
    [SerializeField] public AudioClip[] blackCCAlert;
    [SerializeField] public AudioClip[] blackCCHurt;

    [Header("---------- Humanoid Specimen Audio ----------")]
    [SerializeField] public AudioClip[] humanoidSpecimenAlert;
    [SerializeField] public AudioClip[] humanoidSpecimenAttack;
    [SerializeField] public AudioClip[] humanoidSpecimenHurt;

    [Header("---------- Exploding Specimen Audio ----------")]
    [SerializeField] public AudioClip[] explodingSpecimenHiss;
    [SerializeField] public AudioClip[] explodingSpecimenMovement;
    [SerializeField] public AudioClip[] explodingSpecimenExplode;

    [Header("---------- Slime Audio ----------")]
    [SerializeField] public AudioClip[] slimeAlert;
    [SerializeField] public AudioClip[] slimeAttack;
    [SerializeField] public AudioClip[] slimeDeath;
    [SerializeField] public AudioClip[] slimeMovement;

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
    [SerializeField] public float slimeMechDeathVolumeMulti;
    [SerializeField] public AudioClip slimeMechIntro;
    [SerializeField] public float slimeMechIntroVolumeMulti;
    [SerializeField] public AudioClip slimeMechForceFieldSound;
    [SerializeField] public float slimeMechForceFieldVolumeMulti;

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
    [SerializeField] public AudioClip advSpecIntro;
    [SerializeField] public AudioClip advSpecHurt;

    // ------------------------------------------ Player weapons -----------------------------------------------------

    [Header("---------- Pistol Audio ----------")]
    public AudioClip[] pistolShootSound;
    public AudioClip[] pistolReloadSound;
    public AudioClip[] pistolPickupSound;
    public AudioClip[] pistolEmptySound;

    [Header("---------- Rifle Audio ----------")]
    public AudioClip[] rifleShootSound;
    public AudioClip[] rifleReloadSound;
    public AudioClip[] riflePickupSound;
    public AudioClip[] rifleEmptySound;

    [Header("---------- GrenadeLauncher Audio ----------")]
    public AudioClip[] glShootSound;
    public AudioClip[] glReloadSound;
    public AudioClip[] glPickupSound;
    public AudioClip[] glEmptySound;

    [Header("---------- ArcGun Audio ----------")]
    public AudioClip[] arcgunShootSound;
    public AudioClip[] arcgunReloadSound;
    public AudioClip[] arcgunPickupSound;
    public AudioClip[] arcgunEmptySound;

    [Header("---------- RailGun Audio ----------")]
    public AudioClip[] railgunShootSound;
    public AudioClip[] railgunReloadSound;
    public AudioClip[] railgunPickupSound;
    public AudioClip[] railgunEmptySound;
    public AudioClip[] railgunChargeSound;

    // ------------------------------------------ Environment -----------------------------------------------------
    [Header("---------- Environment Audio ----------")]
    public AudioClip[] boxBreak;
    public float boxBreakVolMulti;
    public AudioClip[] doorOpen;
    public float doorOpenVolMulti;
    public AudioClip[] doorClose;
    public float doorCloseVolMulti;
    public AudioClip[] menuClose;
    public float menuCloseVolMulti;
    public AudioClip[] menuOpen;
    public float menuOpenVolMulti;
    public AudioClip[] menuHover;
    public float menuHoverVolMulti;
    public AudioClip[] bulletTimeEnter;
    public float bulletTimeEnterVolMulti;
    public AudioClip[] bulletTimeExit;
    public float bulletTimeExitVolMulti;

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
