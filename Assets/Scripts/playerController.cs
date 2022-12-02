using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("---- Controller ----")]
    [SerializeField] CharacterController controller;

    [Header("---- Player Stats ----")]
    [SerializeField] int HP;
    [Range(3, 8)] [SerializeField] float playerSpeed;
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
    }

    // Update is called once per frame

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
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
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);

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
        yield return new WaitForSeconds(currFireRate);
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

    public void setPlayerSpeed()
    { }

    public void addPlayerSpeed()
    {

    }
}
