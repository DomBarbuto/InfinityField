using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vendingMachine : MonoBehaviour , IInteractable
{
    [SerializeField] GameObject collectable;
    [SerializeField] AudioSource speaker;
    [SerializeField] AudioClip jingle;
    [SerializeField] AudioClip failSound;
    [SerializeField] int cost;
    [SerializeField] Transform trayPos;
    [SerializeField] float delay;
    [SerializeField] GameObject interactCanvas;

    bool canPurchase = true;



    // Start is called before the first frame update

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player") && Input.GetButtonDown("Interact") && gameManager.instance.credits >= cost && canPurchase)
    //    {
    //        StartCoroutine(dispense());
    //    }
    //    else if (other.CompareTag("Player") && Input.GetButtonDown("Interact"))
    //    {
    //        speaker.PlayOneShot(failSound);
    //    }
    //}

    public void interact()
    {
        StartCoroutine(dispense());
    }

    public void showText()
    {
        interactCanvas.SetActive(true);
        Debug.Log("vending");
    }

    IEnumerator dispense()
    {
        canPurchase = false;
        if (gameManager.instance.credits >= cost)
        {
            gameManager.instance.credits -= cost;
            gameManager.instance.updateCreditUI();
            Instantiate(collectable, trayPos.position, transform.rotation);
            speaker.PlayOneShot(jingle);
            yield return new WaitForSeconds(delay);
            canPurchase = true;
        }
    }
}
