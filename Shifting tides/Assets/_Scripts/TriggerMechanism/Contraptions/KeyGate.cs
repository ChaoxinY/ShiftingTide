using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour
{
    public List<ItemInformation> itemsRequired = new List<ItemInformation>();
    public RequirementCanvas requirementCanvas;
    public GameObject blockade;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            List<ItemInformation> ConsumedItems = new List<ItemInformation>();

            foreach (ItemInformation itemRequired in itemsRequired)
            {
                if (PlayerInventoryManager.IsThisItemInInventory(itemRequired))
                {
                    ConsumedItems.Add(itemRequired);
                    int index = itemsRequired.IndexOf(itemRequired);
                    requirementCanvas.greenBorders[index].gameObject.SetActive(true);
                }
            }
            foreach (ItemInformation itemConsumed in ConsumedItems)
            {
                itemsRequired.Remove(itemConsumed);
            }
            if (itemsRequired.Count == 0) {
                Destroy(blockade,2f);
                Destroy(gameObject,2f);
            }
        }
    }

}
