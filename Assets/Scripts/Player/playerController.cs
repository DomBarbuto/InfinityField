using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerAnimController))]
public class playerController : MonoBehaviour
{
    [Header("---- External Components ----")]
    [SerializeField] CharacterController controller;
    public PlayerAnimController animController;

    [Header("---- Player Stats ----")]
    /*[SerializeField] public float HP;                                               //All of these stats will be consolodated into character prefabs
    [SerializeField] public float energy;                                           //All of these stats will be consolodated into character prefabs
    [SerializeField] public float energyDecreaseRate;      */                         //All of these stats will be consolodated into character prefabs                                         //All of these stats will be consolodated into character prefabs
    [SerializeField] float damageFXLength;                                          //All of these stats will be consolodated into character prefabs
    public playerAbilities playerAbilities;

    [Header("---- Player Movement ----")]
    [SerializeField] public bool isSprinting;                                              //All of these stats will be consolodated into character prefabs
    [SerializeField] float currentMoveSpeed;                                        //All of these stats will be consolodated into character prefabs
    [Range(3, 8)][SerializeField] float walkSpeed;
    [Range(1, 4)][SerializeField] float sprintMultiplier;
    [Range(10, 15)][SerializeField] float jumpHeight;
    [Range(15, 35)][SerializeField] float gravityValue;
    [SerializeField] public Vector3 pushBack;
    [SerializeField] float pushBackTime;

    [Header("---- Character ----")]
    [SerializeField] public List<playerCharacter> characterList = new List<playerCharacter>();                    // 0 will be default. List will never be random. Will always be filled
    [SerializeField] public int currCharacter;


    [Header("Inventory")]
    [SerializeField] public List<weaponCreation> weaponInventory = new List<weaponCreation>();
    [SerializeField] int maxSlots = 5;
    [SerializeField] public int currentWeapon;

    [Header("Weapon Selection and Switching")]
    [SerializeField] public weaponCreation.WeaponType currentWeaponType; // Update this every time you switch weapons
    [SerializeField] List<GameObject> weaponModelList;
    [SerializeField] GameObject currentWeaponModel;
    [SerializeField] List<Transform> muzzlePointList;
    [SerializeField] public Transform currentMuzzlePoint;

    [Header("Interactable System")]
    [SerializeField] float rayDistance;

    [Header("Misc Variables")]
    [SerializeField] public GameObject RocketManExplosion;


    //Private Variables------------------
    bool isFiring;
    bool hasFired = false;  //This is for chargeable weapons
    public bool isReloading;
    bool stepIsPlaying;
    int currJumps;  //Times jumped since being grounded
    float MAXHP;      //Player's maximum health
    float MAXEnergy;  //Player's maximum energy
    float lastUpdate;
    bool hasChargePlayed = false;


    Vector3 playerVelocity;
    public Vector3 move;
    //------------------------------------

    //------- Functions --------

    private void Awake()
    {
        animController = GetComponent<PlayerAnimController>();
    }

    void Start()
    {
        // Set orginal stats
        characterList[currCharacter].HP = characterList[currCharacter].HPMax;
        characterList[currCharacter].energy = characterList[currCharacter].energyMax;

        setPlayerPos();
        currentMoveSpeed = walkSpeed;

        StartCoroutine(characterList[currCharacter].callPerkOnUpdate());
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);

            pushBack.x = Mathf.Lerp(pushBack.x, 0f, Time.deltaTime * pushBackTime);
            pushBack.y = Mathf.Lerp(pushBack.y, 0f, Time.deltaTime * (pushBackTime));
            pushBack.z = Mathf.Lerp(pushBack.z, 0f, Time.deltaTime * pushBackTime);
            useAbility();
            movement();

            if (!stepIsPlaying && move.magnitude > 0.3f && controller.isGrounded)
                StartCoroutine(playSteps());

            // Only shoot if not already shooting, not sprinting, and not reloading
            if (!isFiring && !isSprinting && !isReloading && Input.GetButton("Fire1"))
            {
                if (weaponInventory[currentWeapon] != null)  // Make sure inventory menu is not on
                {
                    if (!weaponInventory[currentWeapon].chargeable)
                    {
                        isFiring = true;
                        animController.shootTrigger();
                    }
                    else
                    {
                        isFiring = true;
                        StartCoroutine(fire());
                    }
                }
            }

