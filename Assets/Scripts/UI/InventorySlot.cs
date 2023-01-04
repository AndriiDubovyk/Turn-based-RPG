using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private ItemDisplayPanel itemDisplayPanel;

    private ItemData itemData;
    private Image image;
    

    private void Start()
    {
        image = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemDisplayPanel = GameObject.Find("ItemDisplayPanel").GetComponent<ItemDisplayPanel>();
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        if(itemData!=null)
        {
            image.sprite = itemData.sprite;
            Color tmpColor = image.color;
            tmpColor.a = 1f;
            image.color = tmpColor;
        }
        else
        {
            image.sprite = null;
            Color tmpColor = image.color;
            tmpColor.a = 0f;
            image.color = tmpColor;
        }
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public void DisplayItem()
    {
        itemDisplayPanel.SetItemData(itemData);
    }
}
