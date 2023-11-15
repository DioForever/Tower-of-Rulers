using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FloorSystem;
using DungeonGeneration;

namespace DungeonGeneration
{
    public class DungeonGeneration
    {
        public static void Main(string[] args)
        {

            DungeonFloor dungeon = new DungeonFloor(1, 15, 15);
            dungeon.GenerateLayout(true);
            dungeon.GenerateMap();
            dungeon.PrintMap(null, true);
        }

    }
}