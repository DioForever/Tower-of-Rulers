using System;
using System.Collections.Generic;
using System.Linq;

using FloorSystem;


namespace DungeonGeneration
{
    /// <summary>
    /// Represents a Dungeon floor, consisting of chunks, rooms, and hallways.
    /// </summary>
    public partial class DungeonFloor : Floor
    {
        public Chunk[,] DungeonMap;
        private static int[,] Layout;
        private Room[] Rooms;
        // private int[][] roomLocations;

        /// <summary>
        /// Initializes a new instance of the DungeonFloor class.
        /// </summary>
        /// <param name="floorNumber">Number of the floor.</param>
        /// <param name="SizeY">Size of the Layout in chunks on the y-axis.</param>
        /// <param name="SizeX">Size of the Layout in chunks on the x-axis.</param>
        public DungeonFloor(int floorNumber, int SizeY, int SizeX) : base(SizeX, SizeY, floorNumber)
        {
            Layout = new int[SizeY, SizeX];
            DungeonMap = new Chunk[0, 0];
            Rooms = new Room[0];
            // GenerateDungeon();
        }
        /// <summary>
        /// Identifiers of each chunk.
        /// </summary>
        public enum RoomIdentifiers
        {
            WALL = 0,
            SPAWNMIDDLE = 4,
            SPAWNROOM = 3,
            ROOMMIDDLE = 2,
            ROOM = 1,
            GUARDIANROOM = 5,
            GUARDIANROOMMIDDLE = 6,
            HALLWAY = 7,
            ENTRY = 8
        }
        /// <summary>
        /// Generates the entire Layout, including rooms and hallways.
        /// </summary>
        /// <param name="debug">True/False, decides if the layout is printed for debugging.</param>
        public void GenerateLayout(bool debug = false)
        {
            GenerateRooms();
            GenerateHallways();
            if (debug) PrintLayout();

        }
        /// <summary>
        /// Generates rooms in the dungeon Layout.
        /// </summary>
        /// <param name="roomMin">Intended minimal room count. Except Spawn and Guardian room.</param>
        /// <param name="roomMax">Intended maximal room count. Except Spawn and Guardian room.</param>
        private void GenerateRooms(int roomMin = 7, int roomMax = 10)
        {
            if (Layout == null) throw new Exception("Layout is null");
            Random rand = new();

            // Generating the middle points of the rooms
            int roomCount = rand.Next(roomMin, roomMax);
            Rooms = new Room[roomCount + 2];
            // Generate spawn room
            int[] spawnRoomLocation = new int[2];
            spawnRoomLocation[0] = rand.Next(1, floorMap.GetLength(0) - 1);
            spawnRoomLocation[1] = rand.Next(1, floorMap.GetLength(1) - 1);
            GenerateRoom(spawnRoomLocation, rand, 0, (int)RoomIdentifiers.SPAWNROOM, 3, 3);
            spawnY = spawnRoomLocation[0];
            spawnX = spawnRoomLocation[1];
            // Generate guardian room
            int[] guardianRoomLocation = new int[2];
            for (int i = 0; i < 1;)
            {
                int tries = 0;
                bool distanceNotReached = true;
                // We intend to reach the distance of half of the map, so the spawn and guardian isnt so close
                while (distanceNotReached)
                {
                    tries++;
                    guardianRoomLocation[0] = rand.Next(1, floorMap.GetLength(0) - 1);
                    guardianRoomLocation[1] = rand.Next(1, floorMap.GetLength(1) - 1);
                    double distance = Math.Sqrt(Math.Pow(guardianRoomLocation[0] - Rooms[0].location[0], 2) + Math.Pow(guardianRoomLocation[1] - Rooms[0].location[1], 2));
                    if (distance >= floorMap.GetLength(0) / 2 && distance <= Math.Sqrt(Math.Pow(guardianRoomLocation[0] + 12 - Rooms[0].location[0], 2) + Math.Pow(guardianRoomLocation[1] + 12 - Rooms[0].location[1], 2)))
                    {
                        distanceNotReached = false;
                    }
                    // If we cant find a location in 500 tries, we just place it somewhere
                    if (tries > 500)
                    {
                        distanceNotReached = false;
                        System.Console.WriteLine("Tries exceeded");
                    }
                }
                // We try to generate a room, if it fails, we try again
                i = GenerateRoom(guardianRoomLocation, rand, 1, (int)RoomIdentifiers.GUARDIANROOM, 4, 4);
            }
            exitY = guardianRoomLocation[0];
            exitX = guardianRoomLocation[1];


            int totalTries = 0;
            // Generate other rooms
            for (int i = 2; i < Rooms.Length;)
            {
                // We try to generate a room, if it fails, we try again, but after 500 tries we just stop
                totalTries += 1;
                if (totalTries > 500)
                {
                    System.Console.WriteLine("Tries exceeded");
                    break;
                }
                int[] roomLocation = new int[2];
                roomLocation[0] = rand.Next(1, floorMap.GetLength(0) - 1);
                roomLocation[1] = rand.Next(1, floorMap.GetLength(1) - 1);
                i = GenerateRoom(roomLocation, rand, i);
            }
            // Remove nulls from rooms
            Rooms = Rooms.Where(x => x != null).ToArray();

        }
        /// <summary>
        /// Generates room in Layout with specified location, size, identifier.
        /// </summary>
        /// <param name="roomLocation">Int array of (y,x) room location.</param>
        /// <param name="rand">Instance of random.</param>
        /// <param name="i">Number that tracks the room increase.</param>
        /// <param name="identifier">Int identifier of the room.</param>
        /// <param name="sizeX">Size of the room on x-axis.</param>
        /// <param name="sizeY">Size of the room on y-axis.</param>
        /// <returns>Returns i, i+1 if room was generated and i if not.</returns>
        private int GenerateRoom(int[] roomLocation, Random rand, int i, int identifier = (int)RoomIdentifiers.ROOM, int sizeX = -1, int sizeY = -1)
        {
            if (Layout == null) throw new Exception("Layout is null");
            // Check if we can place a room there (0 represents empty space)
            if (Layout[roomLocation[0], roomLocation[1]] == 0)
            {
                // We place the room in the Layout and set the identifier+1 (representing the middle of the room)
                Layout[roomLocation[0], roomLocation[1]] = identifier + 1;
                if (sizeX <= 0 && sizeY <= 0)
                {
                    sizeX = rand.Next(3, 4);
                    sizeY = rand.Next(3, 4);
                }
                // We create a new room object and add it to the array
                Room room = new Room(roomLocation, sizeY, sizeX);
                // We fill the room with the identifier
                if (FillRoom(roomLocation[1], roomLocation[0], room.sizeX, room.sizeY, identifier))
                {
                    Rooms[i] = room; i++;
                }
            }
            return i;
        }
        /// <summary>
        /// Generates hallways for the whole Layout.
        /// </summary>
        /// <param name="ammount">Amount of hallways coming out from each room.</param>
        private void GenerateHallways(int ammount = 1)
        {
            // Dictionary of connected rooms, key is the index and value is an array of the closest room indexes
            Dictionary<int, int> connectedRooms = new Dictionary<int, int>();
            // We keep track of the unconnected rooms and remove them from the list if they were connected
            List<int> unconnectedRooms = new List<int>();
            for(int i = 0; i < Rooms.Length; i++){
                unconnectedRooms.Add(i);
            }
            for (int i = 0; i < Rooms.Length; i++)
            {
                Room room = Rooms[i];
                Random rand = new();
                if(i == 1){
                    // We cant connect the spawn room to the guardian room, so we dont
                    int connectTo = rand.Next(2, unconnectedRooms.Count);
                    connectedRooms.Add(i, connectTo);
                    unconnectedRooms.Remove(connectTo);
                    continue;
                }
                // We connect the room to another random room and then remove it from unconnected list
                bool connected= false;
                while(!connected){
                    int connectTo = rand.Next(0, unconnectedRooms.Count);
                    if((i != connectTo)){
                        if(i == 0 && connectTo == 1) continue;
                        connectedRooms.Add(i, connectTo);
                        unconnectedRooms.Remove(connectTo);
                        connected = true;
                    }
                }
            }

            // Generate hallways
            foreach (var pair in connectedRooms)
            {
                Room room1 = Rooms[pair.Key];
                Room room2 = Rooms[pair.Value];
                // System.Console.WriteLine($"Room {string.Join(",", room1.location)} is connected to {string.Join(",", room2.location)}");
                GenerateHallway(room1.location, room2.location);
            }
        }
        /// <summary>
        /// Generates hallway between two positions.
        /// </summary>
        /// <param name="firstLoc">Int array of starting (y,x) location.</param>
        /// <param name="secondLoc">Int array of destination (y,x).</param>
        private void GenerateHallway(int[] firstLoc, int[] secondLoc)
        {
            if (Layout == null) throw new Exception("Layout is null");
            int[] corner = new int[2] { firstLoc[0], secondLoc[1] };
            List<int[]> visited = new List<int[]>(); // List of already visited locations
            ConnectStraight(firstLoc, corner, visited); // Connects firstLoc and corner
            ConnectStraight(corner, secondLoc, visited); // Connects corner and secondLoc
            visited = new List<int[]>();
        }
        /// <summary>
        /// Connects two locations on Layout and connects them in straight line, requires them to be on the same X or Y axis.
        /// </summary>
        /// <param name="firstLoc">Int array of starting (y,x) location.</param>
        /// <param name="secondLoc">Int array of destination (y,x).</param>
        /// <param name="visited">List of already visited locationss.</param>
        private void ConnectStraight(int[] firstLoc, int[] secondLoc, List<int[]> visited)
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
                    // We make sure that if there are HALLWAYS around, we dont spawn another one, to keep it clean
                    if (Utils.GetSides(Layout, startX, y, visited)[1] != (int)RoomIdentifiers.HALLWAY &&
                     Utils.GetSides(Layout, startX, y, visited)[3] != (int)RoomIdentifiers.HALLWAY)
                    {
                        Layout = Utils.MarkMap(Layout, startX, y, (int)RoomIdentifiers.HALLWAY);
                    }
                    // We set the entry point for the room, so we know where to not place walls
                    setEntryPoint(startX, y, true, identifierLast);
                    visited.Add(new int[] { y, startX });

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
                    // We make sure that if there are HALLWAYS around, we dont spawn another one, to keep it clean
                    if ((Utils.GetSides(Layout, x, startY, visited)[0] != (int)RoomIdentifiers.HALLWAY) && (Utils.GetSides(Layout, x, startY, visited)[2] != (int)RoomIdentifiers.HALLWAY))
                    {
                        Layout = Utils.MarkMap(Layout, x, startY, (int)RoomIdentifiers.HALLWAY);
                    }
                    // We set the entry point for the room, so we know where to not place walls
                    setEntryPoint(x, startY, false, identifierLast);
                    visited.Add(new int[] { startY, x });

