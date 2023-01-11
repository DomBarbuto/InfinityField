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
    [SerializeField] public AudioClip[] ricochetSound;
    [Range(0, 1)][SerializeField] public float ricochetSoundVol;

    [Header("---------- Enemy Audio ----------")]
    [SerializeField] public AudioClip[] enemyHurt;
    [Range(0, 1)][SerializeField] public float enemyHurtVol;
    [SerializeField] public AudioClip[] enemyAlert;
    [Range(0, 1)][SerializeField] public float enemyAlertVol;

    [Header("---------- Pistol Audio ----------")]
    public AudioClip[] pistolShootSound;
    [Range(0, 1)] public float pistolShootVol;
    public AudioClip[] pistolReloadSound;
    [Range(0, 1)] public float pistolreloadVol;
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

    [Header("---------- RailGun Audio ----------")]
    public AudioClip[] railgunShootSound;
    [Range(0, 1)] public float railgunShootVol;
    public AudioClip[] railgunReloadSound;
    [Range(0, 1)] public float railgunReloadVol;
    public AudioClip[] railgunPickupSound;
    [Range(0, 1)] public float railgunPickupVol;
    public AudioClip[] railgunEmptySound;

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
