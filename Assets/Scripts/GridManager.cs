using System;
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

    private Vector3Int GetPlayerCell()
    {
        return player.GetComponent<UnitController>().GetPositionOnGrid();
    }

    private Vector3Int[] GetEnemiesCells()
    {
        GameObject[] enemies = turnManager.enemies;
        Vector3Int[] enemiesCells = Array.ConvertAll(enemies, it => it.GetComponent<UnitController>().GetPositionOnGrid());
        return enemiesCells;
    }

    public GameObject GetUnitAtCell(Vector3Int cell)
    {
        if(cell == GetPlayerCell())
        {
            return player;
        } else
        {
            Vector3Int[] enemiesCells = GetEnemiesCells();
            for(int i = 0; i<enemiesCells.Length; i++)
            {
                if (cell == enemiesCells[i]) return turnManager.enemies[i];
            }
        }
        return null;
    }


}
