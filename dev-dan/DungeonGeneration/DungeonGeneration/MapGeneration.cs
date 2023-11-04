using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FloorSystem;

namespace DungeonGeneration
{
    public partial class DungeonFloor
    {
        private void GenerateMap()
        {

        }



        private void GenerateChunk(int identifier, int x, int y)
        {
            switch (identifier)
            {
                case 0:
                    // Empty/Wall
                    Chunk chunkEmpty = new Chunk(new int[5, 5]
                    {
                        {0,0,0,0,0},
                        {0,0,0,0,0},
                        {0,0,0,0,0},
                        {0,0,0,0,0},
                        {0,0,0,0,0}
                    });

                    break;
                case 1:
                    // Spawn Room 
                    break;
                case 2:
                    // Spawn Room middle
                    break;
                case 3:
                    // Dungeon Room middle
                    break;
                case 4:
                    // Dungeon room
                    int[] wallSides = GetWallSides(identifier, x, y);
                    // Lets check if we need to add walls


                    break;
                case 5:
                    // Guardian room middle
                    break;
                case 6:
                    // Dungeon Room 
                    break;
                case 7:
                    // Hallway 
                    break;
                case 9:
                    // Hallway entry/exit point
                    break;
            }

        }

        private int[] GetWallSides(int identifier, int x, int y)
        {
            // We create an array of 4 ints, each int represents a side of the wall {top, right, bottom, left}
            int[] wallSides = new int[4] { 0, 0, 0, 0 };
            // First, lets check if we can even check for walls
            if (x - 1 < 0)
            {
                wallSides[3] = 0;
            }
            if (x + 1 > Layout.Length - 1)
            {
                wallSides[1] = 0;
            }
            if (y - 1 < 0)
            {
                wallSides[0] = 0;
            }
            if (y + 1 > Layout[0].Length - 1)
            {
                wallSides[2] = 0;
            }
            int top = y - 1;
            int right = x + 1;
            int bottom = y + 1;
            int left = x - 1;
            // Now we check for walls
            if (Layout[top][x] != identifier) wallSides[0] = 1;
            if (Layout[y][right] != identifier) wallSides[1] = 1;
            if (Layout[bottom][x] != identifier) wallSides[2] = 1;
            if (Layout[y][left] != identifier) wallSides[3] = 1;
            // We dont need to know corners, cuz we only take take or the sides, corners will appear as 2 walls

            return wallSides;
        }
        // private int[,] AddWallSides(int[] wallSides)
        // {
        //     int[,] roomLayout = new int[5, 5]{
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0}
        //             };
        //     if (wallSides[0] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[i, 0] = 1;
        //         }
        //     }
        //     if (wallSides[1] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[4, i] = 1;
        //         }
        //     }
        //     if (wallSides[2] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[i, 4] = 1;
        //         }
        //     }
        //     if (wallSides[3] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[0, i] = 1;
        //         }
        //     }
        // }

    }
}