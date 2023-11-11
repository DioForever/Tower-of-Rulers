using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using FloorSystem;

namespace DungeonGeneration
{
    public partial class DungeonFloor
    {
        /// <summary>
        /// Generates Dungeon Map from Layout, Layout is required to be already generated.
        /// Layout generation: GenerateLayout(); 
        /// </summary>
        public void GenerateMap()
        {


        }

        private int GetSecondIdentifier(int identifier)
        {

            switch (identifier)
            {
                case 1:
                    return 2;
                case 4:
                    return 3;
                case 6:
                    return 5;
                default:
                    return 2;

            }
        }


        private enum TilesIdentifiers
        {
            PUREWALL,
            FLOOR,
            LEFTWALL,
            TOPWALL,
            RIGHTWALL,
            BOTTOMWALL

        }

    }
}