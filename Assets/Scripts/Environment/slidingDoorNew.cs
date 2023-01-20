using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoorNew : MonoBehaviour, IInteractable, IRoomEntryListener
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject interactCanvas;
    private bool isClosed = true;
    [SerializeField] Collider beforeCollider;


    private void Start()
    {
        HideText();
        beforeCollider.enabled = false;
    }

    public void interact()
    {
        if(isClosed)
        {
            anim.SetTrigger("Open");
            sfxManager.instance.aud.PlayOneShot(sfxManager.instance.doorOpen[Random.Range(0, sfxManager.instance.doorOpen.Length)], sfxManager.instance.doorOpenVolMulti);
            HideText();
        }
    }

    public void showText()
    {
        interactCanvas.SetActive(true);
    }

    public void HideText()
    {
        interactCanvas.SetActive(false);
    }

    public void notify()
    {
        anim.SetTrigger("Close");
        sfxManager.instance.aud.PlayOneShot(sfxManager.instance.doorClose[Random.Range(0, sfxManager.instance.doorClose.Length)], sfxManager.instance.doorCloseVolMulti);
        // Turn on backside collider
        beforeCollider.enabled = true;
    }
}
