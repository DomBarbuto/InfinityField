using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsSlideUp : MonoBehaviour
{
    public void startRollingCredits()
    {
        GetComponent<Animator>().SetTrigger("TriggerCreditsSlide");
    }

    // For at end of animation. Can also be cut short if back button is pressed (mainmenuButtonFunctions script)
    public void animEvent_Hide()
    {
        stopRollingCredits();
    }

    public void stopRollingCredits()
    {
        gameObject.SetActive(false);
    }
}
