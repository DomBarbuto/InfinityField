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
    public GameObject playerLastKnownPosition; // Reference to prefab to be instantiated when player loses the enemy

    [Header("---- UI Components ----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject winMenu;
    public GameObject deathMenu;
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
    [Range(1, 5)][SerializeField] float playerLastKnownPositionTimeout;

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
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
                pause();
            else
                unPause();
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void addCredits(int amount)
    {
        credits += amount;
    }

    public void updateEnemyCount(int amount)
    {
        enemyCount += amount;
    }

    public IEnumerator DisplayPlayerLastKnownPosition()
    {
        GameObject lastKnown = Instantiate(playerLastKnownPosition, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(playerLastKnownPositionTimeout);
        Destroy(lastKnown);
    }

}
