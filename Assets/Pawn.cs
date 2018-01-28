using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

    public PawnAsset AssetInfo;

    public Player Owner;
    
    public int Health;
    public int MovesLeft;
    public int ConsumedTurnsLeft;

    public bool CompletedTurn;

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
}
