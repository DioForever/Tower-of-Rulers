using System;
using FloorSystem;


namespace DungeonGeneration
{

    // static class DungeonLayoutGenerator
    // {
    //     public static DungeonFloor InitDungeonLayout(int floorNumber, bool debug = false)
    //     {
    //         Chunk[,] DungeonMap = new Chunk[33, 33];
    //         DungeonFloor dungeon = new DungeonFloor(DungeonMap, floorNumber);
    //         if (debug) dungeon.PrintDungeon();
    //         return dungeon;
    //     }
    // }



    public partial class DungeonFloor : Floor
    {
        public Chunk[,] DungeonMap = new Chunk[33, 33];
        private static int[][] _Layout = new int[33][];
        public static Chunk[,] _DungeonMap = new Chunk[33, 33];
        // private int[][] roomLocations;


        public DungeonFloor(int floorNumber) : base(_DungeonMap, _Layout, floorNumber)
        {
            _DungeonMap = new Chunk[33, 33];
            for (int i = 0; i < 33; i++)
            {
                Layout[i] = new int[33];
            }
            Layout[15][15] = 1;
            Layout[15][16] = 1;
            Layout[16][15] = 1;
            Layout[17][16] = 1;
            Layout[16][17] = 1;
            Layout[17][17] = 1;
            Layout[15][17] = 1;
            Layout[17][15] = 1;
            Layout[16][16] = 2;
            // GenerateDungeon();



        }

        public static DungeonFloor GenerateLayout(int floorNumber, bool debug = false)
        {
            DungeonFloor dungeon = new DungeonFloor(floorNumber);
            if (debug) dungeon.PrintDungeon();
            return dungeon;
        }

