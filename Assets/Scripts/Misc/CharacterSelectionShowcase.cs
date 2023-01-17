using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectionShowcase : MonoBehaviour
{
    [SerializeField] public GameObject[] characters;
    [SerializeField] GameObject currCharacter;
    [SerializeField] public int characterNumber;
    [SerializeField] public TextMeshPro text;

    private void Start()
    {
        currCharacter = Instantiate(characters[characterNumber], transform.position, transform.rotation);
    }
    public void changeCharacter()
    {
        Destroy(currCharacter);
        currCharacter = Instantiate(characters[characterNumber], transform.position, transform.rotation);
        PlayerPrefs.SetInt("Character", characterNumber);

        switch(characterNumber)
        {
            case 0:
                text.text = "Velocity";
                    break;
            case 1:
                text.text = "Tachyon";
                    break;
            case 2:
                text.text = "Zero";
                    break;
        }
    }
}
