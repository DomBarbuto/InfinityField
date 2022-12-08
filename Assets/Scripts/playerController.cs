using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("---- Controller ----")]
    [SerializeField] CharacterController controller;

    [Header("---- Player Stats ----")]
    [SerializeField] int HP;
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

    [Header("Inventory")]
    [SerializeField] public List<weaponCreation> weaponInventory = new List<weaponCreation>();
    [SerializeField] int maxSlots = 8;     //The max amount of weapons the player can have

    [Header("---- Active Weapon -----")]
    [SerializeField] int currDamage;
    [SerializeField] float currFireRate;
    [SerializeField] int currFireRange;
    [SerializeField] GameObject weaponModel;

    //Private Variables------------------
    bool isFiring;

    int currJumps;  //Times jumped since being grounded
    int MAXHP;      //Player's maximum health
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
            movement();

            if (!isFiring && Input.GetButton("Fire1"))
            {
                isFiring = true;
                StartCoroutine(fire());
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
                Debug.Log("Poop");
                currentMoveSpeed = walkSpeed * sprintMultiplier;
                isSprinting = true;
            }

            // Update energy and UI energy bar
            energy -= Time.deltaTime * (energy * (1 / energyDecreaseRate));
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
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    //Coroutines--------------------------

    IEnumerator fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, currFireRange))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.Log("Shot " + hit.collider.name);
                // TODO: Delete
                Debug.Log("Player hit " + hit.collider.name);

                hit.collider.GetComponent<IDamage>().takeDamage(currDamage);
            }
            else
                // TODO: Delete
                Debug.Log("Miss");
        }

        yield return new WaitForSeconds(currFireRate);

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
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPoint.transform.position;
        controller.enabled = true;
    }

    public void resetPlayerHP()
    {
        HP = MAXHP;
    }

    public void weaponPickUp(weaponCreation weapon)
    {
        weaponModel.GetComponent<MeshFilter>().sharedMesh = weapon.weaponsModel.GetComponent<MeshFilter>().sharedMesh;

        for(int i = 0; i < maxSlots; i++)
        {
            if (weaponInventory[i] == weapon)
            {
                break;
            }
            else if(i == weaponInventory.Count && weaponInventory[i] != weapon)
            {
                weaponInventory.Add(weapon);

            }
        }
        
    }

    public void openInventory()
    {
        gameManager.instance.pause();


    }

    // Setters/Getters

    public int getHP()
    {
        return HP;
    }

    public int getMAXHP()
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
}
