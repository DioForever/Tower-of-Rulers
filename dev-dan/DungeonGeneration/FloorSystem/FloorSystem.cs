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
        public int[,] mapLayout = new int[5, 5];
        public int[,] decorationLayout = new int[5, 5];
        // TODO: Add a way to store npc's and items in the chunk
        public Chunk(int[,] mapLayout)
        {
            this.mapLayout = mapLayout;
        }
    }
}