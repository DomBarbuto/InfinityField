using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour
{
    [SerializeField] Transform endPos;
    [SerializeField] float doorSpeed;
    [SerializeField] AudioSource aud;

    bool openDoor;

    public void Update()
    {
        if (openDoor)
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPos.position, doorSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aud.Play();
            openDoor = true;
        }
    }
}
