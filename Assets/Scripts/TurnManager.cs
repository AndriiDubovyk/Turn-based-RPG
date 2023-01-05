using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject grid;

    private GameObject playerGO;
    private List<GameObject> enemiesGO = new List<GameObject>();

    private PlayerUnit playerUnit;

    private List<EnemyUnit> enemiesUnits = new List<EnemyUnit>();
    private int activeEnemyIndex;

    private Unit activeUnit;

    

    // Start is called before the first frame update
    void Start()
    {
        activeUnit = playerUnit;
        activeEnemyIndex = 0;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemiesGO.Add(enemy);
        enemiesUnits.Add(enemy.GetComponent<EnemyUnit>());
    }

    public void AddPlayer(GameObject player)
    {
        playerGO = player;
        playerUnit = playerGO.GetComponent<PlayerUnit>();
    }

    public List<GameObject> GetEnemiesGO()
    {
        return enemiesGO;
    }

    public List<GameObject> GetPlayerGO()
    {
        return enemiesGO;
    }


    // Update is called once per frame
    void Update()
    {
        ClearDeadEnemies();
        if (enemiesUnits.Count == 0) activeUnit=playerUnit;
        MakeTurn(activeUnit);
    }

    private void ClearDeadEnemies()
    {
        List<GameObject> deadEnemiesGO = new List<GameObject>();
        foreach(GameObject enemyGO in enemiesGO)
        {
            if(enemyGO.GetComponent<Unit>().IsDead())
            {
                deadEnemiesGO.Add(enemyGO);
            }
        }
        foreach (GameObject deadEnemyGO in deadEnemiesGO)
        {
            enemiesGO.Remove(deadEnemyGO);
            enemiesUnits.Remove(deadEnemyGO.GetComponent<EnemyUnit>());
            Destroy(deadEnemyGO);
        }
    }

    private bool IsPlayerPathBlockedByEnemy()
    {
        List<Vector3Int> path = playerUnit.GetMovementPath();
        Vector3Int[] enemiesCells = Array.ConvertAll(enemiesUnits.ToArray(), x => x.GetCell());
        foreach(Vector3Int pathCell in path)
        {
            foreach (Vector3Int enemyCell in enemiesCells)
            {
                if (pathCell == enemyCell)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void GetPlayerInput()
    {
        if (playerUnit.GetMovementPath() != null && playerUnit.IsPathConfirmed())
        {
            if (!IsPlayerPathBlockedByEnemy())
            {
                playerUnit.ConfirmTurn();
            }
            else
            {
                // Clear path
                Debug.Log("Path has been blocked by enemy");
                playerUnit.SetMovementPathTo(playerUnit.GetCell());
                playerUnit.UpdateOverlayMarks();
                playerUnit.SetState(Unit.State.IsThinking);
            }
        }
        else
        {
            playerUnit.SetAction();
        }
    }

    private void MakeTurn(Unit unit)
    {
        switch(unit.GetState())
        {
            case Unit.State.IsThinking:
                if(unit == playerUnit)
                {
                    GetPlayerInput();
                }
                else if(unit is EnemyUnit)
                {
                    PrepareEnemyAction((EnemyUnit)unit);
                }
                break;
            case Unit.State.IsMakingTurn:
                break;
            case Unit.State.IsWaiting:
                PassTurnToNextUnit();
                break;
        }
    }

    private void PrepareEnemyAction(EnemyUnit enemyUnit)
    {
        Vector3Int playerCell = playerUnit.GetCell();
        if (enemyUnit.CanAttack(playerUnit))
        {
            enemyUnit.SetAttackTarget(playerUnit);
        }
        else
        {
            enemyUnit.SetMovementPathTo(playerCell);
        }
        enemyUnit.ConfirmTurn();
    }

    private void PassTurnToNextUnit()
    {
        if(activeUnit==playerUnit && enemiesUnits.Count>0)
        {
            activeEnemyIndex = 0;
            activeUnit = enemiesUnits[activeEnemyIndex];
        } 
        else
        {
            if (activeEnemyIndex < enemiesUnits.Count - 1)
            {
                activeUnit = enemiesUnits[++activeEnemyIndex];
            }
            else
            {
                activeUnit = playerUnit;
            }
        }
        activeUnit.SetState(Unit.State.IsThinking);
    }

    public List<EnemyUnit> GetEnemiesUnits()
    {
        return enemiesUnits;
    }
}