        public void Generate()
        {
            GenerateDungeonLayout();
        }
        public int GenerateDungeonLayout()
        {
            // First we are gonna generate number of rooms we want
            Random random = new Random();
            int roomCount = random.Next(10, 20);
            int succesRoomCount = 0;
            List<int[]> roomLocationsTemp = new List<int[]>();
            // Generate guardian room first so we are sure it is generated
            int[] guardianLocation = GenerateGuardianRoom(7, 7);
            roomLocationsTemp.Add(new int[] { 16, 16 });
            roomLocationsTemp.Add(guardianLocation);

            // Now we loop through the rooms and try to generate them, but we dont want them to overlap
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
            // Now we have all the rooms generated, lets convert the list to array
            int[][] roomLocations = roomLocationsTemp.ToArray();

            // Generate hallways
            Hallways(roomLocations);

            // Lets return the dungeon
            return succesRoomCount;
        }
        private int[] GenerateGuardianRoom(int roomHeight, int roomWidth)
        {
            bool created = false;
            int[] positions = new int[] { 0, 0 };
            // Generate the room
            while (created == false)
            {
                // Lets get a random position from the middle
                positions = PositionFromMiddle(32, 15);
                // Lets loop through until we get a room, cuz we need Guardian room
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

        private void Hallways(int[][] roomLocations, int ammount = 3)
        {
            Dictionary<int, int[][]> connectedRooms = new Dictionary<int, int[][]>();
            for (int r = 0; r < roomLocations.Length; r++)
            {
                // Lets get closest rooms

                int[][] closest = GetClosestRooms(roomLocations[r][0], roomLocations[r][1], roomLocations, ammount);
                // Lets connect them
                for (int c = 0; c < 2; c++)
                {

                    ConnectHallways(roomLocations[r][0], roomLocations[r][1], closest[c][0], closest[c][1]);
                }

            }

        }

        private void ConnectHallways(int x1, int y1, int x2, int y2)
        {
            // Lets first check if its Guardian room, we want only 1 hallway to it
            // if (Layout[y2][x2] != 5 && Layout[y1][x1] != 5)
            // {
            //     // Lets check if there is already a hallway to it
            //     if (CheckHallways(x2, y2) == false || CheckHallways(x1, y1) == false)
            //     {
            //         return;
            //     }
            // }

            // Its same column or row we dont need to calculate anythin
            if (x1 == x2 || y1 == y2)
            {
                ConnectStraight(x1, y1, x2, y2);
                return;

            }

            // Lets get the third corner
            int[] corner = CalculateCorner(x1, y1, x2, y2);

            // lets connect the rooms to the corner
            ConnectStraight(x1, y1, corner[0], corner[1]);
            ConnectStraight(x2, y2, corner[0], corner[1]);


        }
        // private bool CheckHallways(int x, int y)
        // {
        //     for (int i = x - 1; i < x + 1; i++)
        //     {
        //         for (int j = y - 1; j < y + 1; j++)
        //         {
        //             if (Layout[i][j] == 7)
        //             {
        //                 return false;
        //             }
        //         }

        //     }
        //     return true;
        // }

        private void ConnectStraight(int x1, int y1, int x2, int y2, bool overwrite = false)
        {
            int startX = x1;
            int startY = y1;
            int endX = x2;
            int endY = y2;

            if (startX == endX)
            {
                // Vertical iteration
                int minY = Math.Min(startY, endY);
                int maxY = Math.Max(startY, endY);

                for (int y = minY; y <= maxY; y++)
                {
                    MarkLocation(startX, y, 7, overwrite);

                    // You can perform grid-related operations at each position here
                }
            }
            else if (startY == endY)
            {
                // Horizontal iteration
                int minX = Math.Min(startX, endX);
                int maxX = Math.Max(startX, endX);

                for (int x = minX; x <= maxX; x++)
                {
                    MarkLocation(x, startY, 7, overwrite);
                    // You can perform grid-related operations at each position here
                }
            }
            else
            {
                Console.WriteLine("This code handles only horizontal and vertical iterations.");
            }

            getRoomEntryPoints(startX, startY, endX, endY);


        }

        private void getRoomEntryPoints(int x1, int y1, int x2, int y2)
        {

            // MarkLocation(x1, y1, 8, true);
            // MarkLocation(x2, y2, 9, true);
            // Lets loop through the hallway and find the first entry point
            int startX = x1;
            int startY = y1;
            int endX = x2;
            int endY = y2;

            int xLast = 0;
            int yLast = 0;
            int identifierLast = 0;

            if (startX == endX)
            {
                // Vertical iteration
                int minY = Math.Min(startY, endY);
                int maxY = Math.Max(startY, endY);

                for (int y = minY; y <= maxY; y++)
                {
                    // This takes care of the first entry point
                    if (identifierLast == 4 || identifierLast == 6 || identifierLast == 1)
                    {
                        if (Layout[y][startX] == 7) MarkLocation(xLast, yLast, 9, true);

                    }
                    else if (identifierLast == 7)
                    {
                        if (Layout[y][startX] == 4 || Layout[y][startX] == 6 || Layout[y][startX] == 1) MarkLocation(startX, y, 9, true);
                    }
                    xLast = startX;
                    yLast = y;
                    identifierLast = Layout[y][startX];

                    // You can perform grid-related operations at each position here
                }
            }
            else if (startY == endY)
            {
                // Horizontal iteration
                int minX = Math.Min(startX, endX);
                int maxX = Math.Max(startX, endX);



                for (int x = minX; x <= maxX; x++)
                {
                    // This takes care of the first entry point
                    if (identifierLast == 4 || identifierLast == 6 || identifierLast == 1)
                    {
                        if (Layout[startY][x] == 7) MarkLocation(xLast, yLast, 9, true);

                    }
                    else if (identifierLast == 7)
                    {
                        if (Layout[startY][x] == 4 || Layout[startY][x] == 6 || Layout[startY][x] == 1) MarkLocation(x, startY, 9, true);
                    }
                    xLast = x;
                    yLast = startY;
                    identifierLast = Layout[startY][x];

                    // You can perform grid-related operations at each position here
                }
            }
            else
            {
                Console.WriteLine("This code handles only horizontal and vertical iterations.");
            }


        }

        private int[] CalculateCorner(int x1, int y1, int x2, int y2)
        {
            int x3 = 0;
            int y3 = 0;
            // 1 Its above 2
            if (y1 < y2)
            {
                x3 = x2;
                y3 = y1;
            }
            else
            {
                x3 = x1;
                y3 = y2;
            }

            return new int[] { x3, y3 };
        }

        private void MarkLocation(int x, int y, int layerIdentifier = 4, bool overwrite = false)
        {
            // if its empty we just add it

            if (Layout[y][x] == 0)
            {
                Layout[y][x] = layerIdentifier;
            }
            else if (overwrite == true)
            {
                Layout[y][x] = layerIdentifier;
            }
        }

        private int[][] GetClosestRooms(int x, int y, int[][] roomLocations, int ammount = 2)
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
                    if (i > Layout.Length - 1 || j > Layout[0].Length - 1)
                    {
                        return false;
                    }
                    if (Layout[j][i] != 0)
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
                        Layout[j][i] = layerIdentifier;
                    }
                }
            }
            Layout[y][x] = roomIdentifier;
            return true;
        }


        public void PrintDungeon()
        {
            for (int i = 0; i < Layout.Length - 1; i++)
            {
                for (int j = 0; j < Layout[0].Length - 1; j++)
                {

                    switch (Layout[i][j])
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
                    Console.Write(Layout[i][j] + " ");

                }
                Console.WriteLine();
            }
        }
    }
}



