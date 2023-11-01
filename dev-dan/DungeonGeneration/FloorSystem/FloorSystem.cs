using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloorSystem
{
    public class Floor
    {
        public int floorNumber;
        public int[][] Layout = new int[33][];
        public Chunk[,] map;

        public Floor(Chunk[,] map, int[][] Layout, int floorNumber)
        {
            this.Layout = Layout;
            this.floorNumber = floorNumber;
            this.map = map;
        }

    }
    public class Chunk
    {
        public int[][] layout = new int[3][];
        // TODO: Add a way to store npc's and items in the chunk
        public Chunk()
        {
            for (int i = 0; i < 3; i++)
            {
                layout[i] = new int[3];
            }
        }
    }
}