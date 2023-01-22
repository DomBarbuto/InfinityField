using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class uiButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler
{
    [SerializeField] mainMenuButtonFunctions btnFunct;
    [SerializeField] AudioSource mainMenuButtonFunAud;

    EventSystem sys;
    public void OnPointerEnter(PointerEventData ped)
    {
        // When mouse enters 
        mainMenuButtonFunAud.PlayOneShot(btnFunct.buttonEnter);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        // When clicked
        mainMenuButtonFunAud.PlayOneShot(btnFunct.buttonSelect);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // When up/down key is pressed to select new button
        mainMenuButtonFunAud.PlayOneShot(btnFunct.buttonEnter);

        /*// If enter key is pressed
        if(Input.GetButtonDown("Submit"))
        {
            mainMenuButtonFunAud.PlayOneShot(btnFunct.buttonSelect);
        }*/
    }
}