            // Update ammo text and weapon icon
            if (weaponInventory[currentWeapon] != null)
            {
                gameManager.instance.MagazineCurrent.text = weaponInventory[currentWeapon].magazineCurrent.ToString();
                gameManager.instance.AmmoPoolCurrent.text = weaponInventory[currentWeapon].currentAmmoPool.ToString();
                gameManager.instance.currentWeaponIcon.GetComponent<Image>().sprite = weaponInventory[currentWeapon].icon;
            }

            // Only can reload if not reloading and dont have a full magazine
            if (Input.GetButtonDown("Reload") && !isReloading)
            {
                if (weaponInventory[currentWeapon].magazineCurrent < weaponInventory[currentWeapon].magazineMax)
                {
                    // Calls animation trigger, which calls this classes reload via animation EVENT
                    animController.reloadTrigger();
                    isReloading = true;
                }
            }

            if (characterList[currCharacter].isUsingAbility)
            {
                if (characterList[currCharacter].energy >= characterList[currCharacter].energyUseRate)
                {
                    if (characterList[currCharacter].ability != 1)
                    {
                        if (Time.time - lastUpdate >= 0.25f)
                        {
                            characterList[currCharacter].energy -= characterList[currCharacter].energyUseRate;
                            lastUpdate = Time.time;
                            gameManager.instance.updatePlayerEnergyBar();
                        }
                    }
                    else
                    {
                        if (Time.time - lastUpdate >= 0.025f)
                        {
                            characterList[currCharacter].energy -= characterList[currCharacter].energyUseRate;
                            lastUpdate = Time.time;
                            gameManager.instance.updatePlayerEnergyBar();
                        }
                    }
                }
                else
                {
                    characterList[currCharacter].isUsingAbility = false;

                    playerAbilities.useAbility();
                }
            }

            energyRecharge();


