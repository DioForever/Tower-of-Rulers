using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FloorSystem;
using LayoutGeneration;


namespace DungeonGeneration
{
    public class DungeonGeneration
    {
        public static void Main(string[] args)
        {

            DungeonFloor dungeon = DungeonLayoutGenerator.InitDungeonLayout(1, true);

            // TODO: Generate chunks from layout
        }

    }
}