using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private ItemsPickupList itemsPickupList;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    public void SpawnItem(ItemData itemData)
    {
        foreach(ItemPickup ip in itemsPickupList.itemsPickup)
        {
            if(ip.itemData.Equals(itemData))
            {
                Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, ip.gameObject.transform.position.z);
                ip.gameObject.transform.position = newPos;
                Instantiate(ip.gameObject); 
                break;
            }
        }
    }
}
