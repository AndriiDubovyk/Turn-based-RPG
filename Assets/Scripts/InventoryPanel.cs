using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{

    private PlayerUnit player;

    [SerializeField]
    private GameObject[] invetorySlots = new GameObject[6];

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
            string invDesc = "Invetory: ";
            for(int i=0; i< inventory.Length; i++)
            {
                if(inventory[i]!=null)
                {
                    invDesc += inventory[i].name + ", ";
                }
            }
            Debug.Log(invDesc);
            SetInvetoryItems();
        }
    }

    private void SetInvetoryItems()
    {
        ItemData[] inventory = player.GetInvetory();
        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i]!=null)
            {
                Image image = invetorySlots[i].transform.GetChild(0).GetComponent<Image>();
                image.sprite = inventory[i].sprite;

                Color tmpColor = image.color;
                tmpColor.a = 1f;
                image.color = tmpColor;
            }
            else
            {
                Image image = invetorySlots[i].transform.GetChild(0).GetComponent<Image>();
                image.sprite = null;

                Color tmpColor = image.color;
                tmpColor.a = 0f;
                image.color = tmpColor;
            }      
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
