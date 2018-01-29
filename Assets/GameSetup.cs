using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameSetup : MonoBehaviour {
    
    public Tile TilePrefab;
    public Pawn PawnPrefab;
    public Transform TileParent;
    public Transform PawnParent;

    static int SpriteSize = 16;

	public static GameSetup Inst;

    public float OrthographicSize
    {
        get { return Camera.main.orthographicSize; }
        set { Camera.main.orthographicSize = value; }
    }

	public float HeightToWidthRatio
    {
		get { return Screen.height / Screen.width; }
	}

    int xSize, ySize;
    const int SmallMapSize = 0, MediumMapSize = 1, LargeMapSize = 2;
    const int DefaultPawnSetup = 0;
    const int PixPerUnit = 1;

    Vector2 SpriteScale = new Vector2(Screen.width / Map.Width, Screen.width / Map.Width);

    Vector2 PixelPerfectScale = new Vector2(SpriteSize, SpriteSize);

	[System.NonSerialized]
	public float horzExtent, xyGridRatio;

	void Awake()
    {
		if (GameSetup.Inst == null)
        {
			GameSetup.Inst = this;
            Camera.main.orthographicSize = Screen.height / 2;
        }
        else
        {
			Destroy (this);
		}
	}

	public void SetUpGame () {

		// Create the board and tiles
		CreateBoard ();

		// Create starting pawns on both sides
		CreateStartingPawns (GameplayManager.Inst.Player1, GameplayManager.Inst.Player2);
	}

    Vector2 tileOffset;
    Vector2 tileSize;
    float scaleDown;
    Vector3 tileParentPosition;

	/// <summary>
	/// Creates the board.
	/// </summary>
	/// <param name="mapSize">Map size struct.</param>
	/// Adapted from Ray Wenderlich's tile game tutorial:
	/// https://www.raywenderlich.com/152282/how-to-make-a-match-3-game-in-unity
	public void CreateBoard () {
		xSize = Map.Width;
		ySize = Map.Height;

        float vertExtent = Camera.main.orthographicSize;

        // Get new ratio of horz extent: world space of camera height *  grid width/height
        horzExtent = vertExtent * xSize / ySize;

        // Get size of tile sprite
        Vector2 tileSpriteSize = TilePrefab.GetComponent<SpriteRenderer>().size;

        // Get offset for each tile
        tileOffset = new Vector3((horzExtent) * 2 / xSize, (vertExtent) * 2 / ySize);

        // Get localScale for each tile
        tileSize = new Vector3(tileOffset.x / tileSpriteSize.x, tileOffset.y / tileSpriteSize.y) ;
        
        float scaleDown = 0.65f;
        tileSize *= scaleDown;
        tileOffset *= scaleDown;

        // Make a x by y grid of tiles
        for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
                Vector3 tilePos = new Vector3(x * tileOffset.x, (ySize - y) * tileOffset.y);
                Tile newTile = Instantiate(TilePrefab, tilePos, TilePrefab.transform.rotation, TileParent);
                newTile.transform.localScale = tileSize;
                newTile.Location.X = x;
                newTile.Location.Y = y;
				Map.Tiles[x, y] = newTile;
			}
		}

        tileParentPosition = new Vector3(-Screen.width / 2, -Screen.height / 2, 10);
        tileParentPosition.x += (Screen.width - tileOffset.x * Map.Width) / 2;
        tileParentPosition.y += (Screen.height - tileOffset.y * Map.Height) / 2;
        TileParent.position = tileParentPosition;

        Map.Inst.GenerateMap();
	}

	// Creates starting pawns for both players
	public void CreateStartingPawns(Player p1, Player p2) {
        int TopRow = 0;
        int BottomRow = Map.Height - 1;
        int xCenter = (Map.Width - 1) / 2;

        print("Tribes: " + p1.Tribe + ":" + (int)p1.Tribe + ", P2: " + p2.Tribe + ":" + (int)p2.Tribe);


        // Leader set up
        InstantiatePawnAt(Map.Tiles[xCenter, BottomRow], GameAssets.Inst.Leaders[(int)p1.Tribe], p1);
        InstantiatePawnAt(Map.Tiles[xCenter, TopRow], GameAssets.Inst.Leaders[(int)p2.Tribe], p2);

        // Swordsman set up
        InstantiatePawnAt(Map.Tiles[xCenter - 1, BottomRow], GameAssets.Inst.Swordsmen[(int)p1.Tribe], p1);
        InstantiatePawnAt(Map.Tiles[xCenter + 1, BottomRow], GameAssets.Inst.Swordsmen[(int)p1.Tribe], p1);

        InstantiatePawnAt(Map.Tiles[xCenter - 1, TopRow], GameAssets.Inst.Swordsmen[(int)p2.Tribe], p2);
        InstantiatePawnAt(Map.Tiles[xCenter + 1, TopRow], GameAssets.Inst.Swordsmen[(int)p2.Tribe], p2);
    }


    public Pawn InstantiatePawnAt(Tile tile, PawnAssetInfo prefab, Player owner)
    {
        Vector3 pos = tile.transform.position;
        pos.z = -1;
        Pawn newPawn = Instantiate(prefab.GetComponent<Pawn>(), pos, PawnPrefab.transform.rotation, PawnParent);
        newPawn.transform.localScale = tileSize;
        newPawn.Location.X = tile.Location.X;
        newPawn.Location.Y = tile.Location.Y;

        newPawn.AssetInfo = prefab;
        newPawn.Owner = owner;

        if (prefab.Class == Class.Swordsman) { owner.Swordsmen.Add(newPawn); }
        else { owner.Leader = newPawn; }
        
        Map.Tiles[tile.Location.X, tile.Location.Y].Pawn = newPawn;
        
        return newPawn;
    }

}


