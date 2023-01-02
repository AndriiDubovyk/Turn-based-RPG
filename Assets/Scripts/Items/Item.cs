using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GridManager gridManager;
    private PlayerUnit player;

    [SerializeField]
    protected string itemName;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.IsItemTakingActive())
        {
            if (player.GetCell().Equals(GetCell()))
            {
                GameObject.Find("Player").GetComponent<PlayerUnit>().TakeItem(this);
                Destroy(gameObject);
            } else
            {
                player.SetItemTaking(false);
            }
        }     
    }

    public Vector3Int GetCell()
    {
        return gridManager.groundTilemap.WorldToCell(transform.position);
    }
}
