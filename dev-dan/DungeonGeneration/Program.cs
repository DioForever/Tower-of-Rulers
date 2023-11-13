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
            System.Console.WriteLine($"Dungeon size: {dungeon.floorMap.GetLength(0) * 5} {dungeon.floorMap.GetLength(1) * 5} = {dungeon.floorMap.GetLength(0) * 5 * dungeon.floorMap.GetLength(1) * 5}");
            // dungeon.PrintMap();
            // dungeon.GenerateDungeonLayout();
            // dungeon.PrintDungeon();
            // dungeon.GenerateMap();
            // dungeon.GenerateMap();

            // TODO: Generate chunks from layout
        }

    }
}