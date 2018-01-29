using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

    public PawnAssetInfo AssetInfo;
    
    public Animator Animator;

    public Player Owner;

    public Location Location;

    public int Health;
    public int MovesLeft;
    public int ConsumedTurnsLeft;

    public bool CompletedTurn;

    public bool CanAct { get { return !CompletedTurn; } }
    
    public void DealDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            KillPawn();
        }
    }

    public void KillPawn()
    {
        // TODO: Remove from board
    }

    public void RestoreHealth()
    {
        Health = AssetInfo.MaxHealth;
    }

    public virtual int GetAttack()
    {
        return AssetInfo.Attack;
    }

    public virtual int GetHealth()
    {
        return Health;
    }
   
    public bool CanAttackPawn(Pawn enemy)
    {
        return enemy.Location.WithinRangeOf(Location, AssetInfo.AttackDistance, false);
    }
    
    public bool CanPlacePawnOnTile(Tile tile)
    {
        return false;
    }
}
