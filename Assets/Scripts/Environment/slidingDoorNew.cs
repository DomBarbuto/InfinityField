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

        // Turn on backside collider
        beforeCollider.enabled = true;
    }
}
