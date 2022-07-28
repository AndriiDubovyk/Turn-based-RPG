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

    public GameObject player;
    public List<GameObject> enemies;
    private int activeEnemyIndex;
    private GameObject activeUnit;

    

    // Start is called before the first frame update
    void Start()
    {
        turn = Turn.PlayerTurn;
        activeUnit = player;
        activeEnemyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ClearDeadEnemies();
        UnitController activeUnitController = activeUnit.GetComponent<UnitController>();

        if (enemies.Count == 0) turn = Turn.PlayerTurn;

        switch(turn)
        {
            case Turn.PlayerTurn:
                MakePlayerTurn(activeUnitController);
                break;
            case Turn.EnemyTurn:
                MakeEnemyTurn(activeUnitController);
                break;
        }
    }

    private void ClearDeadEnemies()
    {
        List<GameObject> deadEnemies = new List<GameObject>();
        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<CombatUnit>().IsDead())
            {
                deadEnemies.Add(enemy);
            }
        }
        foreach (GameObject deadEnemy in deadEnemies)
        {
            enemies.Remove(deadEnemy);
            Destroy(deadEnemy);
        }
    }

    private bool IsPlayerPathBlockedByEnemy()
    {
        UnitController playerUC = player.GetComponent<UnitController>();
        List<Vector3Int> path = playerUC.GetMovementPath();
        Vector3Int[] enemiesCells = Array.ConvertAll(enemies.ToArray(), x => x.GetComponent<UnitController>().GetPositionOnGrid());
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

    private void MakePlayerTurn(UnitController uc)
    {
        switch (uc.state)
        {
            case UnitController.State.IsThinking:
                if (uc.GetMovementPath() != null 
                    && player.GetComponent<PlayerController>().pathConfirmed)
                {
                    if(!IsPlayerPathBlockedByEnemy())
                    {
                        uc.ConfirmTurn();
                    }
                    else
                    {
                        // Clear path
                        Debug.Log("Path has been blocked by enemy");
                        uc.SetMovementPathTo(uc.GetPositionOnGrid());
                        player.GetComponent<PlayerController>().UpdateOverlayMarks();
                        uc.state = UnitController.State.IsThinking;
                    }
                }
                else
                {
                    activeUnit.GetComponent<PlayerController>().SetAction();
                }
                break;
            case UnitController.State.IsMakingTurn:
                // just wait till animation end
                break;
            case UnitController.State.IsWaiting:
                // pass the turn to the opponent
                if(enemies.Count>0)
                {
                    activeEnemyIndex = 0;
                    activeUnit = enemies[activeEnemyIndex];
                    activeUnit.GetComponent<UnitController>().state = UnitController.State.IsThinking;
                    turn = Turn.EnemyTurn;
                }
                else
                {
                    turn = Turn.PlayerTurn;
                    uc.state = UnitController.State.IsThinking;
                }
                break;
        }
    }

    private void MakeEnemyTurn(UnitController uc)
    {
        switch (uc.state)
        {
            case UnitController.State.IsThinking:
                Vector3Int playerCell = player.GetComponent<UnitController>().GetPositionOnGrid();
                if (uc.CanAttack(player))
                {
                    uc.SetEnemyTarget(player);
                }
                else
                {
                    uc.GetComponent<UnitController>().SetMovementPathTo(playerCell);
                }
                uc.ConfirmTurn();
                break;
            case UnitController.State.IsMakingTurn:
                // just wait till animation end
                break;
            case UnitController.State.IsWaiting:
                // pass the turn to the opponent
                if (activeEnemyIndex < enemies.Count - 1)
                {
                    activeUnit = enemies[++activeEnemyIndex];
                    activeUnit.GetComponent<UnitController>().state = UnitController.State.IsThinking;
                    turn = Turn.EnemyTurn;
                }
                else
                {
                    activeUnit = player;
                    activeUnit.GetComponent<UnitController>().state = UnitController.State.IsThinking;
                    turn = Turn.PlayerTurn;
                }
                break;
        }
    }
}
