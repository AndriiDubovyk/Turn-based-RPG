using System.Collections;
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
    public GameObject[] enemies;
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
        UnitController activeUnitController = activeUnit.GetComponent<UnitController>();
        switch(turn)
        {
            case Turn.PlayerTurn:
                switch(activeUnitController.state)
                {
                    case UnitController.State.IsThinking:
                        if(activeUnitController.GetMovementPath() != null && activeUnit.GetComponent<PlayerController>().pathConfirmed)
                        {
                            activeUnitController.ConfirmTurn();
                        } 
                        else
                        {
                            activeUnit.GetComponent<PlayerController>().SelectDestinationCell();
                        }
                        break;
                    case UnitController.State.IsMakingTurn:
                        // just wait till animation end
                        break;
                    case UnitController.State.IsWaiting:
                        // pass the turn to the opponent
                        activeEnemyIndex = 0;
                        activeUnit = enemies[activeEnemyIndex];
                        activeUnit.GetComponent<UnitController>().state = UnitController.State.IsThinking;
                        turn = Turn.EnemyTurn;
                        break;
                }
                break;
            case Turn.EnemyTurn:
                switch (activeUnitController.state)
                {
                    case UnitController.State.IsThinking:
                        Vector3Int playerCell = player.GetComponent<UnitController>().GetPositionOnGrid();
                        if(activeUnitController.CanAttack(player))
                        {
                            activeUnitController.SetEnemyTarget(player);
                        } 
                        else
                        {
                            activeUnitController.GetComponent<UnitController>().SetMovementPathTo(playerCell);
                        }
                        activeUnitController.ConfirmTurn();
                        break;
                    case UnitController.State.IsMakingTurn:
                        // just wait till animation end
                        break;
                    case UnitController.State.IsWaiting:
                        // pass the turn to the opponent
                        if (activeEnemyIndex < enemies.Length - 1)
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
                break;
        }
    }
}
