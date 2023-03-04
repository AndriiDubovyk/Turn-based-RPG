using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTakeElement : MonoBehaviour
{

    private ItemPickup itemPickup;
    [SerializeField]
    private Image image;

    public void SetItemPickup(ItemPickup itemPickup)
    {
        this.itemPickup = itemPickup;
        image.sprite = itemPickup.itemData.sprite;
    }

    public ItemPickup GetItemPickup()
    {
        return itemPickup;
    }


    public void TakeItem()
    {
        GameObject.Find("Player").GetComponent<PlayerUnit>().TakeItem(itemPickup);
    }
}
