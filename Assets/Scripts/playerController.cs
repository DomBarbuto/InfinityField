using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("---- Controller ----")]
    [SerializeField] CharacterController controller;

    [Header("---- Player Stats ----")]
    [SerializeField] int HP;
    [SerializeField] float currentMoveSpeed;
    [Range(3, 8)] [SerializeField] float walkSpeed;
    [Range(1, 4)][SerializeField] float sprintMultiplier;
    [Range(10, 15)] [SerializeField] float jumpHeight;
    [Range(15, 35)] [SerializeField] float gravityValue;
    [Range(1, 3)] [SerializeField] int jumpsMax;
    [SerializeField] float damageFXLength;

    [Header("---- Active Weapon -----")]
    [SerializeField] int currDamage;
    [SerializeField] float currFireRate;
    [SerializeField] int currFireRange;

    //Private Variables------------------
    bool isFiring;

    int currJumps; //Times jumped since being grounded
    int MAXHP; //Player's maximum health

    Vector3 playerVelocity;
    Vector3 move;
    //------------------------------------

       //------- Functions --------

    // Start is called before the first frame update

    void Start()
    {
        MAXHP = HP;
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

        // Handle sprinting - TODO : 
        if (controller.isGrounded && Input.GetButton("Sprint"))
        {
            currentMoveSpeed = walkSpeed * sprintMultiplier;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            currentMoveSpeed = walkSpeed;
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
        yield return new WaitForSeconds(damageFXLength);
    }

    //Statistic Modification--------------

    public void takeDamage(int dmg)
    {

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


    }
}
