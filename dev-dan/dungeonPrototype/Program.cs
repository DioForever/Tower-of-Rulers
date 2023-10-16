/*
 Layers Room Generation:
    0/1 - Start room, 90%, 3 guaranteed
    1/2 - 60%, 2 guaranteed, 2 guaranteed to increase distance from start
    2/3 - 40%, 2 guaranteed, 2 guaranteed to increase distance from start
    3/4 CANCELLED - 20%, 1 guaranteed, 1 guaranteed to increase distance from start

 Guardian Room:
    2/3 - 100%
    3/4 CANCELLED - 100%

 Grid showcase:
  S - Start room, always the the same one
  R - Standard room
  G - Guardian room, always only one in the dungeon
  + - Connector, connects one Room to another Room


 Room vs Connector
    Room - 12x12
    Connector - 5x3
    -----------------------
    LAYER 1
    | | | | | |
    | | | | | |
    | | |S| | |
    | | | | | |
    | | | | | |
    LAYER 2
    | | | | | | | | | |
    | | | | | | | | | |
    | | |-|-|-|-|-| | |
    | | |-| | | |-| | |
    | | |-| |S| |-| | |
    | | |-| | | |-| | |
    | | |-|-|-|-|-| | |
    | | | | | | | | | |
    | | | | | | | | | |
    LAYER 3
    |-|-|-|-|-|-|-|-|-|-|-|-|-|
    |-| | | | | | | | | | | |-|
    |-| |-|-|-|-|-|-|-|-|-| |-|
    |-| |-| | | | | | | |-| |-|
    |-| |-| |-|-|-|-|-| |-| |-|
    |-| |-| |-| | | |-| |-| |-|
    |-| |-| |-| |S| |-| |-| |-|
    |-| |-| |-| | | |-| |-| |-|
    |-| |-| |-|-|-|-|-| |-| |-|
    |-| |-| | | | | | | |-| |-|
    |-| |-|-|-|-|-|-|-|-|-| |-|
    |-| | | | | | | | | | | |-|
    |-|-|-|-|-|-|-|-|-|-|-|-|-|
    LAYER 4 CANCELLED
    // | | | | | | | | | | | | | | | | | |
    // | | | | | | | | | | | | | | | | | |
    // | | |-|-|-|-|-|-|-|-|-|-|-|-|-| | |
    // | | |-| | | | | | | | | | | |-| | |
    // | | |-| |-|-|-|-|-|-|-|-|-| |-| | |
    // | | |-| |-| | | | | | | |-| |-| | |
    // | | |-| |-| |-|-|-|-|-| |-| |-| | |
    // | | |-| |-| |-| | | |-| |-| |-| | |
    // | | |-| |-| |-| |S| |-| |-| |-| | |
    // | | |-| |-| |-| | | |-| |-| |-| | |
    // | | |-| |-| |-|-|-|-|-| |-| |-| | |
    // | | |-| |-| | | | | | | |-| |-| | |
    // | | |-| |-|-|-|-|-|-|-|-|-| |-| | |
    // | | |-| | | | | | | | | | | |-| | |
    // | | |-|-|-|-|-|-|-|-|-|-|-|-|-| | |
    // | | | | | | | | | | | | | | | | | |
    // | | | | | | | | | | | | | | | | | |

*/

using System;

namespace dungeonPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DungeonFloor dungeon = new DungeonFloor();
            dungeon.GenerateDungeon();
            dungeon.PrintDungeon();


        }

        class DungeonFloor
        {
            public int floorNumber;
            public int[][] layout = new int[33][];
            public DungeonFloor()
            {
                for (int i = 0; i < 33; i++)
                {
                    layout[i] = new int[33];
                }
                layout[16][16] = 1;
            }

            public void GenerateDungeon()
            {
                Random random = new Random();
                int roomCount = random.Next(10, 20);
                int succesRoomCount = 0;
                for (int tries = 0; tries < roomCount * 2 || succesRoomCount >= roomCount; tries++)
                {
                    int x = random.Next(0, 33);
                    int y = random.Next(0, 33);
                    int roomHeight = random.Next(4, 10);
                    int roomHWidth = random.Next(4, 10);
                    if (AttemptGenerateRoom(x, y, roomHeight, roomHWidth))
                    {
                        succesRoomCount++;
                    }

                }
            }

            private bool AttemptGenerateRoom(int x, int y, int roomHeight, int roomWidth)
            {
                bool canGenerate = true;

                for (int i = x; i < x + roomWidth; i++)
                {
                    for (int j = y; j < y + roomHeight; j++)
                    {
                        if (i > layout.Length - 1 || j > layout[0].Length - 1)
                        {
                            canGenerate = false;
                            return false;
                        }
                        if (layout[i][j] != 0)
                        {
                            canGenerate = false;
                            return false;

                        }
                    }
                }
                if (canGenerate)
                {
                    for (int i = x; i < x + roomWidth; i++)
                    {
                        if (i > layout.Length - 1 || x > layout[1].Length - 1)
                        {
                            canGenerate = false;
                            return false;

                        }
                        for (int j = y; j < y + roomHeight; j++)
                        {
                            layout[i][j] = 3;
                        }
                    }
                }
                layout[x][y] = 2;
                return true;
            }

            public void PrintDungeon()
            {
                for (int i = 0; i < layout.Length - 1; i++)
                {
                    for (int j = 0; j < layout[0].Length - 1; j++)
                    {

                        if (layout[i][j] == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                        else if (layout[i][j] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (layout[i][j] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (layout[i][j] == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        else if (layout[i][j] == 4)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write(layout[i][j] + " ");

                    }
                    Console.WriteLine();
                }
            }
        }

    }







}

