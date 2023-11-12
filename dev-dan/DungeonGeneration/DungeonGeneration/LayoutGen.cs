using System;
using FloorSystem;


namespace DungeonGeneration
{
    /// <summary>
    /// A Dungeon floor (Floor) is a collection of chunks that make up a floor and a layout of the floor
    /// </summary>
    public partial class DungeonFloor : Floor
    {
        public Chunk[,] DungeonMap;
        private static int[,]? Layout;
        private Room[] Rooms;
        // private int[][] roomLocations;


        public DungeonFloor(int floorNumber, int SizeY, int SizeX) : base(SizeX, SizeY, floorNumber)
        {
            Layout = new int[SizeY, SizeX];
            DungeonMap = new Chunk[0, 0];
            Rooms = new Room[0];
            // GenerateDungeon();
        }

        enum RoomIdentifiers
        {
            WALL = 0,
            SPAWNMIDDLE = 4,
            SPAWNROOM = 3,
            ROOMMIDDLE = 2,
            ROOM = 1,
            GUARDIANROOM = 5,
            GUARDIANROOMMIDDLE = 6,
            HALLWAY = 7
        }

        public void GenerateLayout(bool debug = false)
        {
            GenerateRooms();
            GenerateHallways();
            if (debug) PrintLayout();

        }
        public void GenerateRooms()
        {
            Random rand = new Random();
            if (Layout == null) throw new Exception("Layout is null");

            // Generating the middle points of the rooms
            int roomCount = rand.Next(7, 10);
            Rooms = new Room[roomCount + 2];
            // Generate spawn room
            int[] spawnRoomLocation = new int[2];
            spawnRoomLocation[0] = rand.Next(1, map.GetLength(0) - 1);
            spawnRoomLocation[1] = rand.Next(1, map.GetLength(1) - 1);
            GenerateRoom(spawnRoomLocation, rand, 0, (int)RoomIdentifiers.SPAWNROOM, 3, 3);
            // Generate guardian room
            int[] guardianRoomLocation = new int[2];
            for (int i = 0; i < 1;)
            {
                int tries = 0;
                bool distanceNotReached = true;
                while (distanceNotReached)
                {
                    tries++;
                    guardianRoomLocation[0] = rand.Next(1, map.GetLength(0) - 1);
                    guardianRoomLocation[1] = rand.Next(1, map.GetLength(1) - 1);
                    double distance = Math.Sqrt(Math.Pow(guardianRoomLocation[0] - Rooms[0].location[0], 2) + Math.Pow(guardianRoomLocation[1] - Rooms[0].location[1], 2));
                    if (distance >= map.GetLength(0) / 2 && distance <= Math.Sqrt(Math.Pow(guardianRoomLocation[0] + 12 - Rooms[0].location[0], 2) + Math.Pow(guardianRoomLocation[1] + 12 - Rooms[0].location[1], 2)))
                    {
                        distanceNotReached = false;
                    }
                    if (tries > 500)
                    {
                        distanceNotReached = false;
                        System.Console.WriteLine("Tries exceeded");
                    }
                }
                i = GenerateRoom(guardianRoomLocation, rand, 1, (int)RoomIdentifiers.GUARDIANROOM, 5, 5);
            }


            // Generate other rooms
            for (int i = 2; i < roomCount + 2;)
            {
                int[] roomLocation = new int[2];
                roomLocation[0] = rand.Next(1, map.GetLength(0) - 1);
                roomLocation[1] = rand.Next(1, map.GetLength(1) - 1);
                i = GenerateRoom(roomLocation, rand, i);
            }

        }

        public int GenerateRoom(int[] roomLocation, Random rand, int i, int identifier = (int)RoomIdentifiers.ROOM, int sizeX = -1, int sizeY = -1)
        {
            if (Layout == null) throw new Exception("Layout is null");
            if (Layout[roomLocation[0], roomLocation[1]] == 0)
            {
                Layout[roomLocation[0], roomLocation[1]] = identifier + 1;
                if (sizeX <= 0 && sizeY <= 0)
                {
                    sizeX = rand.Next(3, 4);
                    sizeY = rand.Next(3, 5);
                }
                Room room = new Room(roomLocation, sizeX, sizeY);

                if (FillRoom(roomLocation[1], roomLocation[0], room.sizeX, room.sizeY, identifier))
                {
                    Rooms[i] = room; i++;
                }
            }
            return i;
        }

        public void GenerateHallways(int ammount = 1)
        {
            // Dictionary of connected rooms, key is the index and value is an array of the closest room indexes
            Dictionary<int, List<int>> connectedRooms = new Dictionary<int, List<int>>();
            for (int i = 0; i < Rooms.Length; i++)
            {
                connectedRooms[i] = new List<int>();
                Room room = Rooms[i];

                // Find the closest rooms
                for (int j = 0; j < Rooms.Length; j++)
                {
                    if (i == j) continue;

                    int distance = Math.Abs(Rooms[i].location[0] - Rooms[j].location[0]) + Math.Abs(Rooms[i].location[1] - Rooms[j].location[1]);

                    bool found = false;
                    for (int k = 0; k < connectedRooms[i].Count; k++)
                    {
                        if (distance < Math.Abs(Rooms[i].location[0] - Rooms[connectedRooms[i][k]].location[0]) + Math.Abs(Rooms[i].location[1] - Rooms[connectedRooms[i][k]].location[1]))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        connectedRooms[i].Add(j);
                        break;
                    }
                }
            }

            // Generate hallways
            foreach (var pair in connectedRooms)
            {
                Room room1 = Rooms[pair.Key];

                foreach (var connectedRoomIndex in pair.Value)
                {
                    Room room2 = Rooms[connectedRoomIndex];

                    // Lets connect the rooms now
                    GenerateHallway(room1.location, room2.location);
                }
            }
        }

