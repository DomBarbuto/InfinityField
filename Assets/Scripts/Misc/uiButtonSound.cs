using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class uiButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler
{
    [SerializeField] mainMenuButtonFunctions mainMenuBtnFunct;
    [SerializeField] AudioSource mainMenuButtonFunAud;

    public void OnPointerEnter(PointerEventData ped)
    {
        // When mouse enters 
        mainMenuButtonFunAud.PlayOneShot(mainMenuBtnFunct.buttonEnter);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        // When clicked
        mainMenuButtonFunAud.PlayOneShot(mainMenuBtnFunct.buttonSelect);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // When up/down key is pressed to select new button
        mainMenuButtonFunAud.PlayOneShot(mainMenuBtnFunct.buttonEnter);

        /*// If enter key is pressed
        if(Input.GetButtonDown("Submit"))
        {
            mainMenuButtonFunAud.PlayOneShot(btnFunct.buttonSelect);
        }*/
    }
}