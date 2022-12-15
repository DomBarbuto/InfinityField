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
    public inventory inventory;
    [SerializeField] GameObject playerLastKnownPosition; // Reference to prefab to be instantiated when player loses the enemy

    [Header("---- UI Components ----")]
    public GameObject reticle;
    public GameObject[] menus;
    [SerializeField] public GameObject activeMenu;
    public GameObject playerDamageFX;                    // Damage screen effect
    public GameObject[] collectableUIFX;                 // Collectable ui effects
    public TextMeshProUGUI creditsCounterText;           // Text for collected credits
    public TextMeshProUGUI MagazineCurrent;              // Shows ammo in magazine
    public TextMeshProUGUI AmmoPoolCurrent;              // Shows ammo in ammo pool
    public GameObject currentWeaponIcon;
    public GameObject currentWeaponUI;
    public Image playerHPBar;
    public Image playerEnergyBar;

    [Header("---- Inventory Menu ----")]
    [SerializeField] GameObject invWheelPointer;
    [SerializeField] GameObject highlight1;
    [SerializeField] GameObject highlight2;
    [SerializeField] GameObject highlight3;
    [SerializeField] GameObject highlight4;
    [SerializeField] GameObject highlight5;
    [SerializeField] public GameObject[] slots;

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

    //public bool isPlayerDetected;
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
        currentWeaponUI.SetActive(false);
    }

    private void Start()
    {
        // Hide reticle on start. Reticle only shows when weapon is selected
        hideReticle();

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
        else if(Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(activeMenu != menus[(int)UIMENUS.inventoryMenu])
            {
                
                SetActiveMenu(UIMENUS.inventoryMenu);
                invWheelPointer.SetActive(true);
                menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles = new Vector3(0, 0, (72 * playerController.currentWeapon));
                //TODO: TEMP COMMENT BY DOM inventory.updateInventory();
            }
        
            
        }


        if(activeMenu == menus[(int)UIMENUS.inventoryMenu])
        {
            getSelectedItem();
            if (playerController.weaponInventory[playerController.currentWeapon] != null)
            {
                playerController.weaponOBJ.GetComponent<MeshFilter>().sharedMesh = playerController.weaponInventory[playerController.currentWeapon].weaponsModel.GetComponentInChildren<MeshFilter>().sharedMesh;
                playerController.weaponOBJ.GetComponent<MeshRenderer>().sharedMaterial = playerController.weaponInventory[playerController.currentWeapon].weaponsModel.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            }
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                menus[(int)UIMENUS.inventoryMenu].transform.Rotate(Vector3.forward, +10);
            }
            else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                menus[(int)UIMENUS.inventoryMenu].transform.Rotate(Vector3.forward, -10);
            }
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

    // calls coroutine to prevent errors from object calling it being destroyed
    public void startCollectableUIFX(float UIFXLength, int index)
    {
        StartCoroutine(playCollectableUIFX(UIFXLength, index));
    }

    public IEnumerator playCollectableUIFX(float UIFXLength, int index)
    {
        collectableUIFX[index].SetActive(true);

        yield return new WaitForSeconds(UIFXLength);
        collectableUIFX[index].SetActive(false);
    }
    public void playHealthUIFX()
    {

    }
    public void playEnergyUIFX()
    {

    }

    public void updatePlayerHPBar()
    {
        playerHPBar.fillAmount = (float)playerController.getHP() / (float)playerController.getMAXHP();
    }

    public void updatePlayerEnergyBar()
    {
        playerEnergyBar.fillAmount = playerController.getEnergy() / playerController.getMAXEnergy();
    }

    public void hideReticle()
    {
        reticle.SetActive(false);
    }

    public void showReticle()
    {
        // Only call setActive if it is not active
        if(!reticle.activeInHierarchy)
            reticle.SetActive(true);
    }

    public void updateEnemyCount(int amount)
    {
        enemyCount += amount;

        if (enemyCount <= 0)
        {
            //End game
            //Win screen activated
            //pause();
            //SetActiveMenu(UIMENUS.winMenu);
        }
    }

    public IEnumerator DisplayPlayerLastKnownPosition()
    {
        currentLastKnownPosition = Instantiate(playerLastKnownPosition, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(playerLastKnownPositionTimeout);

        /*if(currentLastKnownPosition != null && !isPlayerDetected)
        {
            Destroy(currentLastKnownPosition);
            currentLastKnownPosition = null;
        }*/
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
    
    public void getSelectedItem()
    {
        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 36 || menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 324) { highlight1.SetActive(true); //Leave the or statement. It's required
            if (playerController.weaponInventory[0] != null) { playerController.currentWeapon = 0; }} 
        else { highlight1.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 324 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 252) { highlight2.SetActive(true);
            if (playerController.weaponInventory[1] != null) { playerController.currentWeapon = 1; }}
        else { highlight2.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 252 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 180) { highlight3.SetActive(true);
            if (playerController.weaponInventory[2] != null) { playerController.currentWeapon = 2; }}
        else { highlight3.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 180 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 108) { highlight4.SetActive(true);
            if (playerController.weaponInventory[3] != null) { playerController.currentWeapon = 3; }}
        else { highlight4.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 108 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 36) { highlight5.SetActive(true);
            if (playerController.weaponInventory[4] != null) { playerController.currentWeapon = 4; }}
        else { highlight5.SetActive(false); }

        if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Inventory"))
        {
            unPause();
            invWheelPointer.SetActive(false);
            
        }
    }

    public void updateCreditUI()
    {
        creditsCounterText.text = gameManager.instance.credits.ToString();
    }
    //can be used and customized as you wish. This was mainly theoery crafting for the time being

}
