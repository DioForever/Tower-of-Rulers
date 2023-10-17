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
                layout[15][15] = 1;
                layout[15][16] = 1;
                layout[16][15] = 1;
                layout[17][16] = 1;
                layout[16][17] = 1;
                layout[17][17] = 1;
                layout[15][17] = 1;
                layout[17][15] = 1;
                layout[16][16] = 1;

            }

            public void GenerateDungeon()
            {
                Random random = new Random();
                int roomCount = random.Next(10, 20);
                int succesRoomCount = 0;
                GenerateGuardianRoom(6, 6);
                for (int tries = 0; tries < roomCount * 4 || succesRoomCount >= roomCount; tries++)
                {
                    int x = random.Next(0, 33);
                    int y = random.Next(0, 33);
                    int roomHeight = random.Next(4, 6);
                    int roomHWidth = random.Next(4, 6);
                    if (AttemptGenerateRoom(x, y, roomHeight, roomHWidth))
                    {
                        succesRoomCount++;
                    }
                }
            }
            private void GenerateGuardianRoom(int roomHeight, int roomWidth)
            {
                Random random = new Random();
                bool created = false;

                // Generate the room
                while (!created)
                {
                    int[] positions = PositionFromMiddle(32, 15);
                    created = AttemptGenerateRoom(positions[0], positions[1], roomHeight, roomWidth, 4, 5);
                }
            }

            private static int[] PositionFromMiddle(int gridSize, int minDistance)
            {
                Random rand = new Random();

                while (true)
                {
                    int x = rand.Next(gridSize);
                    int y = rand.Next(gridSize);

                    double distance = Math.Sqrt(Math.Pow(x - gridSize / 2, 2) + Math.Pow(y - gridSize / 2, 2));

                    if (distance >= minDistance)
                    {
                        return new int[] { x, y };
                    }
                }
            }

            private void GenerateChestRooms(int ammount)
            {

            }

            private bool AttemptGenerateRoom(int x, int y, int roomHeight, int roomWidth, int roomIdentifier = 2, int layerIdentifier = 3)
            {

                // Check if it overlaps another room
                for (int i = x - 1; i < x + roomWidth + 1; i++)
                {
                    for (int j = y - 1; j < y + roomHeight + 1; j++)
                    {
                        if (i < 0 || j < 0)
                        {
                            return false;
                        }
                        if (i > layout.Length - 1 || j > layout[0].Length - 1)
                        {
                            return false;
                        }
                        if (layout[i][j] != 0)
                        {
                            return false;

                        }
                    }
                }


                // If it can be generated, generate it
                {
                    for (int i = x; i < x + roomWidth; i++)
                    {
                        for (int j = y; j < y + roomHeight; j++)
                        {
                            layout[i][j] = layerIdentifier;
                        }
                    }
                }
                layout[x][y] = roomIdentifier;
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
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else if (layout[i][j] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else if (layout[i][j] == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        else if (layout[i][j] == 4)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (layout[i][j] == 5)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        Console.Write(layout[i][j] + " ");

                    }
                    Console.WriteLine();
                }
            }
        }

    }







}

