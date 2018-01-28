using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameSetup : MonoBehaviour {
    
    public Tile TilePrefab;
    public Transform TileParent;

    static int SpriteSize = 16;

	public static GameSetup instance;

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
		if (GameSetup.instance == null)
        {
			GameSetup.instance = this;
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

        Player player1 = new Player();
        Player player2 = new Player();

		// Create starting pawns on both sides
		CreateStartingPawns (player1, player2);
        
	}

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
        Vector3 tileOffset = new Vector3((horzExtent) * 2 / xSize, (vertExtent) * 2 / ySize);

        // Get localScale for each tile
        Vector3 tileSize = new Vector3(tileOffset.x / tileSpriteSize.x, tileOffset.y / tileSpriteSize.y) ;
        
        float scaleDown = 0.8f;
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

        Vector2 tileParentPosition = new Vector2(-Screen.width / 2, -Screen.height / 2);
        tileParentPosition.x += (Screen.width - tileOffset.x * Map.Width) / 2;
        tileParentPosition.y += (Screen.height - tileOffset.y * Map.Height) / 2;
        TileParent.position = tileParentPosition;

        Map.Inst.GenerateMap();
	}

	// Creates starting pawns for both players
	public void CreateStartingPawns(Player p1, Player p2) {
        int TopRow = 0;
        int BottomRow = Map.Height - 1;
        int xCenter = (Map.Height + 1) / 2;

        // Leader set up
        Pawn p1Leader = new Pawn();
        p1Leader.AssetInfo = GameAssets.Leaders[(int)p1.Tribe];
        Map.Tiles[xCenter, BottomRow].Pawn = p1Leader;

        Pawn p2Leader = new Pawn();
        p2Leader.AssetInfo = GameAssets.Leaders[(int)p2.Tribe];
        Map.Tiles[xCenter, TopRow].Pawn = p2Leader;

        // Swordsman set up
        Pawn p1s1 = new Pawn(), p1s2 = new Pawn();
        p1s1.AssetInfo = p1s2.AssetInfo = GameAssets.Swordsmen[(int)p1.Tribe];
        Map.Tiles[xCenter - 1, BottomRow].Pawn = p1s1;
        Map.Tiles[xCenter + 1, BottomRow].Pawn = p1s2;

        Pawn p2s1 = new Pawn(), p2s2 = new Pawn();
        p2s1.AssetInfo = p2s2.AssetInfo = GameAssets.Swordsmen[(int)p2.Tribe];
        Map.Tiles[xCenter - 1, TopRow].Pawn = p2s1;
        Map.Tiles[xCenter + 1, TopRow].Pawn = p2s2;
    }
    
}


