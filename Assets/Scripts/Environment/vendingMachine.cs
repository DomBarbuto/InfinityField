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

    public void interact()
    {
        StartCoroutine(dispense());
    }

    public void showText()
    {
        interactCanvas.SetActive(true);
    }

    IEnumerator dispense()
    {
        canPurchase = false;
        if (gameManager.instance.credits >= cost)
        {
            gameManager.instance.credits -= cost;
            gameManager.instance.updateCreditUI();

            Instantiate(collectable, trayPos.position, collectable.transform.rotation);
            speaker.PlayOneShot(jingle);
            yield return new WaitForSeconds(delay);
            canPurchase = true;
        }
        else
        {
            speaker.PlayOneShot(failSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideText();
        }
    }
    

    public void HideText()
    {
        interactCanvas.SetActive(false);
    }
}
