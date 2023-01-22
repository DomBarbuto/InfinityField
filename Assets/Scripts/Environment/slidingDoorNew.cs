using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoorNew : MonoBehaviour, IInteractable, IRoomEntryListener
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject interactCanvas;
    private bool isClosed = true;
    [SerializeField] Collider beforeCollider;
    [SerializeField] AudioSource aud;


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
            aud.PlayOneShot(sfxManager.instance.doorOpen[Random.Range(0, sfxManager.instance.doorOpen.Length)]);
            HideText();
        }
    }

    public void showText()
    {
        interactCanvas.SetActive(true);
        StartCoroutine(waitTillHide());
    }

    public void HideText()
    {
        interactCanvas.SetActive(false);
    }

    public void notify()
    {
        anim.SetTrigger("Close");
        aud.PlayOneShot(sfxManager.instance.doorClose[Random.Range(0, sfxManager.instance.doorClose.Length)]);
        // Turn on backside collider
        beforeCollider.enabled = true;
    }

    IEnumerator waitTillHide()
    {
        yield return new WaitForSeconds(2.5f);
        HideText();
    }
}
