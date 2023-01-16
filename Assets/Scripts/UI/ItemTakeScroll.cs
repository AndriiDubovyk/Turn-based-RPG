using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTakeScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject itemTakeElementPrefab;
    [SerializeField]
    private GameObject content;

    private List<GameObject> itemTakeElements = new List<GameObject>();

    private PlayerUnit player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
    }

    public void UpdateItems(List<ItemPickup> allItemPickups)
    {
        Clear();
        foreach(ItemPickup itemPickup in allItemPickups)
        {
            if(player.GetCell().Equals(itemPickup.GetCell()))
            {
                GameObject newElement = Instantiate(itemTakeElementPrefab, content.transform);
                newElement.GetComponent<ItemTakeElement>().SetItemPickup(itemPickup);
                itemTakeElements.Add(newElement);
            }
        }
    }

    private void Clear()
    {
        foreach(GameObject go in itemTakeElements)
        {
            Destroy(go);
        }
        itemTakeElements.Clear();
    }
}
