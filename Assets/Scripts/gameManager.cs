using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    //used for the singleton design pattern
    public static gameManager instance;

    [Header("---- Player Components ----")]
    public GameObject player; //Object reference for the player
    public playerController playerController; //Reference directly to the script

    [Header("---- UI Components ----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject inventoryMenu;//Where player can switch weapons
    public GameObject upgradeMenu;//Where player can upgrade Power Suit statistics such as speed and number of jumps
    public GameObject playerDamageFX;//Damage screen effect and SFX

    [Header ("---- Inventory -----")]
    public int credits; //Amount of currency the player has
    public int upgradeCost; //Amount of credits required to purchase a base upgrade

    [Header("---- Scene Statistics ----")]
    public int enemyCount;

    [Header("---- System Information ----")]
    public bool isPaused = false;
    float timeScaleOrig;
    public GameObject playerSpawnPoint;
    public dynamicAudio composer;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn Point");
        composer = GameObject.FindGameObjectWithTag("Composer").GetComponent<dynamicAudio>();


        playerController = player.GetComponent<playerController>();
        timeScaleOrig = Time.timeScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pause()
    {

    }

    public void unPause()
    {

    }

    public void addCredits(int amount)
    {
        credits += amount;
    }

    public void updateEnemyCount(int amount)
    {

    }

}
