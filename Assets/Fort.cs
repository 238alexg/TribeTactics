using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort : MonoBehaviour {

    public const int Tier1Cost = 20;
    public const int Tier2Cost = 50;
    public const int Tier3Cost = 100;
    
    public Player Owner;
    public FortTier Tier;

    public int GetUpgradeCost()
    {
        switch (Tier)
        {
            case FortTier.Tier1:
                return Tier1Cost;
            case FortTier.Tier2:
                return Tier2Cost;
            case FortTier.Tier3:
                return Tier3Cost;
        }
        return -1;
    }
}


public enum FortTier
{
    Tier0,
    Tier1,
    Tier2,
    Tier3,
    Count
}