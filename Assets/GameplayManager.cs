﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

    public static GameplayManager Inst;

    public static Tile CurrentSelectedTile;

    public static Pawn CurrentSelectedPawn;

    public static SelectionState SelectionState;

    public Player Player1;
    public Player Player2;

    public GameObject TurnPassScreen;
    public Text TurnPassText, PlayerNameHeaderText;

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(this);
        }
        else
        {
            Inst = this;
        }
    }

    public void TileTap(Tile tile)
    {
        switch (SelectionState)
        {
            case SelectionState.NoSelection:
                if (tile.Pawn != null && tile.Pawn.Owner == Player.CurrentTurn)
                {
                    SelectMyPawn(tile.Pawn);
                }
                return;
            case SelectionState.PawnSelected:
                if (tile.Pawn != null)
                {
                    if (tile.Pawn.Owner == Player.CurrentTurn) { SelectMyPawn(tile.Pawn); }
                    else { AttackEnemyPawn(tile.Pawn); }
                }
                return;
            case SelectionState.PlacingPawn:
                TryPlacePawnOnTile(tile);
                return;
            default:
                return;
        }
    }
    
    public void SelectMyPawn(Pawn pawn)
    {
        CurrentSelectedPawn = pawn;
        ChangeSelectionState(SelectionState.PawnSelected);
    }

    public void AttackEnemyPawn(Pawn enemyPawn)
    {
        if (CurrentSelectedPawn != null 
            && CurrentSelectedPawn.CanAct
            && CurrentSelectedPawn.CanAttackPawn(enemyPawn))
        {
            // Handle any animation
            enemyPawn.DealDamage(CurrentSelectedPawn.GetAttack());
        }
    }

    public void TryPlacePawnOnTile(Tile tile)
    {
        if (tile.Pawn != null)
        {
            return;
        }

        Location curLoc = new Location();
        for (int i = 0; i < Location.FiveSlice.Length; i++)
        {
            curLoc = tile.Location + Location.FiveSlice[i];
            if (Map.IsInMapBounds(curLoc))
            {
                Tile placeableTile = Map.Tiles[curLoc.X, curLoc.Y];
                if (placeableTile.Pawn != null 
                    && placeableTile.Pawn.Owner == Player.CurrentTurn
                    && placeableTile.Pawn == Player.CurrentTurn.Leader)
                {
                    PawnAssetInfo assetInfo = Player.CurrentTurn == Player1 ? GameAssets.Inst.Swordsmen[(int)Player1.Tribe] : GameAssets.Inst.Swordsmen[(int)Player1.Tribe];
                    Player.CurrentTurn.Swordsmen.Add(GameSetup.Inst.InstantiatePawnAt(tile, assetInfo, Player.CurrentTurn));
                }
            }
        }
    }
    
    public void CancelPlacePawn()
    {
        Player.CurrentTurn.Gold += 10;
    }
    
    public void BuyPawnButtonPress()
    {
        Player.CurrentTurn.Gold -= 10;
        ChangeSelectionState(SelectionState.PlacingPawn);
    }

    public void MoveButtonPress()
    {

    }

    public void BuildActionButtonPress()
    {

    }

    public void AttackButtonPress()
    {

    }

    public void EndTurnButtonPress()
    {
        Player.CurrentTurn = Player.CurrentTurn == Player1 ? Player2 : Player1;
        TurnPassText.text = "Hey " + Player.CurrentTurn.Name + " it's your turn!";
        TurnPassScreen.SetActive(true);
    }

    public void RemoveNextTurnSplashScreen()
    {
        PlayerNameHeaderText.text = Player.CurrentTurn.Name + "'s Turn";
        TurnPassScreen.SetActive(false);
    }

    public void ChangeSelectionState(SelectionState newState)
    {
        SelectionState = newState;
        // TODO: In/Active action bar buttons
    }
}


public enum SelectionState
{
    NoSelection,
    PawnSelected,
    BuyPawnMenu,
    PlacingPawn,
    Count
}