                    identifierLast = Layout[startY, x];
                }
            }
        }
        /// <summary>
        /// Checks location while creating a hallway and if it finds a room at last or next position, it makes an entry point.
        /// </summary>
        /// <param name="x">Location on x-axis of current position.</param>
        /// <param name="y">Location on y-axis of current position.</param>
        /// <param name="direction">Direction in which hallway is constructed True = up / False = right</param>
        /// <param name="identifierLast">Last identifier.</param>
        private void setEntryPoint(int x, int y, bool direction, int identifierLast)
        {
            if (Layout == null) throw new Exception("Layout is null");
            HashSet<int> identifierLastValues = new HashSet<int> { (int)RoomIdentifiers.ROOM, (int)RoomIdentifiers.GUARDIANROOM, (int)RoomIdentifiers.SPAWNROOM };
            // According to direction, we check the positions differently
            switch (direction)
            {
                case true:
                    if (y <= 0) return;
                    if (identifierLastValues.Contains(identifierLast))
                    {
                        if (Layout[y, x] == (int)RoomIdentifiers.HALLWAY) Layout = Utils.MarkMap(Layout, x, y - 1, (int)RoomIdentifiers.ENTRY, null, true);
                    }
                    else if (identifierLast == 7)
                    {
                        if (identifierLastValues.Contains(Layout[y, x])) Layout = Utils.MarkMap(Layout, x, y, (int)RoomIdentifiers.ENTRY, null, true);
                    }

                    break;
                case false:
                    if (x <= 0) return;
                    if (identifierLastValues.Contains(identifierLast))
                    {
                        if (Layout[y, x] == (int)RoomIdentifiers.HALLWAY) Layout = Utils.MarkMap(Layout, x - 1, y, (int)RoomIdentifiers.ENTRY, null, true);
                    }
                    else if (identifierLast == 7)
                    {
                        if (identifierLastValues.Contains(Layout[y, x])) Layout = Utils.MarkMap(Layout, x, y, (int)RoomIdentifiers.ENTRY, null, true);
                    }
                    break;
            }
        }

        #if UNITY_EDITOR
        private void PrintLayout()
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
                        case (int)RoomIdentifiers.ENTRY:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case 9:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                    }
                    Console.Write(Layout[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");

        }
        /// <summary>
        /// Fills Room in Layout with certain identifier.
        /// </summary>
        /// <param name="x">Middle location of the room on x-axis.</param>
        /// <param name="y">Middle location of the room on y-axis.</param>
        /// <param name="sizeX">X siz of the room.</param>
        /// <param name="sizeY">Y size of the room.</param>
        /// <param name="identifier">Int identifier of RoomIdentifier.</param>
        /// <returns>True/False if room was constructed and doesn't collide with other room.</returns>
        #endif
        private bool FillRoom(int x, int y, int sizeX, int sizeY, int identifier)
        {
            if (Layout == null) throw new Exception("Layout is null");
            int cols = Layout.GetLength(0);
            int rows = Layout.GetLength(1);
            int[,] tempLayout = Layout.Clone() as int[,];
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
    /// <summary>
    /// Object used for storing properties, 
    /// </summary>
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