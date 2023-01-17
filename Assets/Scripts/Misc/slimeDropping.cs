using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeDropping : MonoBehaviour
{
    /*
     * NOTE: This script assumes that this game objects' X and Z scale components are identical.
     */

    // Scaling up
    [SerializeField] bool spawnExtraLarge;

    [SerializeField] float xzScaleIncreaseRate;
    [SerializeField] float xzScaleCurrent;
    [SerializeField] bool isScaling;
    [SerializeField] bool hasReachedFullScale;

    // Scaling down - After finished lerping, there begins a delay to start disappearing, which will trigger it to lerp back down to nothing
    [SerializeField] bool isDisappearing;
    [SerializeField] bool hasDisappeared;
    [SerializeField] int timeUntilDisappear;

    Vector3 fullSizeScale;
    Vector3 shrunkenScale;

    private void Start()
    {
        // Remember where to stop when scaling up -- adding in a random offset on just the x and z components
        if(spawnExtraLarge)
        {
            fullSizeScale = transform.localScale + new Vector3(Random.Range(0.8f, 1.5f), 0, Random.Range(0.8f, 1.5f));
        }
        else
        {
            fullSizeScale = transform.localScale + new Vector3(Random.Range(0, 0.5f), 0, Random.Range(0, 0.5f));
        }

        // Grab the starting out scale, keepiing the y scale the same as actual prefab
        shrunkenScale = new Vector3(0f, transform.localScale.y, 0f);
        xzScaleCurrent = 0f;

        // Set scale to shrunken
        transform.localScale = shrunkenScale;

        // Begin lerping the x and z components to the prefabs actual scale (fullSizeScale)
        isScaling = true;
    }

    private void Update()
    {
        // Stop lerping the scale when has reached full size
        if(isScaling && !hasReachedFullScale)
        {
            xzScaleCurrent = Mathf.Lerp(xzScaleCurrent, fullSizeScale.x, Time.deltaTime * xzScaleIncreaseRate);

            // Now set both the x and z scale components to this lerped value
            transform.localScale = new Vector3(xzScaleCurrent, transform.localScale.y, xzScaleCurrent);

            // Get out of code block - stop lerping and trigger the disappear
            if (xzScaleCurrent >= fullSizeScale.x - 0.2f)
            {
                hasReachedFullScale = true;
                StartCoroutine(waitToDisappear());
            }
        }

        // Once coroutine finishes, begin scaling down
        else if(isDisappearing && !hasDisappeared)
        {
            // NOTE: Here just re-usiing fullSizeScale since we no longer need to remember this
            fullSizeScale.x = Mathf.Lerp(fullSizeScale.x, 0, Time.deltaTime * xzScaleIncreaseRate);
            fullSizeScale.y = Mathf.Lerp(fullSizeScale.y, 0, Time.deltaTime * xzScaleIncreaseRate);
            fullSizeScale.z = Mathf.Lerp(fullSizeScale.z, 0, Time.deltaTime * xzScaleIncreaseRate);

            // Update the scale
            transform.localScale = fullSizeScale;

            // Get out of code block - stop lerping and trigger destroy
            if (transform.localScale.x <= 0.02f)
            {
                hasDisappeared = true;
                Destroy(gameObject, 2f);
            }
        }
    }

    IEnumerator waitToDisappear()
    {
        yield return new WaitForSeconds(timeUntilDisappear);
        isDisappearing = true;

    }
}
