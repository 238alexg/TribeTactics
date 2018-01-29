using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAssetInfo : MonoBehaviour {

    public Tribe Tribe;
    public Class Class;
    public int Attack;
    public int MaxHealth;
    public int AttackDistance;
}

public class TerrainAsset
{
    public Sprite Sprite;
    public Terrain Terrain;
}

public class TerrainBuffAsset
{
    public Sprite Sprite;
    public TerrainBuff TerrainBuff;
}

public enum Class
{
    Swordsman,
    Archer,
    Knight,
    Leader
}