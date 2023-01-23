using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossButton : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactCanvas;
    [SerializeField] public bool isButtonAllowed;
    [SerializeField] public Light redLight;

    private BossAdvancedSpecimen bossScript;


    private void Start()
    {
        redLight.enabled = true;

        // Connect to the specimen boss script and increment its button count
        bossScript = GameObject.FindGameObjectWithTag("BossSpecimen").GetComponent<BossAdvancedSpecimen>();
        bossScript.numberOfButtons++;

        // We don't want the player to be able to interact with this button if it's already been pushed IN THIS state of the fight
        isButtonAllowed = true;
    }

    public void interact()
    {
        // Button is only allowed interaction if it has yet to be hit in the current boss state
        if (!isButtonAllowed)
            return;

        // Call to boss script and update number of buttons hit
        isButtonAllowed = false;
        HideText();
        redLight.enabled = false;
        bossScript.incrementButtonsHit();

    }

    // Interact canvas is hidden when player leaves 
    public void HideText()
    {
        interactCanvas.SetActive(false);
    }


    public void showText()
    {
        if(isButtonAllowed)
            interactCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isButtonAllowed && other.gameObject.CompareTag("Player"))
        {
            HideText();
        }
    }
}
