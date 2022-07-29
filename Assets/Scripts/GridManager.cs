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

    public float tileSize = 1f;
    public float xTilePivot = 0.5f;
    public float yTilePivot = 0.5f;

    private TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = GameObject.Find("GameHandler").GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3Int GetPlayerCell()
    {
        return player.GetComponent<Unit>().GetCell();
    }

    private Vector3Int[] GetEnemiesCells()
    {
        List<GameObject> enemies = turnManager.enemiesGO;
        Vector3Int[] enemiesCells = Array.ConvertAll(enemies.ToArray(), it => it.GetComponent<Unit>().GetCell());
        return enemiesCells;
    }

    public List<Vector3Int> GetOccupiedCells()
    {
        List<Vector3Int> occupiedCells = turnManager.enemiesGO.ConvertAll(x => x.GetComponent<Unit>().GetCell());
        occupiedCells.Add(player.GetComponent<Unit>().GetCell());
        return occupiedCells;
    }

    public Unit GetUnitAtCell(Vector3Int cell)
    {
        if(cell == GetPlayerCell())
        {
            return player.GetComponent<Player>(); ;
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
