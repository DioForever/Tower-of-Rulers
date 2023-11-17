using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using FloorSystem;
using DungeonGeneration;
using System;

public class GenerationteInitiator : MonoBehaviour
{
    public Sprite[] TilesetDungeon;
    public Tilemap tilemap;
    public Tile[] Tileset;
    // Start is called before the first frame update
    void Start()
    {
        DungeonFloor dungeon = new DungeonFloor(1, 15, 15);
        dungeon.GenerateLayout(true);
        dungeon.GenerateMap();
        Debug.Log(dungeon.floorMap.GetLength(0));
        Debug.Log(dungeon.floorMap.GetLength(1));
        int maxX = dungeon.floorMap.GetLength(0)*5;
        int maxY = dungeon.floorMap.GetLength(1)*5;
        for(int y = 0; y < dungeon.floorMap.GetLength(0); y++){
            for(int x = 0; x < dungeon.floorMap.GetLength(1); x++){
                int chunkY = y * 5;
                int chunkX = x * 5;
                // Debug.Log($"{y}-{x} = {dungeon.floorMap[y,x]}");
                Chunk chunk = dungeon.floorMap[y,x];
                for(int y1 = 0; y1 < chunk.map.GetLength(0); y1++){
                    for(int x1 = 0; x1 < chunk.map.GetLength(1); x1++){
                        int totalY = maxY - chunkY - y1;
                        int totalX = chunkX + x1;
                        Vector3Int position = new Vector3Int(totalX, totalY,0);
                        int identififerTile = chunk.map[y1,x1];
                        if(identififerTile != 0) Debug.Log($"{totalY},{totalX} - {identififerTile}");
                        tilemap.SetTile(new Vector3Int(totalX, totalY, 0), Tileset[identififerTile]);
                    }
                }
            }
        }
        tilemap.RefreshAllTiles();
    }
}
