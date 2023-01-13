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
    private TextMeshProUGUI itemStats;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    [SerializeField]
    private Button dropButton;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Image statsIcon;

    [SerializeField]
    private Sprite attackIcon;
    [SerializeField]
    private Sprite defenseIcon;
    [SerializeField]
    private Sprite healIcon;

    private ItemData itemData;

    private InventoryPanel inventoryPanel;
    private PlayerUnit player;

    void Awake()
    {
        inventoryPanel = GameObject.Find("InventoryPanel").GetComponent<InventoryPanel>();
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
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
                itemStats.SetText($"{itemData.attack}");
                tmpColor = statsIcon.color;
                tmpColor.a = 1f;
                statsIcon.color = tmpColor;
                statsIcon.sprite = attackIcon;
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
            else if(itemData.defense>0)
            {
                itemStats.SetText($"{itemData.defense}");
                tmpColor = statsIcon.color;
                tmpColor.a = 1f;
                statsIcon.color = tmpColor;
                statsIcon.sprite = defenseIcon;
                equipButton.gameObject.SetActive(true);
                if (itemData != inventoryPanel.GetEquipedArmor())
                {
                    equipButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Equip");
                }
                else
                {
                    equipButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Unequip");
                }
            }
            else if(itemData.healing>0)
            {
                itemStats.SetText($"{itemData.healing}");
                tmpColor = statsIcon.color;
                tmpColor.a = 1f;
                statsIcon.color = tmpColor;
                statsIcon.sprite = healIcon;
                equipButton.gameObject.SetActive(true);
                equipButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Use");
            }
            else
            {
                tmpColor = statsIcon.color;
                tmpColor.a = 0f;
                statsIcon.color = tmpColor;
                itemStats.SetText("");
                equipButton.gameObject.SetActive(false);
            }

        }
        else
        {
            image.sprite = null;
            statsIcon.sprite = null;
            itemName.SetText("");
            itemStats.SetText("");
            itemDescription.SetText("");

            tmpColor = image.color;
            tmpColor.a = 0f;
            image.color = tmpColor;

            tmpColor = statsIcon.color;
            tmpColor.a = 0f;
            statsIcon.color = tmpColor;

            dropButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }
    }

    public void DropItem()
    {
        player.SetItemTaking(false); // to prevent taking back immediately
        inventoryPanel.DropItem(itemData);   
    }

    public void UseItem()
    {
        if(itemData.attack>0)
        {
            if(itemData!=inventoryPanel.GetEquipedWeapon())
            {
                inventoryPanel.EquipWeapon(itemData);
            }
            else
            {
                inventoryPanel.UnequipWeapon(itemData);
            }
        }
        else if(itemData.defense>0)
        {
            if (itemData != inventoryPanel.GetEquipedArmor())
            {
                inventoryPanel.EquipArmor(itemData);
            }
            else
            {
                inventoryPanel.UnequipArmor(itemData);
            }
        }
        else if(itemData.healing>0)
        {
            inventoryPanel.UseHealingItem(itemData);
        }
    }

}
