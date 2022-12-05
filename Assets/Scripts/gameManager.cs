using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---- Player Components ----")]
    public GameObject player;                            // Object reference for the player
    public playerController playerController;            // Reference directly to the script
    [SerializeField] GameObject playerLastKnownPosition; // Reference to prefab to be instantiated when player loses the enemy

    [Header("---- UI Components ----")]
    public GameObject[] menus;
    public GameObject activeMenu;
    public GameObject playerDamageFX;                    // Damage screen effect
    public GameObject collectedCreditsFX;                // Collectable screen effect
    public TextMeshProUGUI creditsCounterText;           // Text for collected credits
    
    public enum UIMENUS { pauseMenu, winMenu, deathMenu, inventoryMenu, upgradeMenu }

    [Header("---- Inventory -----")]
    public GameObject collectableCreditsPrefab;         // Reference to the collectableCredits prefab
    public int credits;                                 // Amount of currency the player has
    public int upgradeCost;                             // Amount of credits required to purchase a base upgrade

    [Header("---- Scene Statistics ----")]
    public int enemyCount;

    [Header("---- System Information ----")]
    public bool isPaused = false;
    float timeScaleOrig;
    public GameObject playerSpawnPoint;
    public dynamicAudio composer;

    public bool isPlayerDetected;
    [Range(3, 5)][SerializeField] float playerLastKnownPositionTimeout;
    public GameObject currentLastKnownPosition = null;


    void Awake()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<playerController>();
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn Point");
        composer = GameObject.FindGameObjectWithTag("Composer").GetComponent<dynamicAudio>();
        timeScaleOrig = Time.timeScale;
    }

    private void Start()
    {
        creditsCounterText.text = credits.ToString();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            SetActiveMenu(UIMENUS.pauseMenu);

            if (isPaused)
                pause();
            else
                unPause();
        }
    }

    public void pause()
    {
        if (!isPaused)
            isPaused = true;

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        if (isPaused)
            isPaused = false;

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

        if (enemyCount <= 0)
        {
            //End game
            //Win screen activated
            pause();
            SetActiveMenu(UIMENUS.winMenu);
        }
    }

    public IEnumerator DisplayPlayerLastKnownPosition()
    {
        currentLastKnownPosition = Instantiate(playerLastKnownPosition, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(playerLastKnownPositionTimeout);

        if(currentLastKnownPosition != null && !isPlayerDetected)
        {
            Destroy(currentLastKnownPosition);
            currentLastKnownPosition = null;
        }
    }

    // Getters/Setters
    public GameObject GetActiveMenu()
    {
        return activeMenu;
    }

    public void SetActiveMenu(UIMENUS newActiveMenu)
    {
        menus[(int)newActiveMenu].SetActive(true);
        activeMenu = menus[(int)newActiveMenu];
    }

}
