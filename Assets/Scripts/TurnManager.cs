using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject grid;

    private Turn turn;
    private enum Turn
    {
        PlayerTurn,
        EnemyTurn
    }

    public GameObject playerGO;
    public List<GameObject> enemiesGO;

    private PlayerUnit playerUnit;
    private List<EnemyUnit> enemiesUnits;

    private int activeEnemyIndex;
    private Unit activeUnit;

    

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = playerGO.GetComponent<PlayerUnit>();
        enemiesUnits = new List<EnemyUnit>();
        foreach(GameObject enemyGO in enemiesGO)
        {
            enemiesUnits.Add(enemyGO.GetComponent<EnemyUnit>());
        }
        turn = Turn.PlayerTurn;
        activeUnit = playerUnit;
        activeEnemyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ClearDeadEnemies();

        if (enemiesGO.Count == 0) turn = Turn.PlayerTurn;

        switch(turn)
        {
            case Turn.PlayerTurn:
                MakePlayerTurn();
                break;
            case Turn.EnemyTurn:
                MakeEnemyTurn();
                break;
        }
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

    private void MakePlayerTurn()
    {
        switch (playerUnit.GetState())
        {
            case Unit.State.IsThinking:
                if (playerUnit.GetMovementPath() != null 
                    && playerUnit.IsPathConfirmed())
                {
                    if(!IsPlayerPathBlockedByEnemy())
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
                break;
            case Unit.State.IsMakingTurn:
                // just wait till animation end
                break;
            case Unit.State.IsWaiting:
                // pass the turn to the opponent
                if(enemiesGO.Count>0)
                {
                    activeEnemyIndex = 0;
                    activeUnit = enemiesUnits[activeEnemyIndex];
                    activeUnit.SetState(Unit.State.IsThinking);
                    turn = Turn.EnemyTurn;
                }
                else
                {
                    turn = Turn.PlayerTurn;
                    playerUnit.SetState(Unit.State.IsThinking);
                }
                break;
        }
    }

    private void MakeEnemyTurn()
    {
        EnemyUnit enemyUnit = enemiesUnits[activeEnemyIndex];
        switch (enemyUnit.GetState())
        {
            case Unit.State.IsThinking:
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
                break;
            case Unit.State.IsMakingTurn:
                // just wait till animation end
                break;
            case Unit.State.IsWaiting:
                // pass the turn to the opponent
                if (activeEnemyIndex < enemiesGO.Count - 1)
                {
                    activeUnit = enemiesUnits[++activeEnemyIndex];
                    activeUnit.SetState(Unit.State.IsThinking);
                    turn = Turn.EnemyTurn;
                }
                else
                {
                    activeUnit = playerUnit;
                    activeUnit.SetState(Unit.State.IsThinking);
                    turn = Turn.PlayerTurn;
                }
                break;
        }
    }

    public List<EnemyUnit> GetEnemiesUnits()
    {
        return enemiesUnits;
    }
}
