using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{

    private PlayerUnit player;

    [SerializeField]
    private GameObject[] invetorySlots = new GameObject[6];
    [SerializeField]
    private GameObject itemDisplayPanel;
    [SerializeField]
    private GameObject weaponSlotDisplay;
    [SerializeField]
    private GameObject armorSlotDisplay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        InitializeInventory();
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        ItemData[] inventory = player.GetInventory();
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        ItemData[] inventory = player.GetInventory();
        for (int i = 0; i < inventory.Length; i++)
        {
            invetorySlots[i].GetComponent<InventorySlot>().SetItemData(inventory[i]);
        }

        ItemData equipedWeapon = player.GetEquipedWeapon();
        weaponSlotDisplay.GetComponent<WeaponSlot>().SetWeapon(equipedWeapon);
        ItemData equipedArmor = player.GetEquipedArmor();
        armorSlotDisplay.GetComponent<ArmorSlot>().SetArmor(equipedArmor);
        itemDisplayPanel.GetComponent<ItemDisplayPanel>().SetItemData(null);
    }

    public void DropItem(ItemData itemData)
    {
        player.RemoveItem(itemData);
        InitializeInventory(); // reinitialize inventory
        ItemSpawner itemSpawner = GameObject.Find("Grid").GetComponent<ItemSpawner>();
        itemSpawner.SpawnItem(itemData, player.transform.position); // spawn item pickup
    }

    public void UseHealingItem(ItemData itemData)
    {
        if(itemData.healing>0)
        {
            player.RemoveItem(itemData); // remove from main inventory
            player.Heal(itemData.healing);
            InitializeInventory(); // reinitialize inventory
        }
    }

    public void EquipWeapon(ItemData itemData)
    {
        player.RemoveItem(itemData); // remove from main inventory
        player.EquipWeapon(itemData);
        InitializeInventory(); // reinitialize inventory       
    }

    public void EquipArmor(ItemData itemData)
    {
        player.RemoveItem(itemData); // remove from main inventory
        player.EquipArmor(itemData);
        InitializeInventory(); // reinitialize inventory       
    }

    public void UnequipWeapon(ItemData itemData)
    {
        if(player.HasFreeInventorySlots())
        {
            player.RemoveItem(itemData); // remove from main inventory
            player.AddItem(itemData);
            InitializeInventory(); // reinitialize inventory
        }
    }

    public void UnequipArmor(ItemData itemData)
    {
        if (player.HasFreeInventorySlots())
        {
            player.RemoveItem(itemData); // remove from main inventory
            player.AddItem(itemData);
            InitializeInventory(); // reinitialize inventory
        }
    }

    public ItemData GetEquipedWeapon()
    {
        return player.GetEquipedWeapon();
    }

    public ItemData GetEquipedArmor()
    {
        return player.GetEquipedArmor();
    }
}