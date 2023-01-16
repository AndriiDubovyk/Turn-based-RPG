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
    private Color darknesColor;

    [SerializeField]
    private float visibilityRadius;

    public float tileSize = 1f;
    public float xTilePivot = 0.5f;
    public float yTilePivot = 0.5f;

    private TurnManager turnManager;

    private List<ItemPickup> itemPickupList = new List<ItemPickup>();
    private Vector3Int worldSize;
    private List<Vector3Int> visibleTilesPos = new List<Vector3Int>();

    // Start is called before the first frame update
    void Awake()
    {
        turnManager = GameObject.Find("GameHandler").GetComponent<TurnManager>();
        worldSize = GetComponent<LevelGenerator>().GetWorldSize();
        GenerateFogOfWar();
    }

    public void GenerateFogOfWar()
    {
        for (int i = -worldSize.x; i < worldSize.x; i++)
        {
            for (int j = -worldSize.y; j < worldSize.y; j++)
            {
                Vector3Int cell = new Vector3Int(i, j);
                fogOfWarTilemap.SetTile(cell, fogOfWarTile);
                SetTileVisibility(cell, false);
            }
        }
    }

    public void AddItemPickup(ItemPickup itemPickup)
    {
        itemPickupList.Add(itemPickup);
        UpdateItemsVisibility();
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
    }

    public void UpdateVisibility()
    {
        UpdateGridVisibility();
        UpdateItemsVisibility();
        UpdateEnemiesVisibility();
    }

    private void UpdateItemsVisibility()
    {
        foreach(ItemPickup it in itemPickupList)
        {
            if(groundTilemap.GetColor(it.GetCell())==darknesColor)
            {
                it.SetVisibility(false);         
            }
            else
            {
                it.SetVisibility(true);
            }
        }
    }

    private void UpdateEnemiesVisibility()
    {
        foreach (EnemyUnit eu in turnManager.GetEnemiesUnits())
        {
            if (groundTilemap.GetColor(eu.GetCell()) == darknesColor)
            {
                eu.SetVisibility(false);
            }
            else
            {
                eu.SetVisibility(true);
            }
        }
    }

    public void RevealFogOfWar(List<SavedData.Cell> revealedCells)
    {
        foreach(SavedData.Cell x in revealedCells)
        {
            fogOfWarTilemap.SetTile(x, null);
            SetTileVisibility(x, false);
        }
    }

    private void UpdateGridVisibility()
    {
        foreach (Vector3Int cell in visibleTilesPos)
        {
            SetTileVisibility(cell, false);
        }
        visibleTilesPos.Clear();

        Vector3Int playerCell = GetPlayerCell();
        int iR = (int)Math.Round(visibilityRadius);
        for (int i = -iR; i <= iR; i++)
        {
            for (int j = -iR; j <= iR; j++)
            {
                Vector3Int cell = playerCell + new Vector3Int(i, j);
                if (Vector3Int.Distance(playerCell, cell) <= visibilityRadius)
                {
                    Vector2 playerPos = player.transform.position;
                    playerPos.y +=  0.01f;
                    Vector2 target = new Vector2(i, j);
                    
                    if(collidersTilemap.GetTile(cell) == null) // no wall or other obstacles
                    {
                        RaycastHit2D raycast = Physics2D.Raycast(playerPos, target, Vector2.Distance(playerPos, playerPos + target));
                        if (raycast.collider == null)
                        {
                            fogOfWarTilemap.SetTile(cell, null);
                            SetTileVisibility(cell, true);
                        }
                    } 
                    else
                    {
                        if (i > 0) target += new Vector2(-1f, 0); // right wall
                        if (i < 0) target += new Vector2(1f, 0); // left wall
                        if (j > 0) target += new Vector2(0, -1f); // top wall
                        if (j < 0) target += new Vector2(0, 1f); // bot wall
                        RaycastHit2D raycast = Physics2D.Raycast(playerPos, target, Vector2.Distance(playerPos, playerPos + target));
                        if (raycast.collider == null)
                        {
                            fogOfWarTilemap.SetTile(cell, null); 
                            if (j > 0) fogOfWarTilemap.SetTile(cell+new Vector3Int(0, 1), null);
                            SetTileVisibility(cell, true);
                        }
                    }

                }
            }
        }
    }


    public List<SavedData.Cell> GetRevealedFogOfWarCells()
    {
        List<SavedData.Cell> list = new List<SavedData.Cell>();
        for (int i = -worldSize.x; i < worldSize.x; i++)
        {
            for (int j = -worldSize.y; j < worldSize.y; j++)
            {
                Vector3Int cell = new Vector3Int(i, j);
                if (fogOfWarTilemap.GetTile(cell) == null)
                    list.Add(cell);
            }
        }
        return list;
    }

    private void SetTileVisibility(Vector3Int cell, bool isVisible)
    {
        if (isVisible) visibleTilesPos.Add(cell);
        groundTilemap.SetTileFlags(cell, TileFlags.None);
        groundTilemap.SetColor(cell, isVisible ? Color.white : darknesColor);
        collidersTilemap.SetTileFlags(cell, TileFlags.None);
        collidersTilemap.SetColor(cell, isVisible ? Color.white : darknesColor);
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
