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

    [SerializeField]
    private GameObject fieldOfView; // must be circle object
    private float viewRadius;

    public float tileSize = 1f;
    public float xTilePivot = 0.5f;
    public float yTilePivot = 0.5f;

    private TurnManager turnManager;

    private List<ItemPickup> itemPickupList = new List<ItemPickup>();
    private Vector3Int worldSize;


    // Start is called before the first frame update
    void Start()
    {
        turnManager = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        worldSize = GetComponent<LevelGenerator>().GetWorldSize();
        viewRadius = fieldOfView.transform.localScale.x/2;
        GenerateFogOfWar();
    }

    public void GenerateFogOfWar()
    {
        for (int i = -worldSize.x; i < worldSize.x; i++)
        {
            for (int j = -worldSize.y; j < worldSize.y; j++)
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
        int iR = (int)Math.Round(viewRadius);
        for (int i = -iR; i <= iR; i++)
        {
            for (int j = -iR; j <= iR; j++)
            {
                Vector3Int cell = playerCell + new Vector3Int(i, j);
                if(Vector3Int.Distance(playerCell, cell)<=viewRadius)
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
