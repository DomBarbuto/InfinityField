using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeOutAnimEventChild : MonoBehaviour
{
    [SerializeField] mainMenuButtonFunctions menuButtons;

    public void animEventHelper_callLoadNextLevel()
    {
        menuButtons.animEvent_LoadNextLevel();
    }
}
