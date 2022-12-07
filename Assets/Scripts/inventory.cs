using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    weaponCreation weapon;

    public Image icon;

    public Transform inventorySlots;

    inventory[] slots;
    // Start is called before the first frame update
    void Start()
    {
        slots = inventorySlots.GetComponentsInChildren<inventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addWeapon(weaponCreation newWeapon)
    {
        weapon = newWeapon;

        icon.sprite = weapon.icon;
        icon.enabled = true;

    }
    public void updateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < gameManager.instance.playerController.weaponInventory.Count)
            {
                slots[i].addWeapon(gameManager.instance.playerController.weaponInventory[i]);
            }
        }
    }
}
