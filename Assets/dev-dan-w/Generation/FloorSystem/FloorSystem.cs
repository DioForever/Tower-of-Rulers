using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FloorSystem
{
    /// <summary>
    /// A floor is a collection of chunks that make up a floor 
    /// </summary>
    public class Floor
    {
        public int floorNumber;
        public Chunk[,] floorMap;
        public int spawnX;
        public int spawnY;
        public int playerX;
        public int playerY;
        public int exitX;
        public int exitY;

        public Floor(int SizeX, int SizeY, int floorNumber, Chunk[,] floorMap = null)
        {
            this.floorNumber = floorNumber;
            if (floorMap != null) this.floorMap = floorMap;
            else this.floorMap = new Chunk[SizeY, SizeX];
            
        }

    }
    /// <summary>
    /// Collection of 5x5 (int) tile identifiers
    /// </summary>
    public class Chunk
    {
        public int[,] map;
        public int[,] decorationMap;
        public List<TempNpc> npcs;
        // TODO: Add a way to store npc's and items in the chunk (Require Monsters to be created first, and then I can store them)
        // DISCLAIMER: This todo wasnt forgotten, because ther arent monsters, I can only implement locations for the npcs
        public Chunk(int[,] map = null, int[,] decorationMap = null)
        {
            if (map != null) this.map = map;
            else this.map = new int[5, 5]{
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0}
            };
            if (decorationMap != null) this.decorationMap = decorationMap;
            else this.decorationMap = new int[5, 5] {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0}
            };

            // List of npcs
            npcs = new List<TempNpc>();
        }
    }

    public class TempNpc
    {
        public int x;
        public int y;
        public int id;
        public TempNpc(int x, int y, int id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }
    }
}