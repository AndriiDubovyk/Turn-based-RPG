using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorSlot : MonoBehaviour
{
    private Image image;

    private ItemDisplayPanel itemDisplayPanel;

    private ItemData armorData;

    void Awake()
    {
        image = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemDisplayPanel = GameObject.Find("ItemDisplayPanel").GetComponent<ItemDisplayPanel>();
    }

    public void SetArmor(ItemData armorData)
    {
        this.armorData = armorData;
        Color tmpColor;
        if (armorData != null)
        {
            image.sprite = armorData.sprite;

            tmpColor = image.color;
            tmpColor.a = 1f;
            image.color = tmpColor;
        }
        else
        {
            image.sprite = null;
            tmpColor = image.color;
            tmpColor.a = 0f;
            image.color = tmpColor;
        }
    }

    public ItemData GetArmor()
    {
        return armorData;
    }

    public void DisplayItem()
    {
        itemDisplayPanel.SetItemData(armorData);
    }
}
