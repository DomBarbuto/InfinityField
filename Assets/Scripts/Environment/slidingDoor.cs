using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour, IInteractable
{
    [SerializeField] Transform endPos;
    [SerializeField] float doorSpeed;
    [SerializeField] AudioSource aud;
    [SerializeField] Transform startPos;
    [SerializeField]RoomEntry roomEntry;
    bool openDoor = false;
    public bool HasClosed = false;
    [SerializeField] GameObject interactCanvas;
    

    void OperateDoor()
    {
        StopAllCoroutines();
        if (!openDoor)
        {
            StartCoroutine(MoveDoor(endPos.position));
            roomEntry.playerEnterRoom();
        }
        else
        {
            StartCoroutine(MoveDoor(startPos.position));
        }
        openDoor = !openDoor;
    }
    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while (timeElapsed < doorSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / doorSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aud.Play();
            OperateDoor();
            interactCanvas.SetActive(false);
            HasClosed = true;
        }
    }

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        aud.Play();
    //        OperateDoor();
    //    }
    //}

    public void interact()
    {
        aud.Play();
        OperateDoor();
        interactCanvas.SetActive(false);
    }

    public void showText()
    {
        interactCanvas.SetActive(true);
    }

   
}
