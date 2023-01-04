using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItemsPickupList", menuName = "SO/ItemsPickupList")]
public class ItemsPickupList : ScriptableObject
{
    public List<ItemPickup> itemsPickup;
}
