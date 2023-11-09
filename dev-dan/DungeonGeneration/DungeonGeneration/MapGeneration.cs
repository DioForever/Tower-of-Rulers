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
            for (int y = 0; y < Layout.Length; y++)
            {
                for (int x = 0; x < Layout[0].Length; x++)
                {

                    Chunk chunk = GenerateChunk(Layout[y][x], GetSecondIdentifier(Layout[y][x]), x, y);
                    _DungeonMap[y, x] = chunk;
                    // if (chunk.map[0, 0] == 1)
                    // {
                    //     PrintChunk(chunk);
                    // }
                }
            }

            Chunk[,] testMap = new Chunk[5, 5];
            for (int i = 1; i < 6; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    Chunk chunk = new Chunk((new int[5, 5]
                    {
                        {i,0,0,0,j},
                        {0,i,0,j,0},
                        {i,0,0,0,j},
                        {0,i,0,j,0},
                        {i,0,0,0,j}
                    }));

                    testMap[i - 1, j - 1] = chunk;
                }
            }
            PrintMap(testMap);

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

        private enum RoomIdentifier
        {
            r1 = 2,
            r4 = 3,
            r6 = 5,
            r0 = 0,
            r7 = 7

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

        /// <summary>
        /// Takes location (x,y) and identifier.
        /// Returns generated Chunk. Chunk is generated from Layout.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private Chunk GenerateChunk(int identifier, int secondIdentifier, int x, int y)
        {
            Chunk chunk = new Chunk(new int[5, 5]);
            switch (identifier)
            {
                case 0:
                    // Empty/Wall
                    chunk.map = (new int[5, 5]
                    {
                        {0,0,0,0,0},
                        {0,0,0,0,0},
                        {0,0,0,0,0},
                        {0,0,0,0,0},
                        {0,0,0,0,0}
                    });

                    break;
                case 2:
                    // Spawn Room middle
                    break;
                case 1:
                case 3:
                case 4:
                case 5:
                case 6:
                    // Spawn Room 
                    // Dungeon Room middle
                    // Dungeon room
                    // Guardian room middle
                    // Dungeon Room 
                    int[] wallSidesRooms = GetwallSidesRooms(identifier, secondIdentifier, x, y);
                    // System.Console.WriteLine($"wallSidesRooms for {y} {x} - {string.Join(",", wallSidesRooms)}");
                    int[,] chunkDRLayout = new int[5, 5];
                    if (wallSidesRooms[3] == 1)
                    {
                        // WALL
                        for (int chunkY = 0; chunkY < 5; chunkY++)
                        {
                            chunkDRLayout[chunkY, 1] = (int)TilesIdentifiers.LEFTWALL;
                        }
                        // PUREWALL LINE
                        for (int chunkY = 0; chunkY < 5; chunkY++)
                        {
                            chunkDRLayout[chunkY, 0] = (int)TilesIdentifiers.PUREWALL;
                        }


                    }
                    if (wallSidesRooms[1] == 1)
                    {
                        // WALL
                        for (int chunkY = 0; chunkY < 5; chunkY++)
                        {
                            chunkDRLayout[chunkY, 3] = (int)TilesIdentifiers.RIGHTWALL;
                        }
                        // PUREWALL LINE
                        for (int chunkY = 0; chunkY < 5; chunkY++)
                        {
                            chunkDRLayout[chunkY, 4] = (int)TilesIdentifiers.PUREWALL;
                        }

                    }
                    if (wallSidesRooms[0] == 1)
                    {
                        // WALL
                        for (int chunkX = 0; chunkX < 5; chunkX++)
                        {
                            chunkDRLayout[1, chunkX] = (int)TilesIdentifiers.TOPWALL;
                        }
                        // PUREWALL LINE
                        if (wallSidesRooms[3] == 1) chunkDRLayout[1, 0] = (int)TilesIdentifiers.PUREWALL;
                        if (wallSidesRooms[1] == 1) chunkDRLayout[1, 4] = (int)TilesIdentifiers.PUREWALL;
                        for (int chunkX = 0; chunkX < 5; chunkX++)
                        {
                            chunkDRLayout[0, chunkX] = (int)TilesIdentifiers.PUREWALL;
                        }

                    }
                    if (wallSidesRooms[2] == 1)
                    {
                        // WALL
                        for (int chunkX = 0; chunkX < 5; chunkX++)
                        {
                            chunkDRLayout[3, chunkX] = (int)TilesIdentifiers.BOTTOMWALL;
                        }
                        // PUREWALL LINE

                        if (wallSidesRooms[3] == 1) chunkDRLayout[3, 0] = (int)TilesIdentifiers.PUREWALL;
                        if (wallSidesRooms[1] == 1) chunkDRLayout[3, 4] = (int)TilesIdentifiers.PUREWALL;
                        for (int chunkX = 0; chunkX < 5; chunkX++)
                        {
                            chunkDRLayout[4, chunkX] = (int)TilesIdentifiers.PUREWALL;
                        }

                    }
                    chunk.map = chunkDRLayout;



                    break;
                case 7:
                    // Hallway

                    int[] wallSidesHallway = GetwallSidesRooms(identifier, 9, x, y);
                    // System.Console.WriteLine($"wallSidesRooms for {y} {x} - {string.Join(",", wallSidesRooms)}");
                    int[,] chunkHLayout = new int[5, 5];
                    // System.Console.WriteLine(string.Join(", ", wallSidesHallway));
                    if (wallSidesHallway[3] == 1)
                    {
                        // WALL
                        for (int chunkY = 0; chunkY < 5; chunkY++)
                        {
                            chunkHLayout[chunkY, 0] = (int)TilesIdentifiers.LEFTWALL;
                        }


                    }
                    if (wallSidesHallway[1] == 1)
                    {
                        // WALL
                        for (int chunkY = 0; chunkY < 5; chunkY++)
                        {
                            chunkHLayout[chunkY, 4] = (int)TilesIdentifiers.RIGHTWALL;
                        }

                    }
                    if (wallSidesHallway[0] == 1)
                    {
                        // WALL
                        for (int chunkX = 0; chunkX < 5; chunkX++)
                        {
                            chunkHLayout[0, chunkX] = (int)TilesIdentifiers.TOPWALL;
                        }
                        // PUREWALL LINE
                        // if (wallSidesHallway[3] == 1) chunkHLayout[0, 0] = (int)TilesIdentifiers.PUREWALL;
                        // if (wallSidesHallway[1] == 1) chunkHLayout[0, 4] = (int)TilesIdentifiers.PUREWALL;

                    }
                    if (wallSidesHallway[2] == 1)
                    {
                        // WALL
                        for (int chunkX = 0; chunkX < 5; chunkX++)
                        {
                            chunkHLayout[4, chunkX] = (int)TilesIdentifiers.BOTTOMWALL;
                        }
                        // PUREWALL LINE

                        // if (wallSidesHallway[3] == 1) chunkHLayout[4, 0] = (int)TilesIdentifiers.PUREWALL;
                        // if (wallSidesHallway[1] == 1) chunkHLayout[4, 4] = (int)TilesIdentifiers.PUREWALL;

                    }
                    chunk.map = chunkHLayout;
                    // PrintChunk(chunk);
                    break;
                case 9:
                    // Hallway entry/exit point
                    chunk.map = (new int[5, 5]
                    {
                        {9,9,9,9,9},
                        {9,9,9,9,9},
                        {9,9,9,9,9},
                        {9,9,9,9,9},
                        {9,9,9,9,9}
                    });
                    break;
            }
            return chunk;

        }

        private void PrintChunk(Chunk chunk)
        {
            for (int y = 0; y < chunk.map.GetLength(0); y++)
            {
                for (int x = 0; x < chunk.map.GetLength(1); x++)
                {
                    System.Console.Write(chunk.map[y, x]);
                }
                System.Console.WriteLine();
            }
        }

        private void PrintMap(Chunk[,] mapLayout)
        {
            System.Console.WriteLine();
            // int chunkY = 0;
            // int chunkYMax = mapLayout.GetLength(0);
            // int chunkX = 0;
            // int chunkXMax = mapLayout.GetLength(1);
            int y = 0;
            int x = 0;

            for (int chunkY = 0; chunkY < mapLayout.GetLength(0); chunkY++)
            {
                for (int chunkX = 0; chunkX < mapLayout.GetLength(1) * 5; chunkX++)
                {
                    // Lets get chunk at [chunkY, chunkX]
                    int[,] chunkMap = mapLayout[chunkY, chunkX % 5].map;
                    for (int i = 0; i < 5; i++)
                    {
                        PrintChunkOnMap(chunkMap[y, i]);
                    }
                    System.Console.Write(" ");
                    // System.Console.WriteLine(chunkY + " " + chunkX);





                    if (chunkX % 5 == 0 && chunkX != 0) System.Console.WriteLine();
                }
                y++;


                // break;


            }
        }

        private static void PrintChunkOnMap(int identifier)
        {
            switch (identifier)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 9:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }
            Console.Write(identifier);
        }

        /// <summary>
        /// Takes location (x,y) and identifier.
        /// Returns at what sides there is a wall (TOP, RIGHT, BOTTOM, LEFT)
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int[] GetwallSidesRooms(int identifier, int secondIdentifier, int x, int y)
        {


            // We create an array of 4 ints, each int represents a side of the wall {top, right, bottom, left}
            int[] wallSidesRooms = new int[4] { 0, 0, 0, 0 };
            // First, lets check if we can even check for walls
            if (x - 1 < 0)
            {
                wallSidesRooms[3] = 0;
            }
            if (x + 1 > Layout.Length - 1)
            {
                wallSidesRooms[1] = 0;
            }
            if (y - 1 < 0)
            {
                wallSidesRooms[0] = 0;
            }
            if (y + 1 > Layout[0].Length - 1)
            {
                wallSidesRooms[2] = 0;
            }
            int top = y - 1;
            int right = x + 1;
            int bottom = y + 1;
            int left = x - 1;
            // Now we check for walls
            if (Layout[top][x] != identifier && Layout[top][x] != secondIdentifier && Layout[top][x] != 9) wallSidesRooms[0] = 1;
            if (Layout[y][right] != identifier && Layout[y][right] != secondIdentifier && Layout[y][right] != 9) wallSidesRooms[1] = 1;
            if (Layout[bottom][x] != identifier && Layout[bottom][x] != secondIdentifier && Layout[bottom][x] != 9) wallSidesRooms[2] = 1;
            if (Layout[y][left] != identifier && Layout[y][left] != secondIdentifier && Layout[y][left] != 9) wallSidesRooms[3] = 1;
            // We dont need to know corners, cuz we only take take or the sides, corners will appear as 2 walls

            return wallSidesRooms;
        }
        // private int[,] AddwallSidesRooms(int[] wallSidesRooms)
        // {
        //     int[,] roomLayout = new int[5, 5]{
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0},
        //                 {0,0,0,0,0}
        //             };
        //     if (wallSidesRooms[0] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[i, 0] = 1;
        //         }
        //     }
        //     if (wallSidesRooms[1] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[4, i] = 1;
        //         }
        //     }
        //     if (wallSidesRooms[2] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[i, 4] = 1;
        //         }
        //     }
        //     if (wallSidesRooms[3] == 1)
        //     {
        //         for (int i = 0; i < 5; i++)
        //         {
        //             roomLayout[0, i] = 1;
        //         }
        //     }
        // }

    }
}