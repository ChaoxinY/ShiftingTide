using UnityEngine;
using System.Collections;

public class PlayerInventoryManager : MonoBehaviour
{
    //public static PlayerInventory playerInventory;

    //public void Start()
    //{
    //    playerInventory = GameObject.Find("GameManager").GetComponent<PlayerInventory>();
    //}

    //public static bool IsThisItemInInventory(ItemInformation itemToCheck)
    //{
    //    bool itemAvailable = false;
    //    foreach (ItemInformation availableItem in playerInventory.collectedItems)
    //    {
    //        if (availableItem.itemName == itemToCheck.itemName)
    //        {
    //            itemAvailable = true;
    //        }
    //    }
    //    return itemAvailable;
    //}

    public static void AddItemToInventory(Item itemToAdd)
    {
        PlayerInventory.collectedItems.Add(itemToAdd.itemInformation);
        Debug.Log(PlayerInventory.collectedItems[0].itemName);
    }
    //public static void RemoveThisItemFromInventory(Item itemToRemove)
    //{
    //    playerInventory.collectedItems.Remove(itemToRemove);
    //}
}
