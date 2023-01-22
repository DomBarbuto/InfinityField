using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class uiButtonSound_CharSel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler
{
    [SerializeField] CharacterSelectionShowcase css;
    [SerializeField] AudioSource cssAud;

    public void OnPointerEnter(PointerEventData ped)
    {
        // When mouse enters 
        cssAud.PlayOneShot(css.buttonEnter);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        // When clicked
        cssAud.PlayOneShot(css.buttonSelect);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // When up/down key is pressed to select new button
        cssAud.PlayOneShot(css.buttonEnter);
    }
}
