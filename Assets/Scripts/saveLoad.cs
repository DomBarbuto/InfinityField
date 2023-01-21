using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour
{
    public static void saveFromMainMenuOptions()
    {
        PlayerPrefs.Save();
    }

    public static void saveFromPauseMenuOptions()
    {
        PlayerPrefs.SetInt("Character", gameManager.instance.playerController.currCharacter);

        if (gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].perks != null)
        {
            List<perkList> perkList = gameManager.instance.playerController.characterList[gameManager.instance.playerController.currCharacter].perks;

            string jsonPerkString = JsonUtility.ToJson(perkList);

            PlayerPrefs.SetString("myObjectList", jsonPerkString);
        }

        if (gameManager.instance.playerController.weaponInventory != null)
        {
            List<weaponCreation> weaponList = gameManager.instance.playerController.weaponInventory;

            string jsonWeaponString = JsonUtility.ToJson(weaponList);

            PlayerPrefs.SetString("myObjectList", jsonWeaponString);
        }
        PlayerPrefs.Save();
        //Save all permament upgrade checks here

    }

    public static void loadFromMainGame()
    {
        if (PlayerPrefs.HasKey("Character"))
        {
            gameManager.instance.playerController.currCharacter =
            PlayerPrefs.GetInt("Character", gameManager.instance.playerController.currCharacter);
        }

        if (PlayerPrefs.HasKey("weaponList"))
        {
            string weaponLoad = PlayerPrefs.GetString("weaponList");

            List<weaponCreation> weaponLoadList = JsonUtility.FromJson<List<weaponCreation>>(weaponLoad);
        }

        if (PlayerPrefs.HasKey("perkList"))
        {
            string perkLoad = PlayerPrefs.GetString("perkList");

            List<perkList> perkLoadList = JsonUtility.FromJson<List<perkList>>(perkLoad);
        }
    }
}
