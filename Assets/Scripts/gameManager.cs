using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---- Player Components ----")]
    public GameObject player;                            // Object reference for the player
    public playerController playerController;            // Reference directly to the script
    public inventory inventory;
    [SerializeField] GameObject playerLastKnownPosition; // Reference to prefab to be instantiated when player loses the enemy
    [SerializeField] public AudioMixerSnapshot fadeInSnapShot;
    [SerializeField] public AudioMixerSnapshot fadedOutSnapshot;

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
    [SerializeField] public Slider musicVolumeSlider;

    [Header("---- Inventory Menu ----")]
    [SerializeField] GameObject invWheelPointer;
    [SerializeField] GameObject highlight1;
    [SerializeField] GameObject highlight2;
    [SerializeField] GameObject highlight3;
    [SerializeField] GameObject highlight4;
    [SerializeField] GameObject highlight5;
    [SerializeField] public GameObject[] slots;
    [SerializeField] public GameObject[] perksShown;
    [SerializeField] public TextMeshProUGUI[] perksShownNames;
    [SerializeField] public Sprite[] perkIcons;

    public enum UIMENUS { pauseMenu, winMenu, deathMenu, inventoryMenu, optionsMenu, controlsMenu, perksMenu }

    [Header("---- Inventory -----")]
    public GameObject collectableCreditsPrefab;         // Reference to the collectableCredits prefab
    public int credits;                                 // Amount of currency the player has
    public int upgradeCost;                             // Amount of credits required to purchase a base upgrade

    [Header("---- Scene Statistics ----")]
    public int enemyCount;

    [Header("---- System Information ----")]
    public bool isPaused = false;
    public float timeScaleOrig;
    public GameObject playerSpawnPoint;
    public dynamicAudio composer;

    //public bool isPlayerDetected;
    [Range(3, 5)][SerializeField] float playerLastKnownPositionTimeout;

    void Awake()
    {
        instance = this;

        /*// Singleton
        if (instance == null)
        {
            Debug.Log("Instance was null");
            instance = this;
        }
        else
        {
            Debug.Log("new instance destroying");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.root.gameObject);*/


        player = GameObject.FindGameObjectWithTag("Player");
        if (SceneManager.GetActiveScene().name != "Main Menu")
            playerController = player.GetComponent<playerController>();
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn Point");
        composer = GameObject.FindGameObjectWithTag("Composer").GetComponent<dynamicAudio>();
        timeScaleOrig = Time.timeScale;
        currentWeaponUI.SetActive(false);
        updateCreditUI();
        playerController.characterList[playerController.currCharacter].perks.Clear();
        while (playerController.characterList[playerController.currCharacter].perks.Count > 0)
        {
            playerController.characterList[playerController.currCharacter].perks.RemoveAt(0);
        }
        if (playerController.currCharacterModel.GetComponent<SkinnedMeshRenderer>().material != playerController.characterList[playerController.currCharacter].material)
        {
            playerController.currCharacterModel.GetComponent<SkinnedMeshRenderer>().material = playerController.characterList[playerController.currCharacter].material;
        }

        credits = PlayerPrefs.GetInt("Credits");
        creditsCounterText.text = credits.ToString();
    }

    private void Start()
    {
        // Hide reticle on start. Reticle only shows when weapon is selected
        hideReticle();
        buttonFunctions.loadCharacterSettings();
        

    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            SetActiveMenu(UIMENUS.pauseMenu);

            // Pause and fade in/out audio
            if (isPaused)
                pause();
            else
                unPause();
        }
        else if ((Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0) && activeMenu == null)
        {
            if (activeMenu != menus[(int)UIMENUS.inventoryMenu])
            {

                SetActiveMenu(UIMENUS.inventoryMenu);
                invWheelPointer.SetActive(true);
                menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles = new Vector3(0, 0, (-72 * playerController.currentWeapon));
            }
        }
        else if (Input.GetButtonDown("Perks") && (activeMenu == menus[(int)UIMENUS.perksMenu] || activeMenu == null))
        {
            if (activeMenu == menus[(int)UIMENUS.perksMenu])
            {
                unPause();
            }
            else if(activeMenu == null)
            {
                SetActiveMenu(UIMENUS.perksMenu);
                showPerks();
                pause();
            }
        }

        if (activeMenu == menus[(int)UIMENUS.inventoryMenu])
        {
            playerController.canFire = false;
            getSelectedItem();

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                menus[(int)UIMENUS.inventoryMenu].transform.Rotate(Vector3.forward, +10);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                menus[(int)UIMENUS.inventoryMenu].transform.Rotate(Vector3.forward, -10);
            }
        }
    }

    public void pause()
    {
        fadedOutSnapshot.TransitionTo(2);

        if (!isPaused)
            isPaused = true;

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        fadeInSnapShot.TransitionTo(1);

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
        if (!reticle.activeInHierarchy)
            reticle.SetActive(true);
    }

    public void updateEnemyCount(int amount)
    {
        enemyCount += amount;
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
        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 36 || menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 324)
        {
            highlight1.SetActive(true); //Leave the or statement. It's required
            if (playerController.weaponInventory[0] != null) { playerController.currentWeapon = 0; }
        }
        else { highlight1.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 324 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 252)
        {
            highlight2.SetActive(true);
            if (playerController.weaponInventory[1] != null) { playerController.currentWeapon = 1; }
        }
        else { highlight2.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 252 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 180)
        {
            highlight3.SetActive(true);
            if (playerController.weaponInventory[2] != null) { playerController.currentWeapon = 2; }
        }
        else { highlight3.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 180 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 108)
        {
            highlight4.SetActive(true);
            if (playerController.weaponInventory[3] != null) { playerController.currentWeapon = 3; }
        }
        else { highlight4.SetActive(false); }

        if (menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z < 108 && menus[(int)UIMENUS.inventoryMenu].transform.eulerAngles.z > 36)
        {
            highlight5.SetActive(true);
            if (playerController.weaponInventory[4] != null) { playerController.currentWeapon = 4; }
        }
        else { highlight5.SetActive(false); }

        // When player clicks on a weapon slot, unPause, turn off inventory, and select the weapon.
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Inventory"))
        {
            unPause();
            invWheelPointer.SetActive(false);
            playerController.selectWeapon(playerController.weaponInventory[playerController.currentWeapon]);
            playerController.canFire = false;
        }
    }

    public void setWeaponIcon(weaponCreation weapon, int invSlotIndex)
    {
        slots[invSlotIndex].GetComponent<Image>().sprite = weapon.icon;
    }

    public void updateCreditUI()
    {
        creditsCounterText.text = credits.ToString();
    }

    public void setComposerVolume()
    {
        float volumeSliderValue = musicVolumeSlider.value;
        composer.speaker.volume = volumeSliderValue;
    }

    public void showPerks()
    {
        for (int i = 0; i < playerController.characterList[playerController.currCharacter].perks.Count; i++)
        {
            perksShown[i].SetActive(true);
            perksShownNames[i].text = $"{playerController.characterList[playerController.currCharacter].perks[i].perkName} - {playerController.characterList[playerController.currCharacter].perks[i].rarity}";

            switch (playerController.characterList[playerController.currCharacter].perks[i].rarity)
            {
                case perkList.PerkRarity.common:
                    perksShown[i].transform.parent.GetComponent<Image>().color = Color.white;
                    break;

                case perkList.PerkRarity.uncommon:
                    perksShown[i].transform.parent.GetComponent<Image>().color = Color.green;
                    break;

                case perkList.PerkRarity.rare:
                    perksShown[i].transform.parent.GetComponent<Image>().color = Color.blue;
                    break;

                case perkList.PerkRarity.epic:
                    Color purple = new Color(0.5f, 0, 0.5f);
                    perksShown[i].transform.parent.GetComponent<Image>().color = purple;
                    break;

                case perkList.PerkRarity.legendary:
                    Color orange = new Color(1f, 0.5f, 0);
                    perksShown[i].transform.parent.GetComponent<Image>().color = orange;
                    break;


                default:
                    Debug.Log("Something fucked up");
                    break;

            }

            switch(playerController.characterList[playerController.currCharacter].perks[i].perkName)
            {
                case "Precision Synchronizer":
                    perksShown[i].transform.GetComponent<Image>().sprite = perkIcons[0];
                    break;
                case "Adrenaline":
                    perksShown[i].transform.GetComponent<Image>().sprite = perkIcons[1];
                    break;
                case "Time Heals Wounds":
                    perksShown[i].transform.GetComponent<Image>().sprite = perkIcons[2];
                    break;
                case "Rejuvination":
                    perksShown[i].transform.GetComponent<Image>().sprite = perkIcons[3];
                    break;
                case "Rocket Man":
                    perksShown[i].transform.GetComponent<Image>().sprite = perkIcons[4];
                    break;

                default:
                    Debug.Log("Something is not working with names");
                    break;
            }
        }
    }
}
