using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {

    public Sprite[] SwordsmenSprites;
    public Sprite[] ArcherSprites;
    public Sprite[] KnightSprites;
    public Sprite[] LeaderSprites;

    public Sprite[] TerrainSprites;
    public Sprite[] TerrainBuffSprites;

    public static SwordsmanAsset[] Swordsmen = new SwordsmanAsset[(int)Tribe.Count];
    public static ArcherAsset[] Archers = new ArcherAsset[(int)Tribe.Count];
    public static KnightAsset[] Knights = new KnightAsset[(int)Tribe.Count];
    public static LeaderAsset[] Leaders = new LeaderAsset[(int)Tribe.Count];

    public static TerrainAsset[] TerrainAssets = new TerrainAsset[(int)Terrain.Count];
    public static TerrainBuffAsset[] TerrainBuffAssets = new TerrainBuffAsset[(int)TerrainBuff.Count];

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < (int)Terrain.Count; i++)
        {
            TerrainAssets[i] = new TerrainAsset();
            TerrainAssets[i].Sprite = TerrainSprites[i];
            TerrainAssets[i].Terrain = (Terrain)i;
        }
        // TODO: Uncomment from sprites
		//for (int i = 0; i < (int)Tribe.Count; i++)
  //      {
  //          Swordsmen[i].Sprite = SwordsmenSprites[i];
  //          Archers[i].Sprite = ArcherSprites[i];
  //          Knights[i].Sprite = KnightSprites[i];
  //          Leaders[i].Sprite = LeaderSprites[i];
  //      }
  //      for (int i = 0; i < (int)TerrainBuff.Count; i++)
  //      {

  //      }
    }
}

public enum Tribe
{
    Knight,
    Desert,
    Pirate,
    Nymph,
    Dwarf,
    Count
}

public class PawnAsset
{
    public Tribe Tribe;
    public Class Class;
    public Sprite Sprite;
    public int Attack;
    public int MaxHealth;
    public int AttackDistance;
}

public class LeaderAsset : PawnAsset
{
    public LeaderAsset()
    {
        Attack = 2;
        MaxHealth = 10;
        AttackDistance = 1;
    }
}

public class SwordsmanAsset : PawnAsset
{
    public SwordsmanAsset()
    {
        Attack = 2;
        MaxHealth = 2;
        AttackDistance = 1;
    }
}

public class ArcherAsset : PawnAsset
{
    public ArcherAsset()
    {
        Attack = 1;
        MaxHealth = 1;
        AttackDistance = 2;
    }
}

public class KnightAsset : PawnAsset
{
    public KnightAsset()
    {
        Attack = 3;
        MaxHealth = 4;
        AttackDistance = 1;
    }
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