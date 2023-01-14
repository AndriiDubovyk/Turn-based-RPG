using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public GameObject player;
    public Tilemap groundTilemap;
    public Tilemap uiOverlayTilemap;
    public Tilemap collidersTilemap;
    public Tilemap fogOfWarTilemap;

    [SerializeField]
    private Tile fogOfWarTile;


    public float tileSize = 1f;
    public float xTilePivot = 0.5f;
    public float yTilePivot = 0.5f;

    private TurnManager turnManager;

    private List<ItemPickup> itemPickupList = new List<ItemPickup>();


    // Start is called before the first frame update
    void Start()
    {
        turnManager = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        //GenerateFogOfWar();
    }

    public void GenerateFogOfWar()
    {
        for (int i = -200; i < 200; i++)
        {
            for (int j = -200; j < 200; j++)
            {
                fogOfWarTilemap.SetTile(new Vector3Int(i, j), fogOfWarTile);
            }
        }
    }

    public void AddItemPickup(ItemPickup itemPickup)
    {
        itemPickupList.Add(itemPickup);
    }

    public void RemoveItemPickup(ItemPickup itemPickup)
    {
        itemPickupList.Remove(itemPickup);
    }

    public List<ItemPickup> GetItemPickupList()
    {
        return itemPickupList;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        Vector3Int playerCell = GetPlayerCell();
        for (int i = -6; i <= 6; i++)
        {
            for (int j = -6; j <= 6; j++)
            {
                Vector3Int cell = playerCell + new Vector3Int(i, j);
                fogOfWarTilemap.SetTile(cell, null);
            }
        }
    }

    public Vector3Int GetPlayerCell()
    {
        return player.GetComponent<Unit>().GetCell();
    }

    private Vector3Int[] GetEnemiesCells()
    {
        List<GameObject> enemies = turnManager.GetEnemiesGO();
        Vector3Int[] enemiesCells = Array.ConvertAll(enemies.ToArray(), it => it.GetComponent<Unit>().GetCell());
        return enemiesCells;
    }

    public List<Vector3Int> GetOccupiedCells()
    {
        List<Vector3Int> occupiedCells = turnManager.GetEnemiesGO().ConvertAll(x => x.GetComponent<Unit>().GetCell());
        occupiedCells.Add(player.GetComponent<Unit>().GetCell());
        return occupiedCells;
    }

    public Unit GetUnitAtCell(Vector3Int cell)
    {
        if(cell == GetPlayerCell())
        {
            return player.GetComponent<PlayerUnit>(); ;
        } else
        {
            Vector3Int[] enemiesCells = GetEnemiesCells();
            for(int i = 0; i<enemiesCells.Length; i++)
            {
                if (cell == enemiesCells[i]) return turnManager.GetEnemiesUnits()[i];
            }
        }
        return null;
    }


}
