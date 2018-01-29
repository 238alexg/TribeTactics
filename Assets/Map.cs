using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    #region Map Size
    public const int Width = 7;
    public const int Height = 9;
    #endregion

    #region Map Gen: Mountains
    Location[] MountainSeeds;
    bool AllowLinearMountains = false;
    const int MountainRangeValidDistance = 3;
    const int MinMountainRanges = 2, MaxMountainRanges = 3;
    const int MinMountainsPerRange = 2, MaxMountainsPerRange = 3;
    #endregion

    #region Map Gen: Desert
    const int MinExtraDeserts = 1, MaxExtraDeserts = 2;
    #endregion

    #region Map Gen: Forest
    const int MinExtraForests = 1, MaxExtraForests = 2;
    #endregion

    #region Map Gen: Water
    const int MinWaterBodies = 3, MaxWaterBodies = 5;
    const int MinWaterLength = 2, MaxWaterLength = 3;
    const int MinExtraWaters = 1, MaxExtraWaters = 2;
    #endregion

    public static Tile[,] Tiles = new Tile[Width, Height];
    public static Map Inst;

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
    
    public void GenerateMap()
    {
        GenerateMountainRanges();
        GenerateExtraForestAndDesert();
        GenerateWater();
        PlaceMines();
        //PrettyPrintMap();
    }

    void GenerateMountainRanges()
    {
        int mountainRanges = Random.Range(MinMountainRanges, MaxMountainRanges + 1);

        MountainSeeds = new Location[mountainRanges];
        MountainSeeds[0] = new Location(Random.Range(1, Width), Random.Range(1, Height));
        
        for (int i = 1; i < mountainRanges; i++)
        {
            PlaceMountain(i);
        }

        PlaceMountainRanges();
    }

    void PlaceMountain(int mountainIndex)
    {
        int breakCount = 300;
        bool hasPlaced = false;
        while (!hasPlaced)
        {
            MountainSeeds[mountainIndex].X = Random.Range(1, Width);
            MountainSeeds[mountainIndex].Y = Random.Range(1, Height);

            if (IsValidMountain(mountainIndex) || breakCount-- < 0)
            {
                hasPlaced = true;
            }
        }
    }

    bool IsValidMountain(int curMountainIndex)
    {
        for (int j = 0; j < curMountainIndex; j++)
        {
            if (MountainSeeds[j].WithinRangeOf(MountainSeeds[curMountainIndex], MountainRangeValidDistance, diagonal: false))
            {
                return false;
            }
        }
        return true;
    }

    void PlaceMountainRanges()
    {
        for (int i = 0; i < MountainSeeds.Length; i++)
        {
            int MountainsInRange = Random.Range(MinMountainsPerRange, MaxMountainsPerRange + 1);
            Direction forestDir = (Direction)Random.Range((int)Direction.North, (int)Direction.Count);

            Location lastMtLoc = MountainSeeds[i];

            Tiles[lastMtLoc.X, lastMtLoc.Y].Terrain = Terrain.Mountains;
            PlaceForestAndDesertAroundMountain(lastMtLoc, forestDir);

            Location newLoc;
            for (int j = 1; j < MountainsInRange; j++)
            {
                int minXRange = lastMtLoc.X > 2 ? -1 : 0;
                int maxXRange = lastMtLoc.X < Width - 2 ? 2 : 1;
                int minYRange = lastMtLoc.Y > 2 ? -1 : 0;
                int maxYRange = lastMtLoc.Y < Height - 2 ? 2 : 1;

                newLoc.X = lastMtLoc.X + Random.Range(minXRange, maxXRange);
                newLoc.Y = lastMtLoc.Y + Random.Range(minYRange, maxYRange);
                
                Tiles[newLoc.X, newLoc.Y].Terrain = Terrain.Mountains;
                PlaceForestAndDesertAroundMountain(newLoc, forestDir);

                lastMtLoc = newLoc;
            }
        }
    }
    
    void PlaceForestAndDesertAroundMountain(Location mountain, Direction forestDirection)
    {
        Location newForestLoc = GetNormalizedVectorFromDirection(forestDirection);
        Location newDesertLoc = GetNormalizedVectorFromDirection((Direction)(((int)forestDirection + 2) % 4));

        newForestLoc += mountain;
        newDesertLoc += mountain;

        if (IsInMapBounds(newForestLoc, 0, 1) &&
            Tiles[newForestLoc.X, newForestLoc.Y].Terrain != Terrain.Mountains &&
            Random.Range(0, 5) != 0)
        {
            Tiles[newForestLoc.X, newForestLoc.Y].Terrain = Terrain.Forest;
        }

        if (IsInMapBounds(newDesertLoc, 0, 1) &&
            Tiles[newDesertLoc.X, newDesertLoc.Y].Terrain != Terrain.Mountains &&
            Random.Range(0, 5) != 0)
        {
            Tiles[newDesertLoc.X, newDesertLoc.Y].Terrain = Terrain.Desert;
        }
    }

    void GenerateExtraForestAndDesert()
    {
        int randForestCount = 0, randDesertCount = 0;

        for(int i = 0; i < Width; i++)
        {
            for (int j = 1; j < Height - 1; j++)
            {
                if (Tiles[i,j].Terrain == Terrain.Dirt)
                {
                    Location potentialNewTerrain = new Location(i, j);
                    if (randForestCount < MaxExtraForests)
                    {
                        if (TryPlaceRandomTerrain(potentialNewTerrain, Terrain.Forest))
                        {
                            randForestCount++;
                        }
                    }
                    else if (randDesertCount < MaxExtraDeserts && Random.Range(0, 20) == 0)
                    {
                        if (TryPlaceRandomTerrain(potentialNewTerrain, Terrain.Desert))
                        {
                            randDesertCount++;
                        }
                    }
                }
            }
        }
    }

    bool TryPlaceRandomTerrain(Location location, Terrain type)
    {
        if (location.AllNeighborsAreType(Terrain.Dirt) || Random.Range(0, 30) == 0)
        {
            Tiles[location.X, location.Y].Terrain = type;
            if (location.HasNeighborOfType(Terrain.Dirt))
            {
                Location locationNeighbor = location.GetRandomNeighborOfType(Terrain.Dirt);
                Tiles[locationNeighbor.X, locationNeighbor.Y].Terrain = type;
            }
            return true;
        }
        return false;
    }
    
    void GenerateWater()
    {

    }

    void PlaceMines()
    {

    }
    
    void PrettyPrintMap()
    {
        for (int i = 0; i < Width; i++)
        {
            string line = "Row " + i + ": ";
            for(int j = 0; j < Height; j++)
            {
                line += Tiles[i,j].Terrain + (j == Height - 1 ? "" : " | ");
            }
            Debug.Log(line);
        }
    }

    Location GetRandomDirectionVector()
    {
        return GetNormalizedVectorFromDirection(GetRandomDirection());
    }

    Direction GetRandomDirection()
    {
        return (Direction)Random.Range((int)Direction.North, (int)Direction.Count);
    }

    Location GetNormalizedVectorFromDirection(Direction dir)
    {
        switch(dir)
        {
            case Direction.North:
                return new Location(0, 1);
            case Direction.East:
                return new Location(1, 0);
            case Direction.South:
                return new Location(0, -1);
            case Direction.West:
                return new Location(-1, 0);
            default:
                return new Location(0, 0);
        }
    }

    public static bool IsInMapBounds(Location location, int xEdgeConstraint = 0, int yEdgeConstraint = 0)
    {
        if (location.X < 0 + xEdgeConstraint || location.X >= Width - xEdgeConstraint)
        {
            return false;
        }
        else if (location.Y < 0 + yEdgeConstraint || location.Y >= Height - yEdgeConstraint)
        {
            return false;
        }
        return true;
    }

    public static bool IsInMapBounds(int x, int y, int xEdgeConstraint = 0, int yEdgeConstraint = 0)
    {
        if (x < 0 + xEdgeConstraint || x >= Width - xEdgeConstraint)
        {
            return false;
        }
        else if (y < 0 + yEdgeConstraint || y >= Height - yEdgeConstraint)
        {
            return false;
        }
        return true;
    }
}

