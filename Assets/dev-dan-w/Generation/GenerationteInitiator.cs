using UnityEngine;
using UnityEngine.Tilemaps;

using FloorSystem;
using DungeonGeneration;
using System.Collections.Generic;

public class GenerationteInitiator : MonoBehaviour
{
    public Tilemap wallMap;
    public Tilemap floorMap;
    public Tilemap tileChunkDecorationMap;
    public Tilemap tileDecorationMap;
    public Tile[] Tileset;
    public AnimatedTile[] DecorationTilesetAnimated;
    public Tile[] DecorationTileset;

    public Tile[] TilesetOW;
    public Tile[] DecorationTilesetOW; 
    public Transform player;

    void Start()
    {
        DungeonFloor dungeon = new DungeonFloor(1, 15, 15);
        dungeon.GenerateLayout(true);
        dungeon.GenerateMap();

        Debug.Log(dungeon.floorMap.GetLength(0));
        Debug.Log(dungeon.floorMap.GetLength(1));

        int maxX = dungeon.floorMap.GetLength(0)*5;
        int maxY = dungeon.floorMap.GetLength(1)*5;


    
        // Set the player location to the spawn location
        float playerX = dungeon.spawnX*5+2.5f;
        float playerY = maxY - (float)(dungeon.spawnY)*5-2.5f;
        Vector3 playerPosition = new Vector3(playerX, playerY, 0);
        player.position = playerPosition;

        if(true) initChunks(dungeon);

        floorMap.RefreshAllTiles();
        wallMap.RefreshAllTiles();
        tileDecorationMap.RefreshAllTiles();
    }
    public void initChunks(Floor floor){

        int maxX = floor.floorMap.GetLength(0)*5;
        int maxY = floor.floorMap.GetLength(1)*5;

        for(int y = 0; y < floor.floorMap.GetLength(0); y++){
            for(int x = 0; x < floor.floorMap.GetLength(1); x++){
                int chunkY = y * 5;
                int chunkX = x * 5;
                Chunk chunk = floor.floorMap[y,x];
                if(true) {
                    setupOpenWorldChunk(chunk, maxX, maxY, chunkX, chunkY);
                }else {
                    setupDungeonChunk(chunk, maxX, maxY, chunkX, chunkY);
                }
            }
        }
    }

    public void setupOpenWorldChunk(Chunk chunk, int maxX, int maxY, int chunkX, int chunkY){
        HashSet<int> identifiersFloor = new HashSet<int> { 15, 16, 17, 20, 21, 22 };

        for(int y1 = 0; y1 < chunk.map.GetLength(0); y1++){
            for(int x1 = 0; x1 < chunk.map.GetLength(1); x1++){
                int totalY = maxY - chunkY - y1 -1;
                int totalX = chunkX + x1;
                
                // We are gonna have 2 positions, one for the tilemap/smalldecoration map and one for the chunk size decoration map
                Vector3Int position = new Vector3Int(totalX, totalY,0);
                Vector3Int positionChunk = new Vector3Int(totalX/5, totalY/5,0);
                int identififerTile = chunk.map[y1,x1];
                int identififerDecoration = chunk.decorationMap[y1,x1];

                // if its floor we need to add it to the floor map, if its wall we need to add it to the wall map
                if(identifiersFloor.Contains(identififerTile)) floorMap.SetTile(position, Tileset[identififerTile]);
                // else wallMap.SetTile(position, Tileset[identififerTile]);

                // if its 0 we dont need to do anything
                // if(identififerDecoration != 0){
                //     if(identififerDecoration == 3) tileChunkDecorationMap.SetTile(positionChunk, DecorationTilesetAnimated[(identififerDecoration-3)]);
                //     else tileDecorationMap.SetTile(position, DecorationTileset[(identififerDecoration-1)]);
                // } 
            }
        }
    }

    public void setupDungeonChunk(Chunk chunk, int maxX, int maxY, int chunkX, int chunkY){
        HashSet<int> identifiersFloor = new HashSet<int> { 15, 16, 17, 20, 21, 22 };

        for(int y1 = 0; y1 < chunk.map.GetLength(0); y1++){
            for(int x1 = 0; x1 < chunk.map.GetLength(1); x1++){
                int totalY = maxY - chunkY - y1 -1;
                int totalX = chunkX + x1;

                // We are gonna have 2 positions, one for the tilemap/smalldecoration map and one for the chunk size decoration map
                Vector3Int position = new Vector3Int(totalX, totalY,0);
                Vector3Int positionChunk = new Vector3Int(totalX/5, totalY/5,0);
                int identififerTile = chunk.map[y1,x1];
                int identififerDecoration = chunk.decorationMap[y1,x1];

                // if its floor we need to add it to the floor map, if its wall we need to add it to the wall map
                if(identifiersFloor.Contains(identififerTile)) floorMap.SetTile(position, Tileset[identififerTile]);
                else wallMap.SetTile(position, Tileset[identififerTile]);

                // if its 0 we dont need to do anything
                if(identififerDecoration != 0){
                    if(identififerDecoration == 3) tileChunkDecorationMap.SetTile(positionChunk, DecorationTileset[(identififerDecoration-3)]);
                    else tileDecorationMap.SetTile(position, DecorationTileset[(identififerDecoration-1)]);
                } 
            }
        }
    }
}

