using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayPanel : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    [SerializeField]
    private Button dropButton;
    [SerializeField]
    private Button equipButton;

    private ItemData itemData;

    private InventoryPanel inventoryPanel;

    void Start()
    {
        inventoryPanel = GameObject.Find("InventoryPanel").GetComponent<InventoryPanel>();
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        Color tmpColor;
        if (itemData != null)
        {
            image.sprite = itemData.sprite;
            itemName.SetText(itemData.name);
            itemDescription.SetText(itemData.description);

            tmpColor = image.color;
            tmpColor.a = 1f;
            image.color = tmpColor;

            dropButton.gameObject.SetActive(true);
            if(itemData.attack>0)
            {
                equipButton.gameObject.SetActive(true);
                if (itemData != inventoryPanel.GetEquipedWeapon())
                {
                    equipButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Equip");
                }
                else
                {
                    equipButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Unequip");
                }
            }
            else
            {
                equipButton.gameObject.SetActive(false);
            }

        }
        else
        {
            image.sprite = null;
            itemName.SetText("");
            itemDescription.SetText("");

            tmpColor = image.color;
            tmpColor.a = 0f;
            image.color = tmpColor;

            dropButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }
    }

    public void DropItem()
    {
        inventoryPanel.DropItem(itemData);
    }

    public void UseItem()
    {
        if(itemData.attack>0)
        {
            if(itemData!=inventoryPanel.GetEquipedWeapon())
            {
                inventoryPanel.EquipItem(itemData);
            }
            else
            {
                inventoryPanel.UnequipItem(itemData);
            }
        }
    }

}
