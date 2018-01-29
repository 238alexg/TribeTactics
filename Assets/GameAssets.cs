using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {

    public static GameAssets Inst;

    public Sprite[] TerrainSprites;
    public Sprite[] TerrainBuffSprites;
    
    public PawnAssetInfo[] Swordsmen;
    public PawnAssetInfo[] Archers;
    public PawnAssetInfo[] Knights;
    public PawnAssetInfo[] Leaders;

    public static TerrainAsset[] TerrainAssets = new TerrainAsset[(int)Terrain.Count];
    public static TerrainBuffAsset[] TerrainBuffAssets = new TerrainBuffAsset[(int)TerrainBuff.Count];

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

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < (int)Terrain.Count; i++)
        {
            TerrainAssets[i] = new TerrainAsset();
            TerrainAssets[i].Sprite = TerrainSprites[i];
            TerrainAssets[i].Terrain = (Terrain)i;
        }
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


