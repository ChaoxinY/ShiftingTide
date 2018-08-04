using UnityEngine;
using System.Collections;

public class PlayerInventoryManager : MonoBehaviour
{
    public static bool IsThisItemInInventory(ItemInformation itemToCheck)
    {
        bool itemAvailable = false;
        foreach (ItemInformation availableItem in PlayerInventory.collectedItems)
        {
            if (availableItem.itemName == itemToCheck.itemName)
            {
                itemAvailable = true;
            }
        }
        return itemAvailable;
    }

    public static void AddItemToInventory(Item itemToAdd)
    {
        PlayerInventory.collectedItems.Add(itemToAdd.itemInformation);      
    }
    public static void RemoveThisItemFromInventory(ItemInformation itemToRemove)
    {
        PlayerInventory.collectedItems.Remove(itemToRemove);
    }
}