        public void GenerateHallway(int[] firstLoc, int[] secondLoc)
        {
            if (Layout == null) throw new Exception("Layout is null");
            int[] corner = new int[2] { firstLoc[0], secondLoc[1] };
            ConnectStraight(firstLoc, corner);
            ConnectStraight(corner, secondLoc);
        }

        public void ConnectStraight(int[] firstLoc, int[] secondLoc)
        {
            if (Layout == null) throw new Exception("Layout is null");
            int startX = firstLoc[1];
            int startY = firstLoc[0];
            int endX = secondLoc[1];
            int endY = secondLoc[0];
            int identifierLast = 0;
            if (startX == endX)
            {
                // Vertical iteration
                int minY = Math.Min(startY, endY);
                int maxY = Math.Max(startY, endY);

                for (int y = minY; y <= maxY; y++)
                {
                    MarkMap(startX, y, 7);
                    setEntryPoint(startX, y, 'u', identifierLast);
                    identifierLast = Layout[y, startX];
                }
            }
            else if (startY == endY)
            {
                // Horizontal iteration
                int minX = Math.Min(startX, endX);
                int maxX = Math.Max(startX, endX);

                for (int x = minX; x <= maxX; x++)
                {
                    MarkMap(x, startY, 7);
                    setEntryPoint(x, startY, 'r', identifierLast);
                    identifierLast = Layout[startY, x];
                }
            }
        }

        private void setEntryPoint(int x, int y, char direction, int identifierLast)
        {
            if (Layout == null) throw new Exception("Layout is null");
            HashSet<int> identifierLastValues = new HashSet<int> { (int)RoomIdentifiers.ROOM, (int)RoomIdentifiers.GUARDIANROOM, (int)RoomIdentifiers.SPAWNROOM };
            switch (direction)
            {
                case 'u':
                    if (y <= 0) return;
                    if (identifierLastValues.Contains(identifierLast))
                    {
                        if (Layout[y, x] == (int)RoomIdentifiers.HALLWAY) MarkMap(x, y - 1, 8, null, true);
                    }
                    else if (identifierLast == 7)
                    {
                        if (identifierLastValues.Contains(Layout[y, x])) MarkMap(x, y, 8, null, true);
                    }

                    break;
                case 'r':
                    if (x <= 0) return;
                    if (identifierLastValues.Contains(identifierLast))
                    {
                        if (Layout[y, x] == (int)RoomIdentifiers.HALLWAY) MarkMap(x - 1, y, 8, null, true);
                    }
                    else if (identifierLast == 7)
                    {
                        if (identifierLastValues.Contains(Layout[y, x])) MarkMap(x, y, 8, null, true);
                    }
                    break;
            }
        }

        public void MarkMap(int x, int y, int identifier, int[]? overwriteSpecific = null, bool overwrite = false)
        {
            if (Layout == null) throw new Exception("Layout is null");
            if (overwriteSpecific != null && overwriteSpecific.Contains(Layout[y, x]))
            {
                Layout[y, x] = identifier;
            }
            else if (overwrite)
            {
                Layout[y, x] = identifier;
            }
            else if (Layout[y, x] == 0)
            {
                Layout[y, x] = identifier;
            }
        }


        public void PrintLayout()
        {
            if (Layout == null) throw new Exception("Layout is null");
            for (int i = 0; i < Layout.GetLength(0); i++)
            {
                for (int j = 0; j < Layout.GetLength(1); j++)
                {
                    switch (Layout[i, j])
                    {
                        case (int)RoomIdentifiers.WALL:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case (int)RoomIdentifiers.ROOM:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case (int)RoomIdentifiers.ROOMMIDDLE:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case (int)RoomIdentifiers.SPAWNROOM:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case (int)RoomIdentifiers.SPAWNMIDDLE:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case (int)RoomIdentifiers.GUARDIANROOM:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case (int)RoomIdentifiers.GUARDIANROOMMIDDLE:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case (int)RoomIdentifiers.HALLWAY:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case 8:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                    }
                    Console.Write(Layout[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");

        }

        public bool FillRoom(int x, int y, int sizeX, int sizeY, int identifier)
        {
            if (Layout == null) throw new Exception("Layout is null");
            int cols = Layout.GetLength(0);
            int rows = Layout.GetLength(1);
            int[,]? tempLayout = Layout.Clone() as int[,];
            if (tempLayout == null) throw new Exception("tempLayout is null");

            // Lets check if its not right next to another room
            for (int i = y - sizeY / 2 - 1; i < y + sizeY / 2 + 2; i++)
            {
                for (int j = x - sizeX / 2 - 1; j < x + sizeX / 2 + 2; j++)
                {
                    if (i < 0 || j < 0 || i >= cols || j >= rows) continue;
                    if (i == y && j == x) continue;
                    if (tempLayout[i, j] != 0)
                    {
                        Layout[y, x] = 0;
                        return false;
                    };
                }
            }

            for (int i = y - sizeY / 2; i < y + sizeY / 2 + 1; i++)
            {
                for (int j = x - sizeX / 2; j < x + sizeX / 2 + 1; j++)
                {
                    if (i < 0 || j < 0 || i >= cols || j >= rows) continue;
                    if (i == y && j == x) continue;
                    tempLayout[i, j] = identifier;
                }
            }

            Layout = tempLayout;
            return true;
        }

    }

    class Room
    {
        public int[] location;
        public int sizeX;
        public int sizeY;
        // public int[,] decorationLayout;
        // public int[,] layout;
        public Room(int[] location, int sizeX, int sizeY)
        {
            this.location = location;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }
    }
}



