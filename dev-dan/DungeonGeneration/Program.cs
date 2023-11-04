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

            DungeonFloor dungeon = DungeonFloor.InitDungeonLayout(1, true);
            dungeon.Generate();

            // TODO: Generate chunks from layout
        }

    }
}