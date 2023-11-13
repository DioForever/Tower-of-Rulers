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

        public Floor(int SizeX, int SizeY, int floorNumber, Chunk[,]? floorMap = null)
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
        public int[,] decorationLayout;
        // TODO: Add a way to store npc's and items in the chunk
        public Chunk(int[,] map = null, int[,]? decorationLayout = null)
        {
            if (map != null) this.map = map;
            else this.map = new int[3, 3];
            if (decorationLayout != null) this.decorationLayout = decorationLayout;
            else this.decorationLayout = new int[3, 3];
        }
    }
}