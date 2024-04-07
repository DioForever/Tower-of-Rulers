using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

using FloorSystem;
using DungeonGeneration;
using WorldGeneration;
using System.Collections.Generic;

public class GenerationteInitiator : MonoBehaviour
{
    // Tilemaps
    public Tilemap wallMap;
    public Tilemap floorMap;
    public Tilemap tileChunkDecorationMap;
    public Tilemap tileDecorationMap;
    public Tilemap tileLeafMap;
    public Tilemap tileGroundDecMap;

    // Dungeon tiles
    public Tile[] Tileset;
    public Tile[] DecorationTileset;

    // OpenWorld tiles
    public Tile[] TilesetOW;
    public RuleTile[] ruleTilesOW;
    public RuleTile[] ruleTilesGroundDec;
    public Tile[] treeTileset;
    public Tile[] leafsTileset;
    public Tile[] DecorationTilesetOW;
    public GameObject[] treeBasePrefabs;
    public Tile[] wallTileset;
    public GameObject[] treeLeafPrefabs;
    public GameObject[] buldingsPrefabs;
    public GameObject[] decorationPrefabs;

    // Shared
    public Transform player;
    public GameObject teleport;
    public AnimatedTile[] DecorationTilesetAnimated;

    public static Floor floor_;

    void Start()
    {
        LoadFloor(false, false, 1);

        floorMap.RefreshAllTiles();
        wallMap.RefreshAllTiles();
        tileDecorationMap.RefreshAllTiles();
    }

    public void LoadFloor(bool worldType, bool worldGenerated, int floorNumber)
    {
        // true = Dungeon, false = Open World

        SaveFloorType("floorType", worldType);

        if (!worldGenerated)
        {
            if (worldType)
            {
                // Its dungeon
                DungeonFloor floor = new DungeonFloor(floorNumber, 15, 15);
                floor.GenerateLayout(true);
                floor.GenerateMap();

                floor_ = floor;



                setPlayerLoc(floor);
                initChunks(floor, worldType);
            }
            else
            {
                // its open world
                WorldFloor floor = new WorldFloor(floorNumber, 75, 75);
                floor.GenerateMap();

                floor_ = floor;

                setPlayerLoc(floor);
                initChunks(floor, worldType);
            }
        }
        else
        {
            // Load the world
            if (worldType)
            {
                // Its dungeon - LOAD DUNEGON
                DungeonFloor floor = new DungeonFloor(1, 15, 15);
                floor.GenerateLayout(true);
                floor.GenerateMap();

                setPlayerLoc(floor);
                initChunks(floor, worldType);
            }
            else
            {
                // its open world - LOAD OPEN WORLD
                WorldFloor floor = new WorldFloor(10, 75, 75);
                floor.GenerateMap();

                setPlayerLoc(floor);
                initChunks(floor, worldType);
            }
        }


    }

