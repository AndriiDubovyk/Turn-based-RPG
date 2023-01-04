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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        ItemData[] inventory = player.GetInvetory();
        if(gameObject.activeSelf)
        {
            string invDesc = "Inventory: ";
            for(int i=0; i< inventory.Length; i++)
            {
                if(inventory[i]!=null)
                {
                    invDesc += inventory[i].name + ", ";
                }
            }
            Debug.Log(invDesc);
            InitializeInventory();
        }
    }

    public void InitializeInventory()
    {
        ItemData[] inventory = player.GetInvetory();
        for (int i = 0; i < inventory.Length; i++)
        {
            invetorySlots[i].GetComponent<InventorySlot>().SetItemData(inventory[i]);
        }
        itemDisplayPanel.GetComponent<ItemDisplayPanel>().SetItemData(null);
    }

    public void DropItem(ItemData itemData)
    {
        player.DropInventoryItem(itemData);
        InitializeInventory(); // reinitialize inventory
        GameObject.Find("Grid").GetComponent<ItemSpawner>().SpawnItem(itemData); // spawn item pickup
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
