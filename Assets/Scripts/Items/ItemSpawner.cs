using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private ItemsPickupList itemsPickupList;

    public void SpawnItem(ItemData itemData, Vector3 position)
    {
        foreach(ItemPickup ip in itemsPickupList.itemsPickup)
        {
            if(ip.itemData.Equals(itemData))
            {
                Vector3 newPos = new Vector3(position.x, position.y, ip.gameObject.transform.position.z);
                ip.gameObject.transform.position = newPos;
                Instantiate(ip.gameObject); 
                break;
            }
        }
    }

    public void SpawnItem(string itemName, Vector3 position)
    {
        foreach (ItemPickup ip in itemsPickupList.itemsPickup)
        {
            if (ip.itemData.name.Equals(itemName))
            {
                Vector3 newPos = new Vector3(position.x, position.y, ip.gameObject.transform.position.z);
                ip.gameObject.transform.position = newPos;
                Instantiate(ip.gameObject);
                break;
            }
        }
    }
}
