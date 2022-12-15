using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("---- Controller ----")]
    [SerializeField] CharacterController controller;

    [Header("---- Player Stats ----")]
    [SerializeField] float HP;
    [SerializeField] float energy;
    [SerializeField] float energyDecreaseRate;

    [Header("---- Player Movement ----")]
    [SerializeField] bool isSprinting;
    [SerializeField] float currentMoveSpeed;
    [Range(3, 8)] [SerializeField] float walkSpeed;
    [Range(1, 4)][SerializeField] float sprintMultiplier;
    [Range(10, 15)] [SerializeField] float jumpHeight;
    [Range(15, 35)] [SerializeField] float gravityValue;
    [Range(1, 3)] [SerializeField] int jumpsMax;
    [SerializeField] float damageFXLength;
    [SerializeField] public Vector3 pushBack;
    [SerializeField] float pushBackTime;

    [Header("Inventory")]
    [SerializeField] public List<weaponCreation> weaponInventory = new List<weaponCreation>();
    [SerializeField] int maxSlots = 5;                      //The max amount of weapons the player can have
    [SerializeField] Transform laserPistolMuzzlePoint;
    [SerializeField] Transform laserRifleMuzzlePoint;
    [SerializeField] Transform grenadeLauncherMuzzlePoint;

    [Header("---- Active Weapon -----")]
    [SerializeField] public int currentWeapon;
    [SerializeField] public GameObject weaponOBJ;
    public Transform currMuzzlePoint;

    [Header("---- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)][SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerJump;
    [Range(0, 1)][SerializeField] float playerJumpVol;
    [SerializeField] AudioClip[] playerFootstep;
    [Range(0, 1)][SerializeField] float playerFootstepVol;

    /*[Header("---- RigidBodyMovement ----")]
    [SerializeField] private Rigidbody playerRB;
    private Vector3 playerMovementInput;
    [SerializeField] public Transform onGround;*/

    //Private Variables------------------
    bool isFiring;
    bool stepIsPlaying;
    int currJumps;  //Times jumped since being grounded
    float MAXHP;      //Player's maximum health
    float MAXEnergy;  //Player's maximum energy

    Vector3 playerVelocity;
    Vector3 move;
    //------------------------------------

       //------- Functions --------

    // Start is called before the first frame update

    void Start()
    {
        // Set orginal stats
        MAXHP = HP;
        MAXEnergy = energy;

        setPlayerPos();
        currentMoveSpeed = walkSpeed;
    }

    // Update is called once per frame

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);

            pushBack.x = Mathf.Lerp(pushBack.x, 0f, Time.deltaTime * pushBackTime);
            pushBack.y = Mathf.Lerp(pushBack.y, 0f, Time.deltaTime * (pushBackTime));
            pushBack.z = Mathf.Lerp(pushBack.z, 0f, Time.deltaTime * pushBackTime);
            movement();

            if (!stepIsPlaying && move.magnitude > 0.3f && controller.isGrounded)
                StartCoroutine(playSteps());

            if (!isFiring && Input.GetButton("Fire1"))
            {
                if (weaponInventory[currentWeapon] != null)
                {
                    isFiring = true;
                    StartCoroutine(fire());
                    
                }
            }
            gameManager.instance.MagazineCurrent.text = weaponInventory[currentWeapon].magazineCurrent.ToString();
            gameManager.instance.AmmoPoolCurrent.text = weaponInventory[currentWeapon].currentAmmoPool.ToString();
            gameManager.instance.currentWeaponIcon.GetComponent<Image>().sprite = weaponInventory[currentWeapon].icon;

            if (Input.GetButtonDown("Reload"))
            {
                reload();
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

        // Handle sprinting - Using GetButtonDown because of the lerp while holding
        if (controller.isGrounded && Input.GetButton("Sprint"))
        {
            // Only call if not sprinting
            if (!isSprinting)
            {
                currentMoveSpeed = walkSpeed * sprintMultiplier;
                isSprinting = true;
            }

            // Update energy and UI energy bar
            energy -= Time.deltaTime * energyDecreaseRate;
            Debug.Log(energy);
            gameManager.instance.updatePlayerEnergyBar();
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            currentMoveSpeed = walkSpeed;
            isSprinting = false;
        }

        //Player Movement
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * currentMoveSpeed);

        if (Input.GetButtonDown("Jump") && currJumps < jumpsMax)
        {
            currJumps++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(playerJump[Random.Range(0, playerJump.Length)], playerJumpVol);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
    }

    //Coroutines--------------------------

    IEnumerator fire()
    {
        
        RaycastHit hit;
        Debug.Log(gameManager.instance.activeMenu);
        if (gameManager.instance.activeMenu == null)
        {
            if (weaponInventory[currentWeapon].magazineCurrent > 0)
            {
                // For grenade launcher
                if (weaponInventory[currentWeapon].isThrowable)
                {
                    Instantiate(weaponInventory[currentWeapon].thrownObject, currMuzzlePoint.transform.position, currMuzzlePoint.transform.rotation);
                }
                // For every other weapon that does raycasting
                else
                {
                    // Creates muzzle flash effect
                    Instantiate(weaponInventory[currentWeapon].flashFX, currMuzzlePoint.transform.position, currMuzzlePoint.transform.rotation);
                    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, weaponInventory[currentWeapon].shootDistance))
                    {
                        if (hit.collider.GetComponent<IDamage>() != null)
                        {
                            hit.collider.GetComponent<IDamage>().takeDamage(weaponInventory[currentWeapon].weaponDamage);
                        }
                    }
                    // Creates impact effect
                    Instantiate(weaponInventory[currentWeapon].hitFX, hit.point, transform.rotation);
                }
                weaponInventory[currentWeapon].magazineCurrent -= 1;
            }
            else
            {
                //Add reload click if no ammo in magazine
            }
        }
            yield return new WaitForSeconds(weaponInventory[currentWeapon].shootRate);
            isFiring = false;

    }

    IEnumerator playDamageFX()
    {
        gameManager.instance.playerDamageFX.SetActive(true);
        yield return new WaitForSeconds(damageFXLength);
        gameManager.instance.playerDamageFX.SetActive(false);
    }

    //Statistic Modification--------------

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        aud.PlayOneShot(playerHurt[Random.Range(0, playerHurt.Length)], playerHurtVol);
        StartCoroutine(playDamageFX());
        gameManager.instance.updatePlayerHPBar();
        
        // Player death
        if (HP <= 0)
        {
            gameManager.instance.pause();
            gameManager.instance.SetActiveMenu(gameManager.UIMENUS.deathMenu);
        }
    }

    public void addJump(int amount)
    {

    }

    public void setPlayerPos()
    {
        //controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPoint.transform.position;
        //controller.enabled = true;
    }

    public void addPlayerHP(float amount)
    {
        HP += amount;

        if(HP > MAXHP)
        {
            HP = MAXHP;
        }
    }

    public void resetPlayerHP()
    {
        HP = MAXHP;
    }

    public void addPlayerEnergy(float amount)
    {
        energy += amount;

        if(energy > MAXEnergy)
        {
            energy = MAXEnergy;
        }
    }

    public void resetPlayerEnergy()
    {
        energy = MAXEnergy;
    }

    public void weaponPickUp(weaponCreation weapon)
    {
        // Shows reticle if not already showing
        gameManager.instance.showReticle();

        // Update weapon inventory
        for (int i = 0; i < maxSlots; i++)
        {
            // If we already have this weapon, dont duplicate
            if (weaponInventory[i] == weapon)
            {
                Debug.Log("already had weapon");
                // TODO: Grab ammo from weapon
                // TODO: Play ammo pickup audio clip
                break;
            }

            // First time picking up weapon, "Add" weapon to weapon inventory and select the weapon
            else if (weaponInventory[i] == null)
            {
                Debug.Log("add weapon to inventory");

                // Set weapon inventory slot[i] to this weapon
                weaponInventory[i] = weapon;

                // Transfer mesh and material 
                //Here, doing getComponentInChildren!!!
                currentWeapon = i;
                weaponOBJ.GetComponent<MeshFilter>().sharedMesh = weaponInventory[currentWeapon].weaponsModel.GetComponentInChildren<MeshFilter>().sharedMesh;
                weaponOBJ.GetComponent<MeshRenderer>().sharedMaterial = weaponInventory[currentWeapon].weaponsModel.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                // Transfer stats to weapon
                /*                currWeaponDamage = weapon.weaponDamage;
                                currShootRate = weapon.shootRate;
                                currShootDistance = weapon.shootDistance;*/
                gameManager.instance.slots[i].GetComponent<Image>().sprite = weapon.icon;
                // Select currMuzzlePoint
                switch (weapon.weaponMuzzleType)
                {
                    case weaponCreation.WeaponType.pistol:
                        currMuzzlePoint = laserPistolMuzzlePoint;
                        break;

                    case weaponCreation.WeaponType.rifle:
                        currMuzzlePoint = laserRifleMuzzlePoint;
                        break;

                    case weaponCreation.WeaponType.grenadeLauncher:
                        currMuzzlePoint = grenadeLauncherMuzzlePoint;
                        break;
                    default:
                        break;
                }

                break;
            }
        }
        
    }

    // This isn't being called from anywhere?
    public void openInventory()
    {
        gameManager.instance.pause();

    }

    // Setters/Getters

    public float getHP()
    {
        return HP;
    }

    public float getMAXHP()
    {
        return MAXHP;
    }

    public float getEnergy()
    {
        return energy;
    }

    public float getMAXEnergy()
    {
        return MAXEnergy;
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
        aud.PlayOneShot(playerFootstep[Random.Range(0, playerFootstep.Length)], playerFootstepVol);

        if (isSprinting)
            yield return new WaitForSeconds(0.3f);
        else
            yield return new WaitForSeconds(0.5f);

        stepIsPlaying = false;
    }

    public void reload()
    {
        int reloadAmount = weaponInventory[currentWeapon].magazineMax - weaponInventory[currentWeapon].magazineCurrent;

        if (weaponInventory[currentWeapon].currentAmmoPool > reloadAmount)
        {
            weaponInventory[currentWeapon].currentAmmoPool -= reloadAmount;
            weaponInventory[currentWeapon].magazineCurrent += reloadAmount;
        }
        else if (weaponInventory[currentWeapon].currentAmmoPool > 0 && weaponInventory[currentWeapon].currentAmmoPool < reloadAmount)
        {
            reloadAmount = weaponInventory[currentWeapon].currentAmmoPool;
            weaponInventory[currentWeapon].currentAmmoPool -= reloadAmount;
            weaponInventory[currentWeapon].magazineCurrent += reloadAmount;
        }

        if (weaponInventory[currentWeapon].weaponMuzzleType == weaponCreation.WeaponType.pistol)
        {
            //add OneShotAudio
        }
        else if(weaponInventory[currentWeapon].weaponMuzzleType == weaponCreation.WeaponType.rifle)
        {
            //add OneShotAudio
        }
        else if (weaponInventory[currentWeapon].weaponMuzzleType == weaponCreation.WeaponType.grenadeLauncher)
        {
            //add OneShotAudio
        }
    }
}
