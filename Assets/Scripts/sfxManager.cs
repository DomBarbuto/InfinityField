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
    [SerializeField] public AudioClip[] turretAttack;
    [SerializeField] public AudioClip[] turretDeath;

    // ------------------------------------------- Boss -------------------------------------------------------------
    [Header("---------- Slime Mech Audio ----------")]
    [SerializeField] public AudioClip slimeMechDeath;
    [SerializeField] public AudioClip slimeMechIntro;
    [SerializeField] public AudioClip slimeMechForceFieldSound;

    [Header("---------- Home Security System Audio ----------")]
    [SerializeField] public AudioClip HSSDeath;
    [SerializeField] public AudioClip HSSIntro;
    [SerializeField] public AudioClip HSSPhase1;
    [SerializeField] public AudioClip HSSPhase2;
    [SerializeField] public AudioClip HSSPhase3;

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
}
