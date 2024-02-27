using UnityEngine;
using UnityEngine.Tilemaps;

using FloorSystem;
using DungeonGeneration;
using WorldGeneration;
using System.Collections.Generic;

public class GenerationteInitiator : MonoBehaviour
{
    public Tilemap wallMap;
    public Tilemap floorMap;
    public Tilemap tileChunkDecorationMap;
    public Tilemap tileDecorationMap;
    public Tilemap tileLeafMap;
    public Tile[] Tileset;
    public AnimatedTile[] DecorationTilesetAnimated;
    public Tile[] DecorationTileset;

    public Tile[] TilesetOW;
    public RuleTile[] ruleTilesOW;
    public Tile[] treeTileset;
    public Tile[] leafsTileset;
    public Tile[] DecorationTilesetOW;
    public Transform player;

    void Start()
    {
        bool worldType = false;
        // true = Dungeon, false = Open World
        // DungeonFloor dungeon = new DungeonFloor(1, 15, 15);
        // dungeon.GenerateLayout(true);
        // dungeon.GenerateMap();

        if (worldType)
        {
            // Its dungeon
            DungeonFloor floor = new DungeonFloor(1, 15, 15);
            floor.GenerateLayout(true);
            floor.GenerateMap();

            setPlayerLoc(floor);
            initChunks(floor, worldType);
        }
        else
        {
            // its open world
            WorldFloor floor = new WorldFloor(10, 15, 15);
            floor.GenerateMap();

            setPlayerLoc(floor);
            initChunks(floor, worldType);
        }


        floorMap.RefreshAllTiles();
        wallMap.RefreshAllTiles();
        tileDecorationMap.RefreshAllTiles();
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

        int maxX = floor.floorMap.GetLength(0) * 5;
        int maxY = floor.floorMap.GetLength(1) * 5;

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
                    setupOpenWorldChunk(chunk, maxX, maxY, chunkX, chunkY);
                }
            }
        }
    }

    public void setupOpenWorldChunk(Chunk chunk, int maxX, int maxY, int chunkX, int chunkY)
    {

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
                floorMap.SetTile(position, ruleTilesOW[identififerTile]);

                // if its 0 we dont need to do anything

                if (identififerDecoration > 10 && identififerDecoration <= 54)
                {
                    string treeIds = identififerDecoration.ToString();
                    int baseId = int.Parse(treeIds[0].ToString());
                    int leafId = int.Parse(treeIds[1].ToString());
                    UtilsOW.LoadTree(tileDecorationMap, tileLeafMap, treeTileset, leafsTileset, totalX - 2, totalY - 2, baseId, leafId);
                }
                else if (identififerDecoration != 0)
                {
                    if (identififerDecoration == 3) tileChunkDecorationMap.SetTile(positionChunk, DecorationTileset[(identififerDecoration - 3)]);
                    else tileDecorationMap.SetTile(position, DecorationTileset[(identififerDecoration - 1)]);
                }
            }
        }

        // int totalYY = maxY - chunkY;
        // int totalXX = chunkX;
        // UtilsOW.LoadTree(tileDecorationMap, tileLeafMap, treeTileset, leafsTileset, totalXX, totalYY, 1, 2);

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

