using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Location Location;
    public TerrainBuff TerrainBuff;
    public Pawn Pawn;
    public SpriteRenderer SpriteRen;

    Terrain _Terrain;
    public Terrain Terrain
    {
        get
        {
            return _Terrain;
        }
        set
        {
            _Terrain = value;
            SpriteRen.sprite = GameAssets.TerrainAssets[(int)value].Sprite;
        }
    }
    
    public void ChangeTerrain(Terrain newTerrain)
    {
        // TODO: Dust animation?
        Terrain = newTerrain;
    }

    public void ChangeBuff(TerrainBuff newBuff)
    {
        // TODO: Dust animation
        TerrainBuff = newBuff;
    }
}

public enum Terrain
{
    Dirt,
    Mountains,
    Forest,
    Water,
    Desert,
    Count
}

public enum TerrainBuff
{
    None,
    Mine,
    Treehouse,
    Sandstorm,
    Hurricane,
    Count
}