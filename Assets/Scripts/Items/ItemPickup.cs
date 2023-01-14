using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;

    private GridManager gridManager;
    private PlayerUnit player;

    void Awake()
    {
        SetVisibility(false);
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FieldOfView") SetVisibility(true);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FieldOfView") SetVisibility(false);
    }

    private void SetVisibility(bool isVisible)
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        if (isVisible) tmp.a = 1f;
        else tmp.a = 0f;
        GetComponent<SpriteRenderer>().color = tmp;
    }
}
