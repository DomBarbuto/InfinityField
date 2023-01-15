using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSection : MonoBehaviour
{
    // NOTE: The ground floor underneath the platforms' LAYER must be set to the PlatformUnderneath
    // NOTE: The platform object themselves' LAYER must be set to Platform
    /*
     * List of platform game object prefabs 
     */
    [Tooltip("Ensure that each of these objects have a rigidbody set to kinematic.")]
    [SerializeField] GameObject[] platformObjects;
    [SerializeField] int timeUntilDestroy;

    [Header("For Dropping Sequentially")]
    [SerializeField] bool currentlyDropping;
    [Range(0.2f, 2.0f)][SerializeField] float minDropInterval;
    [Range(0.2f, 2.0f)][SerializeField] float maxDropInterval;

    // This will turn on physics for all platforms and turn off their collider 
    // so that they just fall through the floor and eventually destroy.
    public void DropPlatformsAtOnce()
    {
        foreach(GameObject platform in platformObjects)
        {
            platform.GetComponent<Collider>().enabled = false;
            platform.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void DropPlatformsSequentially(float randTimeBetweenDrops)
    {
        // Get a random time interval between each drop
        float randInterval = Random.Range(minDropInterval, maxDropInterval);

        // Go through each platform, firing the drop coroutine one after the other
        int i = 0;
        while(i < platformObjects.Length)
        {
            if(!currentlyDropping)
            {
                currentlyDropping = true;
                StartCoroutine(DropSinglePlatform(i , randInterval));
                i++;
            }
        }
    }

    IEnumerator DropSinglePlatform(int platformIndex, float randTimeBetweenDrops)
    {
        // Turn off collider and turn on physics for the platform object
        platformObjects[platformIndex].GetComponent<Collider>().enabled = false;
        platformObjects[platformIndex].GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(randTimeBetweenDrops);
        currentlyDropping = false;
    }

    IEnumerator destroyTimer()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        foreach (GameObject platform in platformObjects)
        {
            Destroy(platform);
        }
    }



}
