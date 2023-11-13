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
        int chunkSize = 5;
        public void GenerateMap()
        {
            if (Layout == null) throw new Exception("Layout is null");


            for (int chunkY = 0; chunkY < Layout.GetLength(0); chunkY++)
            {
                for (int chunkX = 0; chunkX < Layout.GetLength(1); chunkX++)
                {
                    Chunk chunk = GenerateChunk(chunkX, chunkY);
                    // Chunk chunk = new Chunk();
                    // chunk.map = new int[5, 5];
                    // System.Console.WriteLine($"Chunk: {chunkX} {chunkY}");
                    floorMap[chunkY, chunkX] = chunk;
                }
            }




        }

        private Chunk GenerateChunk(int x, int y)
        {
            if (Layout == null) throw new Exception("Layout is null");

            Chunk chunk = new Chunk();
            chunk.map = new int[5, 5] {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0}
            };
            // If its empty chunk, we dont need to continue
            if (Layout[y, x] == 0) return chunk;
            // First we make everything ground

            // First we there are walls or not
            int[] wallSides = GetSides(Layout, x, y);

            HashSet<int> identifiersRooms = new HashSet<int> { (int)RoomIdentifiers.ROOM,(int)RoomIdentifiers.ROOMMIDDLE,
             (int)RoomIdentifiers.GUARDIANROOM, (int)RoomIdentifiers.GUARDIANROOMMIDDLE,(int)RoomIdentifiers.SPAWNROOM, (int)RoomIdentifiers.SPAWNMIDDLE };
            HashSet<int> identifiersIgnore = new HashSet<int> { (int)RoomIdentifiers.WALL, (int)RoomIdentifiers.HALLWAY, -1 };

            if (identifiersRooms.Contains(Layout[y, x]))
            {
                // We draw the walls
                // TOP
                if (identifiersIgnore.Contains(wallSides[0]))
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.TOPWALL);
                }
                // RIGHT
                if (identifiersIgnore.Contains(wallSides[1]))
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.RIGHTWALL, chunk.map.GetLength(1) - 1);
                }
                // BOTTOM
                if (identifiersIgnore.Contains(wallSides[2]))
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.BOTTOMWALL, 0, chunk.map.GetLength(0) - 1);
                }
                // LEFT
                if (identifiersIgnore.Contains(wallSides[3]))
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.LEFTWALL);
                }
                // Now we draw the floor
                // chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.FLOOR, 1, 1);

            }
            else if (Layout[y, x] == (int)RoomIdentifiers.ENTRY)
            {
                if (wallSides[0] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.TOPWALL);
                }
                if (wallSides[1] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.RIGHTWALL, chunk.map.GetLength(1) - 1);
                }
                if (wallSides[2] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.BOTTOMWALL, 0, chunk.map.GetLength(0) - 1);
                }

            }
            else if (Layout[y, x] == (int)RoomIdentifiers.HALLWAY)
            {
                if (wallSides[0] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.TOPWALL);
                }
                if (wallSides[1] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.RIGHTWALL, chunk.map.GetLength(1) - 1);
                }
                if (wallSides[2] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.BOTTOMWALL, 0, chunk.map.GetLength(0) - 1);
                }
                if (wallSides[3] == (int)RoomIdentifiers.WALL)
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.LEFTWALL);
                }


            }
            return chunk;
        }


        private int[,] DrawVerticalLine(int[,] map, int groupIdentifier, int xOffset = 0, int yOffset = 0)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                map[y + yOffset, xOffset] = groupIdentifier;
                // System.Console.WriteLine($"{y + yOffset} {xOffset}");
                // map[y + yOffset, xOffset] = getIdentifierByGroup(groupIdentifier, map, xOffset, y + yOffset);
            }
            return map;
        }
        private int[,] DrawHorizontalLine(int[,] map, int groupIdentifier, int xOffset = 0, int yOffset = 0)
        {
            for (int x = 0; x < map.GetLength(1) - xOffset; x++)
            {
                // System.Console.WriteLine($"{yOffset} {x + xOffset}");
                map[yOffset, x + xOffset] = groupIdentifier;
                // map[yOffset, x + xOffset] = getIdentifierByGroup(groupIdentifier, map, x + xOffset, yOffset);
            }
            return map;
        }
        private enum TilesGroupIdentifiers
        {
            TOPWALL = 1,
            RIGHTWALL,
            BOTTOMWALL,
            LEFTWALL,
            FLOOR
        }

        public static bool ArrayEquals(int[] a, int[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            return !a.Where((t, i) => t != b[i]).Any();
        }

        public static bool HaveVisited(int[] array, List<int[]> visitedList)
        {
            return visitedList.Any(visitedArray => ArrayEquals(array, visitedArray));
        }

        public int[] GetSides(int[,] map, int x, int y, List<int[]>? locationsIgnore = null)
        {
            if (map == null) throw new Exception("Given map is null");

            // We create an array of 4 ints, each int represents a side of the wall {top, right, bottom, left}
            int[] wallSidesRooms = new int[4] { 0, 0, 0, 0 };
            // First, lets check if we can even check for walls
            if (locationsIgnore != null)
            {
                // System.Console.WriteLine("CHECKIN HALL");
                // System.Console.WriteLine(String.Join(" || ", locationsIgnore.Select(x => String.Join(",", x))));
                // System.Console.WriteLine($"CHECKIN HALL {string.Join(",", new int[2] { y, x - 1 })} {HaveVisited(new int[2] { y, x - 1 }, locationsIgnore)} 0");
                // System.Console.WriteLine($"CHECKIN HALL {string.Join(",", new int[2] { y, x + 1 })} {HaveVisited(new int[2] { y, x + 1 }, locationsIgnore)} 1");
                // System.Console.WriteLine($"CHECKIN HALL {string.Join(",", new int[2] { y - 1, x })} {HaveVisited(new int[2] { y - 1, x }, locationsIgnore)} 2");
                // System.Console.WriteLine($"CHECKIN HALL {string.Join(",", new int[2] { y + 1, x })} {HaveVisited(new int[2] { y - 1, x }, locationsIgnore)} 3");

                if (HaveVisited(new int[2] { y, x - 1 }, locationsIgnore))
                {
                    wallSidesRooms[3] = -1;
                }
                if (HaveVisited(new int[2] { y, x + 1 }, locationsIgnore))
                {
                    wallSidesRooms[1] = -1;
                }
                if (HaveVisited(new int[2] { y - 1, x }, locationsIgnore))
                {
                    wallSidesRooms[0] = -1;
                }
                if (HaveVisited(new int[2] { y - 1, x }, locationsIgnore))
                {
                    wallSidesRooms[2] = -1; ;
                }
            }
            if (x - 1 < 0)
            {
                wallSidesRooms[3] = -1;
            }
            if (x + 1 > map.GetLength(0) - 1)
            {
                wallSidesRooms[1] = -1;
            }
            if (y - 1 < 0)
            {
                wallSidesRooms[0] = -1;
            }
            if (y + 1 > map.GetLength(1) - 1)
            {
                wallSidesRooms[2] = -1;
            }
            int top = y - 1;
            int right = x + 1;
            int bottom = y + 1;
            int left = x - 1;
            // Now we save the identifiers of the rooms
            if (wallSidesRooms[0] != -1) { wallSidesRooms[0] = map[top, x]; }
            if (wallSidesRooms[1] != -1) { wallSidesRooms[1] = map[y, right]; }
            if (wallSidesRooms[2] != -1) { wallSidesRooms[2] = map[bottom, x]; }
            if (wallSidesRooms[3] != -1) { wallSidesRooms[3] = map[y, left]; }

            // System.Console.WriteLine($"{y}:{x} -WALLSIDES: {string.Join(",", wallSidesRooms)}");
            return wallSidesRooms;
        }

        public void PrintMap(Chunk[,]? mapLayout = null)
        {
            if (mapLayout == null) mapLayout = floorMap;
            int[,] mapMerged = new int[mapLayout.GetLength(0) * chunkSize, mapLayout.GetLength(1) * chunkSize];
            for (int i = 0; i < mapLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.GetLength(1); j++)
                {
                    // Now we go one deeper, inside the chunks
                    Chunk chunk = mapLayout[i, j];
                    for (int y = 0; y < chunk.map.GetLength(0); y++)
                    {
                        for (int x = 0; x < chunk.map.GetLength(1); x++)
                        {
                            int indexY = i * chunkSize + y;
                            int indexX = j * chunkSize + x;
                            mapMerged[indexY, indexX] = chunk.map[y, x];
                        }
                    }
                }
            }

            for (int y = 0; y < mapMerged.GetLength(0); y++)
            {
                for (int x = 0; x < mapMerged.GetLength(1); x++)
                {
                    PrintChunkOnMap(mapMerged[y, x]);
                }
                System.Console.WriteLine();
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
            Console.Write(identifier + " ");
        }

    }
}