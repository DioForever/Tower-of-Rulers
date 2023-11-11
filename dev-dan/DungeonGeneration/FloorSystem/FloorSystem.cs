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
        public Chunk[,] map;

        public Floor(int SizeX, int SizeY, int floorNumber, Chunk[,]? map = null)
        {
            this.floorNumber = floorNumber;
            if (map != null) this.map = map;
            else this.map = new Chunk[SizeY, SizeX];
        }

    }
    /// <summary>
    /// Collection of 5x5 (int) tile identifiers
    /// </summary>
    public class Chunk
    {
        public int[,] map
        {
            get
            {
                return map;
            }
            set
            {
                if (value.GetLength(0) == 5 && value.GetLength(1) == 5)
                {
                    map = value;
                }
                else throw new Exception("Chunk must be 5x5");
            }
        }
        public int[,] decorationLayout;
        // TODO: Add a way to store npc's and items in the chunk
        public Chunk(int[,] map, int[,]? decorationLayout = null)
        {
            this.map = map;
            if (decorationLayout != null) this.decorationLayout = decorationLayout;
            else this.decorationLayout = new int[map.GetLength(0), map.GetLength(1)];

        }
    }
}