public struct Location
{
    public int X;
    public int Y;

    public static Location[] FiveSlice = new Location[4]
    {
        new Location(0, 1), new Location(1, 0), new Location(0, -1), new Location(-1, 0)
    };
    
    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool WithinRangeOf(Location otherLoc, int distance, bool diagonal)
    {
        int xDistance = Mathf.Abs(otherLoc.X - X);
        int yDistance = Mathf.Abs(otherLoc.Y - Y);

        if (xDistance <= distance && yDistance <= distance) 
        {
            if (!diagonal)
            {
                return (xDistance + yDistance <= distance);
            }
            return true;
        }
        return false;
    }
    
    public bool HasNeighborOfType(Terrain terrainType)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Map.IsInMapBounds(i, j, 0, 1))
                {
                    if (Map.Tiles[i, j].Terrain == terrainType)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool HasNeighborOfType(Terrain terrainType, out Location location)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Map.IsInMapBounds(i, j, 0, 1))
                {
                    if (Map.Tiles[i, j].Terrain == terrainType)
                    {
                        location = new Location(i, j);
                        return true;
                    }
                }
            }
        }
        location = new Location();
        return false;
    }

    public bool AllNeighborsAreType(Terrain terrainType)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Map.IsInMapBounds(i, j, 0, 1))
                {
                    if (Map.Tiles[i, j].Terrain != terrainType)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public Location GetRandomNeighborOfType(Terrain terrainType)
    {
        List<Location> neighborsOfType = new List<Location>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Map.IsInMapBounds(i, j, 0, 1))
                {
                    if (Map.Tiles[i, j].Terrain == terrainType)
                    {
                        neighborsOfType.Add(new Location(i, j));
                    }
                }
            }
        }
        if (neighborsOfType.Count == 0)
        {
            return this;
        }

        return neighborsOfType[Random.Range(0, neighborsOfType.Count)];
    }

    public override string ToString()
    {
        return "X: " + X + ", Y: " + Y;
    }

    public static Location operator + (Location loc1, Location loc2)
    {
        return new Location(loc1.X + loc2.X, loc1.Y + loc2.Y);
    }
}

public enum Direction
{
    North,
    East,
    South,
    West,
    Count
}