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
            Chunk[][] map = new Chunk[33][];
            DungeonFloor dungeon = new DungeonFloor(map);
            dungeon.PrintDungeon();


        }



        class DungeonFloor : Floor
        {
            public Chunk[][] map = new Chunk[33][];
            // private int[][] roomLocations;

            public DungeonFloor(Chunk[][] map) : base(map)
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
                layout[16][16] = 2;
                GenerateDungeon();


            }

            public int GenerateDungeon()
            {
                Random random = new Random();
                int roomCount = random.Next(10, 20);
                int succesRoomCount = 0;
                List<int[]> roomLocationsTemp = new List<int[]>();
                // Generate guardian room first so we are sure it is generated
                int[] guardianLocation = GenerateGuardianRoom(6, 6);
                roomLocationsTemp.Add(new int[] { 16, 16 });
                roomLocationsTemp.Add(guardianLocation);
                Dictionary<int[], int[]> connectedRooms = new Dictionary<int[], int[]>();

                // Console.WriteLine(guardianLocation[0] + ", " + guardianLocation[1] + "guardian room");
                for (int tries = 0; (tries < roomCount * 4 || succesRoomCount >= roomCount) && tries <= 100; tries++)
                {
                    int x = random.Next(0, 33);
                    int y = random.Next(0, 33);
                    int roomHeight = random.Next(4, 7);
                    int roomHWidth = random.Next(4, 7);
                    if (AttemptGenerateRoom(x, y, roomHeight, roomHWidth))
                    {
                        succesRoomCount++;
                        roomLocationsTemp.Add(new int[] { x, y });
                    }
                }
                System.Console.WriteLine("Succesfully generated " + (succesRoomCount + 2) + " rooms");
                int[][] roomLocations = roomLocationsTemp.ToArray();
                for (int i = 0; i < roomLocations.Length; i++)
                {
                    System.Console.WriteLine(roomLocations[i][0] + ", " + roomLocations[i][1]);
                }

                // Generate hallways
                Hallways(roomLocations, connectedRooms);
                // for (int i = 0; i < roomLocations.Length; i++)
                // {
                //     // System.Console.WriteLine(roomLocations[i][0] + ", " + roomLocations[i][1]);
                //     int[][] closest = GetClosestRooms(roomLocations[i][0], roomLocations[i][1], roomLocations, 3);
                //     System.Console.WriteLine("Closest rooms to " + roomLocations[i][0] + ", " + roomLocations[i][1] + ": ");
                //     for (int j = 0; j < closest.Length; j++)
                //     {
                //         System.Console.WriteLine(j + " " + closest[j][0] + ", " + closest[j][1]);
                //     }
                //     // layout[roomLocations[i][0]][roomLocations[i][1]] = 0;
                // }
                return succesRoomCount;
            }
            private int[] GenerateGuardianRoom(int roomHeight, int roomWidth)
            {
                bool created = false;
                int[] positions = new int[] { 0, 0 };
                // Generate the room
                while (created == false)
                {
                    // System.Console.WriteLine("Try guardian");
                    positions = PositionFromMiddle(32, 15);
                    created = AttemptGenerateRoom(positions[0], positions[1], roomHeight, roomWidth, 5, 6);
                }
                // System.Console.WriteLine("Guardian room generated at: " + positions[0] + ", " + positions[1]);
                return positions;
            }

            private static int[] PositionFromMiddle(int gridSize, int minDistance)
            {

                while (true)
                {
                    Random rand = new Random();
                    int x = rand.Next(gridSize);
                    int y = rand.Next(gridSize);

                    double distance = Math.Sqrt(Math.Pow(x - gridSize / 2, 2) + Math.Pow(y - gridSize / 2, 2));

                    if (distance >= minDistance && distance <= 18.5f)
                    {
                        return new int[] { x, y };
                    }
                }
            }

            private void Hallways(int[][] roomLocations, Dictionary<int[], int[]> connectedRooms, int ammount = 3)
            {
                for (int r = 0; r < roomLocations.Length; r++)
                {
                    // Lets get closest rooms
                    int[][] closest = GetClosestRooms(roomLocations[r][0], roomLocations[r][1], roomLocations, ammount);
                    // Lets connect them
                    for (int c = 0; c < 3; c++)
                    {
                        ConnectHallways(roomLocations[r][0], roomLocations[r][1], closest[c][0], closest[c][1]);
                        break;
                    }
                    break;

                }

            }

            private void ConnectHallways(int x, int y, int x2, int y2)
            {
                // Its same column
                if (x == x2)
                {
                    // If its above go strait up
                    if (y < y2)
                    {
                        for (int i = y; i < y2; i++)
                        {
                            MarkLocation(x, i, 7);
                        }
                    }
                    else
                    {
                        for (int i = y2; i < y; i++)
                        {
                            MarkLocation(x, i, 7);
                        }
                    }
                    return;

                }
                // Its same row
                else if (y == y2)
                {
                    // If its to the left go strait left
                    if (x < x2)
                    {
                        for (int i = x; i < x2; i++)
                        {
                            MarkLocation(i, y, 7);
                        }
                    }
                    else
                    {
                        for (int i = x2; i < x; i++)
                        {
                            MarkLocation(i, y, 7);
                        }
                    }
                    return;
                }

                // Its not in the same row nor column
                int[] intersection = CalculateIntersection(x, y, x2, y2);
                System.Console.WriteLine(intersection[0] + ", " + intersection[1]);
                if (intersection[0] >= 0 && intersection[1] >= 0 && intersection[0] <= 31 && intersection[1] <= 31) { MarkLocation(intersection[0], intersection[1], 8); }
                MarkLocation(x, y, 8);
                MarkLocation(x2, y2, 8);
                // Lets get the intersection


            }

            private int[] CalculateIntersection(int x1, int y1, int x2, int y2)
            {
                int x3 = x1 + (y2 - y1);
                int y3 = y1 - (x2 - x1);

                int x4 = x2 + (y2 - y1);
                int y4 = y2 - (x2 - x1);


                return new int[] { x3, y3 };
            }

            private void MarkLocation(int x, int y, int layerIdentifier = 4)
            {
                if (layout[y][x] == 0) layout[y][x] = layerIdentifier;
            }

            private int[][] GetClosestRooms(int x, int y, int[][] roomLocations, int ammount = 3)
            {
                int[][] closestRooms = new int[ammount][];

                for (int i = 0; i < roomLocations.Length; i++)
                {
                    int distance = Math.Abs(x - roomLocations[i][0]) + Math.Abs(y - roomLocations[i][1]);

                    for (int j = 0; j < ammount; j++)
                    {
                        // if (closestRooms[j][0] != x && closestRooms[j][1] != y) continue;
                        if ((closestRooms[j] == null || distance < Math.Abs(x - closestRooms[j][0]) + Math.Abs(y - closestRooms[j][1])) && distance != 0)
                        {
                            closestRooms[j] = new int[2] { roomLocations[i][0], roomLocations[i][1] };
                            break;
                        }
                    }
                }
                return closestRooms;
            }


            private bool AttemptGenerateRoom(int x, int y, int roomHeight, int roomWidth, int roomIdentifier = 3, int layerIdentifier = 4)
            {

                // Check if it overlaps another room
                for (int i = x - roomWidth / 2 - 1; i < x + roomWidth / 2 + 1; i++)
                {
                    for (int j = y - roomHeight / 2 - 1; j < y + roomHeight / 2 + 1; j++)
                    {
                        if (i < 0 || j < 0)
                        {
                            return false;
                        }
                        if (i > layout.Length - 1 || j > layout[0].Length - 1)
                        {
                            return false;
                        }
                        if (layout[j][i] != 0)
                        {
                            return false;

                        }
                    }
                }


                // If it can be generated, generate it
                {
                    for (int i = x - roomWidth / 2; i < x + roomWidth / 2; i++)
                    {
                        for (int j = y - roomHeight / 2; j < y + roomHeight / 2; j++)
                        {
                            layout[j][i] = layerIdentifier;
                        }
                    }
                }
                layout[y][x] = roomIdentifier;
                return true;
            }

            public void PrintDungeon()
            {
                for (int i = 0; i < layout.Length - 1; i++)
                {
                    for (int j = 0; j < layout[0].Length - 1; j++)
                    {

                        switch (layout[i][j])
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
                        }
                        Console.Write(layout[i][j] + " ");

                    }
                    Console.WriteLine();
                }
            }
        }
        class Floor
        {
            public int floorNumber;
            public int[][] layout = new int[33][];
            public Chunk[][] map;

            public Floor(Chunk[][] map)
            {
                for (int i = 0; i < 33; i++)
                {
                    layout[i] = new int[33];
                }
                this.map = map;
            }

        }


        class Chunk
        {
            int[][] layout = new int[3][];
            // TODO: Add a way to store npc's and items in the chunk
            public Chunk()
            {
                for (int i = 0; i < 3; i++)
                {
                    layout[i] = new int[3];
                }
            }
        }

    }







}

