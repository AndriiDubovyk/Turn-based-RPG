using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    private GridManager gridManager;
    private PlayerUnit player;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        gameObject.GetComponent<SpriteRenderer>().sprite = itemData.sprite;
        gridManager.AddItemPickup(this);
    }

    private void OnDestroy()
    {
        gridManager.RemoveItemPickup(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.IsItemTakingActive())
        {
            if (player.GetCell().Equals(GetCell()))
            {
                GameObject.Find("Player").GetComponent<PlayerUnit>().TakeItem(this);
            }
        }     
    }

    public Vector3Int GetCell()
    {
        return gridManager.groundTilemap.WorldToCell(transform.position);
    }
}