    public void SaveFloorType(string saveKey, bool type)
    {
        PlayerPrefs.SetInt(saveKey, type ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void setPlayerLoc(Floor floor)
    {
        // Set the player location to the spawn location
        float playerX = floor.spawnX * 5 + 2.5f;
        float playerY = floor.floorMap.GetLength(1) * 5 - (float)(floor.spawnY) * 5 - 2.5f;
        Vector3 playerPosition = new Vector3(playerX, playerY, 0);
        player.position = playerPosition;
    }
    public void initChunks(Floor floor, bool type)
    {

        int maxX = floor.floorMap.GetLength(1) * 5;
        int maxY = floor.floorMap.GetLength(0) * 5;

        for (int y = 0; y < floor.floorMap.GetLength(0); y++)
        {
            for (int x = 0; x < floor.floorMap.GetLength(1); x++)
            {
                int chunkY = y * 5;
                int chunkX = x * 5;
                Chunk chunk = floor.floorMap[y, x];
                if (type)
                {
                    setupDungeonChunk(chunk, maxX, maxY, chunkX, chunkY);
                }
                else
                {
                    // Spawn teleport in the middle of the world
                    if (x == floor.floorMap.GetLength(1) / 2 && y == floor.floorMap.GetLength(0) / 2)
                    {
                        float spawnX = floor.spawnX * 5 + 2.5f;
                        float spawnY = floor.floorMap.GetLength(1) * 5 - (float)(floor.spawnY) * 5 - 2.5f;
                        UtilsOW.LoadTeleport(tileDecorationMap, spawnX, spawnY, teleport);
                    }
                    setupOpenWorldChunk(chunk, maxX, maxY, chunkX, chunkY);
                }
            }
        }

        // Setup the wall around the chunks, just the outer ones if its an Open World
        if (!type)
        {
            // TOP WALL
            for (int x = 0; x < maxX; x++)
            {
                wallMap.SetTile(new Vector3Int(x, maxY, 0), wallTileset[1]);
            }
            // BOTTOM WALL
            for (int x = 0; x < maxX; x++)
            {
                wallMap.SetTile(new Vector3Int(x, -1, 0), wallTileset[6]);
            }
            // LEFT WALL
            for (int y = 0; y < maxY; y++)
            {
                wallMap.SetTile(new Vector3Int(-1, y, 0), wallTileset[3]);
            }
            // RIGHT WALL
            for (int y = 0; y < maxY; y++)
            {
                wallMap.SetTile(new Vector3Int(maxX, y, 0), wallTileset[4]);
            }

            // TOP LEFT CORNER
            wallMap.SetTile(new Vector3Int(-1, maxY, 0), wallTileset[0]);
            // TOP RIGHT CORNER
            wallMap.SetTile(new Vector3Int(maxX, maxY, 0), wallTileset[2]);
            // BOTTOM LEFT CORNER
            wallMap.SetTile(new Vector3Int(-1, -1, 0), wallTileset[5]);
            // BOTTOM RIGHT CORNER
            wallMap.SetTile(new Vector3Int(maxX, -1, 0), wallTileset[7]);

        }
    }

    public void setupOpenWorldChunk(Chunk chunk, int maxX, int maxY, int chunkX, int chunkY)
    {
        HashSet<int> identifiersTrees = new HashSet<int> { 10, 11, 12, 13, 14, 21, 22, 23, 24, 31, 32, 33, 34, 41, 42, 43, 44, 51, 52, 53, 54 };
        for (int y1 = 0; y1 < chunk.map.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < chunk.map.GetLength(1); x1++)
            {
                int totalY = maxY - chunkY - y1 - 1;
                int totalX = chunkX + x1;

                // UtilsOW.LoadTree(tileDecorationMap, treeTileset, totalX, totalY, 0, 0);

                // We are gonna have 2 positions, one for the tilemap/smalldecoration map and one for the chunk size decoration map
                Vector3Int position = new Vector3Int(totalX, totalY, 0);
                Vector3Int positionChunk = new Vector3Int(totalX / 5, totalY / 5, 0);
                int identififerTile = chunk.map[y1, x1];
                int identififerDecoration = chunk.decorationMap[y1, x1];


                // if its ground we need to add it to the floor map
                if (identififerTile < ruleTilesOW.Length) floorMap.SetTile(position, ruleTilesOW[identififerTile]);

                // if its 0 we dont need to do anything

                if (identififerDecoration >= 1 && identififerDecoration <= 5)
                {
                    // Flowers, Grass
                    tileGroundDecMap.SetTile(position, ruleTilesGroundDec[(identififerDecoration - 1)]);
                }
                else if (identififerDecoration >= 6 && identififerDecoration <= 10)
                {
                    // Buildings
                    UtilsOW.LoadBuilding(tileDecorationMap, totalX, totalY, buldingsPrefabs[identififerDecoration - 6]);
                }
                else if (identififerDecoration == 15)
                {
                    // Monster campfire
                    UtilsOW.LoadDecoration(tileDecorationMap, totalX, totalY, decorationPrefabs[identififerDecoration - 15]);
                }
                else if (identifiersTrees.Contains(identififerDecoration))
                {
                    // Trees
                    string treeIds = identififerDecoration.ToString();
                    int baseId = int.Parse(treeIds[0].ToString());
                    int leafId = int.Parse(treeIds[1].ToString());
                    UtilsOW.LoadTree(tileDecorationMap, tileLeafMap, totalX - 2, totalY - 2, baseId, leafId, treeBasePrefabs[baseId - 1], treeLeafPrefabs[leafId]);
                }
            }
        }
    }

    public void setupDungeonChunk(Chunk chunk, int maxX, int maxY, int chunkX, int chunkY)
    {
        HashSet<int> identifiersFloor = new HashSet<int> { 15, 16, 17, 20, 21, 22 };

        for (int y1 = 0; y1 < chunk.map.GetLength(0); y1++)
        {
            for (int x1 = 0; x1 < chunk.map.GetLength(1); x1++)
            {
                int totalY = maxY - chunkY - y1 - 1;
                int totalX = chunkX + x1;

                // We are gonna have 2 positions, one for the tilemap/smalldecoration map and one for the chunk size decoration map
                Vector3Int position = new Vector3Int(totalX, totalY, 0);
                Vector3Int positionChunk = new Vector3Int(totalX / 5, totalY / 5, 0);
                int identififerTile = chunk.map[y1, x1];
                int identififerDecoration = chunk.decorationMap[y1, x1];

                // if its floor we need to add it to the floor map, if its wall we need to add it to the wall map
                if (identifiersFloor.Contains(identififerTile)) floorMap.SetTile(position, Tileset[identififerTile]);
                else wallMap.SetTile(position, Tileset[identififerTile]);

                // if its 0 we dont need to do anything
                if (identififerDecoration != 0)
                {
                    if (identififerDecoration == 3) tileChunkDecorationMap.SetTile(positionChunk, DecorationTileset[(identififerDecoration - 3)]);
                    else tileDecorationMap.SetTile(position, DecorationTileset[(identififerDecoration - 1)]);
                }
            }
        }
    }



}