            // TODO: Pull into a separate script
            RaycastHit interactHit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out interactHit, rayDistance))
            {
                if (interactHit.collider.gameObject.CompareTag("Interactable"))
                {
                    interactHit.collider.GetComponent<IInteractable>().showText();

                    if (Input.GetButtonDown("Interact"))
                    {
                        //If hit is door and it hasnt closed yet
                        if (interactHit.collider.GetComponent<slidingDoorNew>())
                            interactHit.collider.GetComponent<IInteractable>().interact();

                        // Else if hit is a vending machine
                        else if (interactHit.collider.GetComponent<vendingMachine>())
                        {
                            interactHit.collider.GetComponent<IInteractable>().interact();
                        }
                        // Else if hit is a boss button
                        else if(interactHit.collider.GetComponent<BossButton>())
                        {
                            interactHit.collider.GetComponent<IInteractable>().interact();
                        }
                    }
                }
            }
        }
    }


    //Movement----------------------------

    void movement()
    {
        //Used for resetting jumps when grounded
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            currJumps = 0;
            playerVelocity.y = 0f;
        }

        //Player Movement
        if (characterList[currCharacter].ability == 0 && characterList[currCharacter].isUsingAbility == false)
        {
            characterList[currCharacter].currSpeed = characterList[currCharacter].speed;
        }
        else if (characterList[currCharacter].ability != 0)
        {
            characterList[currCharacter].currSpeed = characterList[currCharacter].speed;
        }


        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * characterList[currCharacter].currSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            characterList[currCharacter].callItemOnJump();
            currJumps++;
            playerVelocity.y = jumpHeight;
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.playerJump[Random.Range(0, sfxManager.instance.playerJump.Length)], sfxManager.instance.playerJumpVol);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
    }
    /*if(Time.time - lastUpdate >= 0.25f)
                            {
                                weaponInventory[currentWeapon].charge += 0.25f;
                                lastUpdate= Time.time;
                            }*/
    //Coroutines--------------------------

    public IEnumerator fire()
    {
        if (gameManager.instance.activeMenu == null)
        {
            if (weaponInventory[currentWeapon].magazineCurrent > 0)
            {
                if (weaponInventory[currentWeapon].chargeable)
                {

                    if (Input.GetButton("Fire1") && hasFired == false)
                    {
                        if (!hasChargePlayed)
                        {
                            //sfxManager.instance.aud.PlayOneShot(sfxManager.instance.railgunChargeSound[Random.Range(0, sfxManager.instance.railgunChargeSound.Length)], sfxManager.instance.railgunChargeVol);
                        }
                        if (characterList[currCharacter].ability != 1)
                        {
                            if (Time.time - lastUpdate >= 0.25f)
                            {
                                weaponInventory[currentWeapon].charge += 0.25f;
                                lastUpdate = Time.time;
                            }
                        }
                        else
                        {
                            if (Time.time - lastUpdate >= 0.025f)
                            {
                                weaponInventory[currentWeapon].charge += 0.25f;
                                lastUpdate = Time.time;
                            }
                        }
                        if (weaponInventory[currentWeapon].charge >= weaponInventory[currentWeapon].chargeTime)
                        {
                            Instantiate(weaponInventory[currentWeapon].actualWeaponProjectile, currentMuzzlePoint.transform.position, currentMuzzlePoint.transform.rotation);
                            if (weaponInventory[currentWeapon].flashFX != null)
                            {
                                Instantiate(weaponInventory[currentWeapon].flashFX, currentMuzzlePoint.transform.position, currentMuzzlePoint.transform.rotation);
                            }

                            playShootSound();

                            weaponInventory[currentWeapon].magazineCurrent -= 1;
                            hasFired = true;
                            if (characterList[currCharacter].ability == 1 && characterList[currCharacter].isUsingAbility)
                            {
                                yield return new WaitForSeconds((float)(weaponInventory[currentWeapon].shootRate / 10));
                            }
                            else
                            {
                                yield return new WaitForSeconds(weaponInventory[currentWeapon].shootRate);
                            }

                            isFiring = false;
                        }


                    }
                    else if (Input.GetButtonUp("Fire1"))
                    {
                        hasFired = false;
                        Debug.Log("Reset Charge");
                        weaponInventory[currentWeapon].charge = 0;
                    }


                }
                else
                {
                    Instantiate(weaponInventory[currentWeapon].actualWeaponProjectile.gameObject, currentMuzzlePoint.transform.position, currentMuzzlePoint.transform.rotation);
                    if (weaponInventory[currentWeapon].flashFX != null)
                    {
                        Instantiate(weaponInventory[currentWeapon].flashFX, currentMuzzlePoint.transform.position, currentMuzzlePoint.transform.rotation);
                    }

                    //TODO: CHANGE THIS VIA SHOOT ANIMATION EVENT
                    //animController.shootTrigger();
                    playShootSound();

                    weaponInventory[currentWeapon].magazineCurrent -= 1;
                    if (characterList[currCharacter].ability == 1 && characterList[currCharacter].isUsingAbility)
                    {
                        yield return new WaitForSeconds((float)(weaponInventory[currentWeapon].shootRate / 10));
                    }
                    else
                    {
                        yield return new WaitForSeconds(weaponInventory[currentWeapon].shootRate);
                    };
                    isFiring = false;
                }


            }
            else
            {
                sfxManager.instance.aud.PlayOneShot(weaponInventory[currentWeapon].emptySound[Random.Range(0, weaponInventory[currentWeapon].emptySound.Length)], weaponInventory[currentWeapon].emptyVol);
                yield return new WaitForSeconds(1);
                isFiring = false;
            }
        }
        yield return new WaitForSeconds(0.01f);
        isFiring = false;

    }

    private void playShootSound()
    {
        AudioClip clipToPlay = null;
        float shootVolume = 0;
        switch (currentWeaponType)
        {
            case weaponCreation.WeaponType.Pistol:
                clipToPlay = sfxManager.instance.pistolShootSound[Random.Range(0, sfxManager.instance.pistolShootSound.Length)];
                shootVolume = sfxManager.instance.pistolShootVol;
                break;
            case weaponCreation.WeaponType.Rifle:
                clipToPlay = sfxManager.instance.rifleShootSound[Random.Range(0, sfxManager.instance.rifleShootSound.Length)];
                shootVolume = sfxManager.instance.rifleShootVol;
                break;
            case weaponCreation.WeaponType.GrenadeLauncher:
                clipToPlay = sfxManager.instance.glShootSound[Random.Range(0, sfxManager.instance.glShootSound.Length)];
                shootVolume = sfxManager.instance.glShootVol;
                break;
            case weaponCreation.WeaponType.ArcGun:
                clipToPlay = sfxManager.instance.arcgunShootSound[Random.Range(0, sfxManager.instance.arcgunShootSound.Length)];
                shootVolume = sfxManager.instance.arcgunShootVol;
                break;
            case weaponCreation.WeaponType.RailGun:
                clipToPlay = sfxManager.instance.railgunShootSound[Random.Range(0, sfxManager.instance.railgunShootSound.Length)];
                shootVolume = sfxManager.instance.railgunShootVol;
                break;
            default:
                break;
        }
        sfxManager.instance.aud.PlayOneShot(clipToPlay, shootVolume);
    }

    private void playWeaponPickupSound()
    {
        AudioClip clipToPlay = null;
        float pickupVolume = 0;
        switch (currentWeaponType)
        {
            case weaponCreation.WeaponType.Pistol:
                clipToPlay = sfxManager.instance.pistolPickupSound[Random.Range(0, sfxManager.instance.pistolPickupSound.Length)];
                pickupVolume = sfxManager.instance.pistolShootVol;
                break;
            case weaponCreation.WeaponType.Rifle:
                clipToPlay = sfxManager.instance.riflePickupSound[Random.Range(0, sfxManager.instance.riflePickupSound.Length)];
                pickupVolume = sfxManager.instance.rifleShootVol;
                break;
            case weaponCreation.WeaponType.GrenadeLauncher:
                clipToPlay = sfxManager.instance.glPickupSound[Random.Range(0, sfxManager.instance.glPickupSound.Length)];
                pickupVolume = sfxManager.instance.glShootVol;
                break;
            case weaponCreation.WeaponType.ArcGun:
                clipToPlay = sfxManager.instance.arcgunPickupSound[Random.Range(0, sfxManager.instance.arcgunPickupSound.Length)];
                pickupVolume = sfxManager.instance.arcgunShootVol;
                break;
            case weaponCreation.WeaponType.RailGun:
                clipToPlay = sfxManager.instance.railgunPickupSound[Random.Range(0, sfxManager.instance.railgunPickupSound.Length)];
                pickupVolume = sfxManager.instance.railgunShootVol;
                break;
            default:
                break;
        }
        sfxManager.instance.aud.PlayOneShot(clipToPlay, pickupVolume);
    }

    private void playReloadSound()
    {
        AudioClip clipToPlay = null;
        float pickupVolume = 0;
        switch (currentWeaponType)
        {
            case weaponCreation.WeaponType.Pistol:
                clipToPlay = sfxManager.instance.pistolReloadSound[Random.Range(0, sfxManager.instance.pistolReloadSound.Length)];
                pickupVolume = sfxManager.instance.pistolShootVol;
                break;
            case weaponCreation.WeaponType.Rifle:
                clipToPlay = sfxManager.instance.rifleReloadSound[Random.Range(0, sfxManager.instance.rifleReloadSound.Length)];
                pickupVolume = sfxManager.instance.rifleShootVol;
                break;
            case weaponCreation.WeaponType.GrenadeLauncher:
                clipToPlay = sfxManager.instance.glReloadSound[Random.Range(0, sfxManager.instance.glReloadSound.Length)];
                pickupVolume = sfxManager.instance.glShootVol;
                break;
            case weaponCreation.WeaponType.ArcGun:
                clipToPlay = sfxManager.instance.arcgunReloadSound[Random.Range(0, sfxManager.instance.arcgunReloadSound.Length)];
                pickupVolume = sfxManager.instance.arcgunShootVol;
                break;
            case weaponCreation.WeaponType.RailGun:
                clipToPlay = sfxManager.instance.railgunReloadSound[Random.Range(0, sfxManager.instance.railgunReloadSound.Length)];
                pickupVolume = sfxManager.instance.railgunShootVol;
                break;
            default:
                break;
        }
        sfxManager.instance.aud.PlayOneShot(clipToPlay, pickupVolume);
    }

    public void useAbility()
    {
        if (Input.GetButtonDown("Ability"))
        {
            if (characterList[currCharacter].energy >= characterList[currCharacter].energyUseRate)
            {
                characterList[currCharacter].isUsingAbility = true;

                playerAbilities.useAbility();
            }
        }
        else if (Input.GetButtonUp("Ability"))
        {
            if (characterList[currCharacter].isUsingAbility)
            {
                animController.switchSprintingState(false);
                characterList[currCharacter].isUsingAbility = false;
                playerAbilities.useAbility();

                //This is only for character with a sprint
                isSprinting = false;
            }
        }
    }

    IEnumerator playDamageFX()
    {
        gameManager.instance.playerDamageFX.SetActive(true);
        yield return new WaitForSeconds(damageFXLength);
        gameManager.instance.playerDamageFX.SetActive(false);
    }

    IEnumerator shootAfterReloadDelay()
    {
        yield return new WaitForSeconds(weaponInventory[currentWeapon].shootAfterReloadTime);
        isReloading = false;
    }

    //Statistic Modification--------------

    public void takeDamage(int dmg)
    {
        characterList[currCharacter].HP -= dmg;
        int playCheck = Random.Range(0, 2);
        if (playCheck == 0)
        {
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.playerHurt[Random.Range(0, sfxManager.instance.playerHurt.Length)], sfxManager.instance.playerHurtVol);
        }

        StartCoroutine(playDamageFX());
        gameManager.instance.updatePlayerHPBar();

        // Player death
        if (characterList[currCharacter].HP <= 0)
        {
            gameManager.instance.pause();
            gameManager.instance.SetActiveMenu(gameManager.UIMENUS.deathMenu);
        }
    }

    public void setPlayerPos()
    {
        //controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPoint.transform.position;
        //controller.enabled = true;
    }

    public void addPlayerHP(float amount)
    {
        characterList[currCharacter].HP += amount;

        if (characterList[currCharacter].HP > MAXHP)
        {
            characterList[currCharacter].HP = MAXHP;
        }
    }

    public void resetPlayerHP()
    {
        characterList[currCharacter].HP = MAXHP;
    }

    public void addPlayerEnergy(float amount)
    {
        characterList[currCharacter].energy += amount;

        if (characterList[currCharacter].energy > characterList[currCharacter].energyMax)
        {
            characterList[currCharacter].energy = characterList[currCharacter].energyMax;
        }
    }

    public void resetPlayerEnergy()
    {
        characterList[currCharacter].energy = characterList[currCharacter].energyMax;
    }

    public void weaponPickUp(weaponCreation weapon)
    {
        // Shows reticle if not already showing
        gameManager.instance.showReticle();

        // Turn on weapon UI 
        if (!gameManager.instance.currentWeaponUI.activeSelf)
        {
            gameManager.instance.currentWeaponUI.SetActive(true);
        }

        // Update weapon inventory
        for (int i = 0; i < maxSlots; i++)
        {
            // If we already have this weapon, dont duplicate
            if (weaponInventory[i] == weapon)
            {
                Debug.Log("already had weapon");
                weaponInventory[currentWeapon].currentAmmoPool += weaponInventory[currentWeapon].maxAmmoPool / 5;
                playWeaponPickupSound();
                break;
            }

            // First time picking up weapon, "Add" weapon to weapon inventory and select the weapon
            else if (weaponInventory[i] == null)
            {
                // Set weapon inventory slot[i] to this weapon
                weaponInventory[i] = weapon;
                gameManager.instance.slots[i].SetActive(true);
                currentWeapon = i;

                // Set inventory slot icon to this weapoons icon
                gameManager.instance.setWeaponIcon(weapon, i);

                // Select current ammo
                weaponInventory[currentWeapon].currentAmmoPool = weaponInventory[currentWeapon].maxAmmoPool / 2;
                weaponInventory[currentWeapon].magazineCurrent = weaponInventory[currentWeapon].magazineMax;

                // Select weapon
                pickupWeapon(weapon);

                break;
            }
        }
    }

    public void perkPickUp(perkCreation perk)
    {

    }

    void pickupWeapon(weaponCreation weapon)
    {
        // Deactivate current weapon model game object if already have a weapon
        if (currentWeaponModel != null)
            currentWeaponModel.SetActive(false);

        //TODO: MAY NEED ADJUSTING HERE WHEN ADDING UNEQUIP AND EQUIP ANIMATIONS
        // Switch to the appropriate animation state

        animController.switchAnimState(currentWeaponType, weapon.weaponType);
        currentWeaponType = weapon.weaponType;      // Stored for animations

        // Select appropriate weapon model and weapon muzzle point
        currentWeaponModel = weaponModelList[(int)weapon.weaponType];
        currentWeaponModel.SetActive(true);
        currentMuzzlePoint = muzzlePointList[(int)weapon.weaponType];

        if (weapon.chargeable)
        {
            weapon.charge = 0;
            weapon.currentAmmoPool = 1;
        }

        // Play audio
        playWeaponPickupSound();

    }

    public void selectWeapon(weaponCreation weapon)
    {
        // Deactivate current weapon model game object
        if (currentWeaponModel != null)
            currentWeaponModel.SetActive(false);

        //TODO: MAY NEED ADJUSTING HERE WHEN ADDING UNEQUIP AND EQUIP ANIMATIONS
        // Switch to the appropriate animation state
        if (currentWeaponType != weaponCreation.WeaponType.Unequipped)
        {
            animController.switchAnimState(currentWeaponType, weapon.weaponType);
            currentWeaponType = weapon.weaponType;      // Stored for animations

            // Select appropriate weapon model and weapon muzzle point
            currentWeaponModel = weaponModelList[(int)weapon.weaponType];
            currentWeaponModel.SetActive(true);
            currentMuzzlePoint = muzzlePointList[(int)weapon.weaponType];

            // Play audio
            playWeaponPickupSound();
        }
    }

    // Setters/Getters

    public float getHP()
    {
        return characterList[currCharacter].HP;
    }

    public float getMAXHP()
    {
        return characterList[currCharacter].HPMax;
    }

    public float getEnergy()
    {
        return characterList[currCharacter].energy;
    }

    public float getMAXEnergy()
    {
        return characterList[currCharacter].energyMax;
    }

    /*private void rigidBodyMove()
    {
        if(Input.GetButton("Sprint"))
        {
                Vector3 MoveVectorSprint = transform.TransformDirection(playerMovementInput) * (walkSpeed * sprintMultiplier);
                playerRB.velocity = new Vector3(MoveVectorSprint.x, playerRB.velocity.y, MoveVectorSprint.z);
        }
        else
        {
                Vector3 MoveVector = transform.TransformDirection(playerMovementInput) * walkSpeed;
                playerRB.velocity = new Vector3(MoveVector.x, playerRB.velocity.y, MoveVector.z);
        }

        if(Input.GetButtonDown("Jump"))
        {
            if(playerRB.velocity.y == 0)
            {
                playerRB.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
            
        }
    }*/

    public void pushBackInput(Vector3 dir)
    {
        pushBack = dir;
    }

    IEnumerator playSteps()
    {
        stepIsPlaying = true;
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.playerFootstep[Random.Range(0, sfxManager.instance.playerFootstep.Length)], sfxManager.instance.playerFootstepVol);

        if (isSprinting)
            yield return new WaitForSeconds(0.3f);
        else
            yield return new WaitForSeconds(0.5f);

        stepIsPlaying = false;
    }

    public void reload()
    {
        int reloadAmount = weaponInventory[currentWeapon].magazineMax - weaponInventory[currentWeapon].magazineCurrent;

        if (weaponInventory[currentWeapon].currentAmmoPool >= reloadAmount && weaponInventory[currentWeapon].magazineCurrent != weaponInventory[currentWeapon].magazineMax)
        {
            weaponInventory[currentWeapon].currentAmmoPool -= reloadAmount;
            weaponInventory[currentWeapon].magazineCurrent += reloadAmount;
            if (weaponInventory[currentWeapon].chargeable)
            {
                weaponInventory[currentWeapon].currentAmmoPool = 1;
                weaponInventory[currentWeapon].charge = 0;
                hasFired = false;
            }
        }
        else if (weaponInventory[currentWeapon].currentAmmoPool > 0 && weaponInventory[currentWeapon].currentAmmoPool < reloadAmount)
        {
            reloadAmount = weaponInventory[currentWeapon].currentAmmoPool;
            weaponInventory[currentWeapon].currentAmmoPool -= reloadAmount;
            weaponInventory[currentWeapon].magazineCurrent += reloadAmount;
        }

        //Play audio 
        playReloadSound();
        StartCoroutine(shootAfterReloadDelay());
    }

    public void energyRecharge()
    {
        if (characterList[currCharacter].rechargable && !characterList[currCharacter].isUsingAbility)
        {
            if (Time.time - lastUpdate >= 0.25f)
            {
                if (characterList[currCharacter].energy < characterList[currCharacter].energyMax)
                {
                    characterList[currCharacter].energy += 5;
                    lastUpdate = Time.time;
                    gameManager.instance.updatePlayerEnergyBar();
                    if (characterList[currCharacter].energy > characterList[currCharacter].energyMax)
                    {
                        characterList[currCharacter].energy = characterList[currCharacter].energyMax;
                    }
                }

            }
        }
    }
}
