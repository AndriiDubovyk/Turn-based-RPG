using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private ItemsPickupList itemsPickupList;

    public GameObject SpawnItem(ItemData itemData, Vector3 position)
    {
        GameObject spawnedItem = null;
        foreach (ItemPickup ip in itemsPickupList.itemsPickup)
        {
            if(ip.itemData.Equals(itemData))
            {
                Vector3 newPos = new Vector3(position.x, position.y, ip.gameObject.transform.position.z);
                ip.gameObject.transform.position = newPos;
                spawnedItem = Instantiate(ip.gameObject);
                spawnedItem.GetComponent<ItemPickup>().itemData = itemData.Clone(); // new link
                Debug.Log($"Spawn {itemData.name}");
                break;
            }
        }
        return spawnedItem;
    }

    public GameObject SpawnItem(string itemName, Vector3 position)
    {
        GameObject spawnedItem = null;
        foreach (ItemPickup ip in itemsPickupList.itemsPickup)
        {
            if (ip.itemData.name.Equals(itemName))
            {
                Vector3 newPos = new Vector3(position.x, position.y, ip.gameObject.transform.position.z);
                ip.gameObject.transform.position = newPos;
                spawnedItem = Instantiate(ip.gameObject);
                spawnedItem.GetComponent<ItemPickup>().itemData = (spawnedItem.GetComponent<ItemPickup>().itemData).Clone(); // new link
                break;
            }
        }
        return spawnedItem;
    }
}
