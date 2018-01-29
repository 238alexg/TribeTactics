using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player CurrentTurn;

    public Tribe Tribe;
    public string Name;

    public Pawn Leader;
    public List<Pawn> Swordsmen;
    public List<Pawn> Archers;
    public List<Pawn> Knights;

    public int Gold;

    public int ActionsLeft;

    public bool IsSignedIn
    {
        get
        {
            return Tribe != Tribe.Count && !string.IsNullOrEmpty(Name);
        }
    }
}