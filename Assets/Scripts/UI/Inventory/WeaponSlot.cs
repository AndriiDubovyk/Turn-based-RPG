using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    private Image image;

    private ItemDisplayPanel itemDisplayPanel;

    private ItemData weaponData;

    void Awake()
    {
        image = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemDisplayPanel = GameObject.Find("ItemDisplayPanel").GetComponent<ItemDisplayPanel>();
    }

    public void SetWeapon(ItemData weaponData)
    {
        this.weaponData = weaponData;
        Color tmpColor;
        if (weaponData != null)
        {
            image.sprite = weaponData.sprite;

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

    public ItemData GetWeapon()
    {
        return weaponData;
    }

    public void DisplayItem()
    {
        itemDisplayPanel.SetItemData(weaponData);
    }
}
