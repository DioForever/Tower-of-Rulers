using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
                    chunk = ChunkTilesSetter(chunk, chunkX, chunkY);

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
            else
            {
                for (int i = 0; i < chunk.map.GetLength(0); i++)
                {
                    for (int j = 0; j < chunk.map.GetLength(1); j++)
                    {
                        chunk.map[i, j] = (int)TilesGroupIdentifiers.FLOOR;
                    }
                }
            }
            HashSet<int> identifiersRooms = new HashSet<int> { (int)RoomIdentifiers.ROOM,(int)RoomIdentifiers.ROOMMIDDLE,
             (int)RoomIdentifiers.GUARDIANROOM, (int)RoomIdentifiers.GUARDIANROOMMIDDLE,(int)RoomIdentifiers.SPAWNROOM, (int)RoomIdentifiers.SPAWNMIDDLE };
            HashSet<int> identifiersIgnore = new HashSet<int> { (int)RoomIdentifiers.WALL, (int)RoomIdentifiers.HALLWAY, -1 };

            int[] wallSides = GetSides(Layout, x, y);
            if (identifiersRooms.Contains(Layout[y, x]))
            {
                // We draw the walls
                // RIGHT
                if (identifiersIgnore.Contains(wallSides[1]))
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.RIGHTWALL, chunk.map.GetLength(1) - 1);
                }
                // LEFT
                if (identifiersIgnore.Contains(wallSides[3]))
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.LEFTWALL);
                }
                // BOTTOM
                if (identifiersIgnore.Contains(wallSides[2]))
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.BOTTOMWALL, 0, chunk.map.GetLength(0) - 1);
                }
                // TOP
                if (identifiersIgnore.Contains(wallSides[0]))
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.TOPWALL);
                }

            }
            else if (Layout[y, x] == (int)RoomIdentifiers.ENTRY)
            {
                // We draw the walls

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

                // We draw the floor corners

                // RIGHT
                if (wallSides[1] == (int)RoomIdentifiers.HALLWAY)
                {
                    // chunk.map[0, 4] = (int)TilesGroupIdentifiers.TOPWALL;
                    // chunk.map[4, 4] = (int)TilesGroupIdentifiers.BOTTOMWALL;

                    chunk.map = MarkMap(chunk.map, 4, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 4, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // LEFT
                if (wallSides[3] == (int)RoomIdentifiers.HALLWAY)
                {
                    // chunk.map[0, 0] = (int)TilesGroupIdentifiers.TOPWALL;
                    // chunk.map[4, 0] = (int)TilesGroupIdentifiers.BOTTOMWALL;

                    chunk.map = MarkMap(chunk.map, 0, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 0, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // TOP 
                if (wallSides[0] == (int)RoomIdentifiers.HALLWAY)
                {
                    // chunk.map[0, 0] = (int)TilesGroupIdentifiers.TOPWALL;
                    // chunk.map[0, 4] = (int)TilesGroupIdentifiers.TOPWALL;

                    chunk.map = MarkMap(chunk.map, 0, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 4, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // BOTTOM
                if (wallSides[2] == (int)RoomIdentifiers.HALLWAY)
                {
                    // chunk.map[4, 0] = (int)TilesGroupIdentifiers.BOTTOMWALL;
                    // chunk.map[4, 4] = (int)TilesGroupIdentifiers.BOTTOMWALL;

                    chunk.map = MarkMap(chunk.map, 0, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 4, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }

            }
            else if (Layout[y, x] == (int)RoomIdentifiers.HALLWAY)
            {
                // We draw the floor corners

                // RIGHT
                if (wallSides[1] == (int)RoomIdentifiers.ENTRY || wallSides[1] == (int)RoomIdentifiers.HALLWAY)
                {
                    chunk.map = MarkMap(chunk.map, 4, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 4, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // LEFT
                if (wallSides[3] == (int)RoomIdentifiers.ENTRY || wallSides[3] == (int)RoomIdentifiers.HALLWAY)
                {
                    chunk.map = MarkMap(chunk.map, 0, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 0, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // TOP 
                if (wallSides[0] == (int)RoomIdentifiers.ENTRY || wallSides[0] == (int)RoomIdentifiers.HALLWAY)
                {

                    chunk.map = MarkMap(chunk.map, 0, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 4, 0, (int)TilesGroupIdentifiers.TOPWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // BOTTOM
                if (wallSides[2] == (int)RoomIdentifiers.ENTRY || wallSides[2] == (int)RoomIdentifiers.HALLWAY)
                {
                    chunk.map = MarkMap(chunk.map, 0, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                    chunk.map = MarkMap(chunk.map, 4, 4, (int)TilesGroupIdentifiers.BOTTOMWALL, new int[1] { (int)TilesGroupIdentifiers.FLOOR });
                }
                // We draw the walls
                if (wallSides[0] == (int)RoomIdentifiers.WALL || wallSides[0] == (int)RoomIdentifiers.ROOM ||
                wallSides[0] == (int)RoomIdentifiers.GUARDIANROOM || wallSides[0] == (int)RoomIdentifiers.GUARDIANROOMMIDDLE ||
                 wallSides[0] == (int)RoomIdentifiers.SPAWNROOM)
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.TOPWALL);
                }
                if (wallSides[1] == (int)RoomIdentifiers.WALL || wallSides[1] == (int)RoomIdentifiers.ROOM ||
                 wallSides[1] == (int)RoomIdentifiers.GUARDIANROOM || wallSides[1] == (int)RoomIdentifiers.GUARDIANROOMMIDDLE
                 || wallSides[1] == (int)RoomIdentifiers.SPAWNROOM)
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.RIGHTWALL, chunk.map.GetLength(1) - 1);
                }
                if (wallSides[2] == (int)RoomIdentifiers.WALL || wallSides[2] == (int)RoomIdentifiers.ROOM ||
                 wallSides[2] == (int)RoomIdentifiers.GUARDIANROOM || wallSides[2] == (int)RoomIdentifiers.GUARDIANROOMMIDDLE
                 || wallSides[2] == (int)RoomIdentifiers.SPAWNROOM)
                {
                    chunk.map = DrawHorizontalLine(chunk.map, (int)TilesGroupIdentifiers.BOTTOMWALL, 0, chunk.map.GetLength(0) - 1);
                }
                if (wallSides[3] == (int)RoomIdentifiers.WALL || wallSides[3] == (int)RoomIdentifiers.ROOM ||
                 wallSides[3] == (int)RoomIdentifiers.GUARDIANROOM || wallSides[3] == (int)RoomIdentifiers.GUARDIANROOMMIDDLE
                 || wallSides[3] == (int)RoomIdentifiers.SPAWNROOM)
                {
                    chunk.map = DrawVerticalLine(chunk.map, (int)TilesGroupIdentifiers.LEFTWALL);
                }


            }
            return chunk;
        }
        private Chunk ChunkTilesSetter(Chunk chunk, int x, int y)
        {
            Chunk tempChunk = new Chunk();
            tempChunk.map = new int[5, 5];
            for (int i = 0; i < chunk.map.GetLength(0); i++)
            {
                for (int j = 0; j < chunk.map.GetLength(1); j++)
                {
                    if (chunk.map[i, j] == 0) continue;
                    tempChunk.map[i, j] = GetWallIdentifier(chunk.map, j, i);
                }
            }

            return tempChunk;
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
        private enum TilesIdentifiers
        {
            WALL = 0,
            // CORNERS
            TOPCORNERLEFT = 1,
            TOPCORNERRIGHT = 2,
            BOTTOMCORNERLEFT = 3,
            BOTTOMCORNERRIGHT = 4,
            // TOP WALLS
            TOPWALLLEFT = 5,
            TOPWALLMIDDLE = 6,
            TOPWALLRIGHT = 7,
            // BOTTOM WALLS
            BOTTOMWALLLEFT = 8,
            BOTTOMWALLMIDDLE = 9,
            BOTTOMWALLRIGHT = 10,
            // LEFT WALLS
            LEFTWALLTOP = 11,
            LEFTWALLMIDDLE = 12,
            LEFTWALLBOTTOM = 13,
            // RIGHT WALLS
            RIGHTWALLTOP = 14,
            RIGHTWALLMIDDLE = 15,
            RIGHTWALLBOTTOM = 16,
            // TOP FLOOR 
            TOPFLOORLEFT = 17,
            TOPFLOORMIDDLE = 18,
            TOPFLOORRIGHT = 19,
            // MIDDLE FLOOR
            MIDDLEFLOORLEFT = 20,
            MIDDLEFLOORMIDDLE = 21,
            MIDDLEFLOORRIGHT = 22,
            // BOTTOM FLOOR
            BOTTOMFLOORLEFT = 23,
            BOTTOMFLOORMIDDLE = 24,
            BOTTOMFLOORRIGHT = 25,
        }
        private int GetWallIdentifier(int[,] map, int x, int y)
        {
            switch (map[y, x])
            {
                case (int)TilesGroupIdentifiers.TOPWALL:
                    return GetTOPWALIdentifier(GetSides(map, x, y));
                case (int)TilesGroupIdentifiers.RIGHTWALL:
                    return GetRIGHTWALLIdentifier();
                case (int)TilesGroupIdentifiers.BOTTOMWALL:
                    return GetBOTTOMWALLIdentifier(GetSides(map, x, y));
                case (int)TilesGroupIdentifiers.LEFTWALL:
                    return GetLEFTWALLIdentifier();
                case (int)TilesGroupIdentifiers.FLOOR:
                    return GetFLOORIdentifier(GetSides(map, x, y));

            }
            return 0;
        }

        private int GetTOPWALIdentifier(int[] sides)
        {
            // Its a LEFTTOP corner
            if (sides[1] == (int)TilesGroupIdentifiers.TOPWALL && sides[2] == (int)TilesGroupIdentifiers.LEFTWALL)
            {
                return (int)TilesIdentifiers.TOPCORNERLEFT;
            }
            // Its RIGHTTOP corner
            else if (sides[2] == (int)TilesGroupIdentifiers.RIGHTWALL && sides[3] == (int)TilesGroupIdentifiers.TOPWALL)
            {
                return (int)TilesIdentifiers.TOPCORNERRIGHT;
            }
            // These are the corners with the hallways
            else if (sides[0] == -1 && sides[1] == -1 && sides[3] != (int)TilesGroupIdentifiers.TOPWALL)
            {
                return (int)TilesIdentifiers.BOTTOMCORNERLEFT;
            }
            else if (sides[3] == -1 && sides[0] == -1 && sides[1] != (int)TilesGroupIdentifiers.TOPWALL)
            {
                return (int)TilesIdentifiers.BOTTOMCORNERRIGHT;
            }
            // Its a middle wall, so we randomly pick one of the top wall tiles
            else
            {
                Random rand = new Random();
                int random = rand.Next(0, 2);
                if (random == 0) return (int)TilesIdentifiers.TOPWALLLEFT;
                else if (random == 1) return (int)TilesIdentifiers.TOPWALLRIGHT;
                else return (int)TilesIdentifiers.TOPWALLMIDDLE;
            }
        }
        private int GetRIGHTWALLIdentifier()
        {
            Random rand = new Random();
            int random = rand.Next(0, 2);
            if (random == 0) return (int)TilesIdentifiers.RIGHTWALLTOP;
            else if (random == 1) return (int)TilesIdentifiers.RIGHTWALLBOTTOM;
            else return (int)TilesIdentifiers.RIGHTWALLMIDDLE;
        }
        private int GetBOTTOMWALLIdentifier(int[] sides)
        {
            // Its a LEFTTOP corner
            if (sides[1] == (int)TilesGroupIdentifiers.BOTTOMWALL && sides[0] == (int)TilesGroupIdentifiers.LEFTWALL)
            {
                return (int)TilesIdentifiers.BOTTOMCORNERLEFT;
            }
            // Its RIGHTTOP corner
            else if (sides[0] == (int)TilesGroupIdentifiers.RIGHTWALL && sides[3] == (int)TilesGroupIdentifiers.BOTTOMWALL)
            {
                return (int)TilesIdentifiers.BOTTOMCORNERRIGHT;
            }
            // These are the corners with the hallways
            else if (sides[2] == -1 && sides[1] == -1 && sides[3] != (int)TilesGroupIdentifiers.BOTTOMWALL)
            {
                return (int)TilesIdentifiers.TOPCORNERRIGHT;
            }
            else if (sides[2] == -1 && sides[3] == -1 && sides[1] != (int)TilesGroupIdentifiers.BOTTOMWALL)
            {
                return (int)TilesIdentifiers.TOPCORNERLEFT;
            }
            // Its a middle wall, so we randomly pick one of the top wall tiles
            else
            {
                Random rand = new Random();
                int random = rand.Next(0, 2);
                if (random == 0) return (int)TilesIdentifiers.BOTTOMFLOORLEFT;
                else if (random == 1) return (int)TilesIdentifiers.BOTTOMFLOORRIGHT;
                else return (int)TilesIdentifiers.BOTTOMFLOORMIDDLE;
            }
        }
        private int GetLEFTWALLIdentifier()
        {
            Random rand = new Random();
            int random = rand.Next(0, 2);
            if (random == 0) return (int)TilesIdentifiers.LEFTWALLTOP;
            else if (random == 1) return (int)TilesIdentifiers.LEFTWALLBOTTOM;
            else return (int)TilesIdentifiers.LEFTWALLMIDDLE;
        }
        private int GetFLOORIdentifier(int[] sides)
        {
            // We gonna first check if it doesnt need to be replaced with a corner
            if (sides[0] == (int)TilesGroupIdentifiers.RIGHTWALL && sides[1] == (int)TilesGroupIdentifiers.TOPWALL)
            {
                return (int)TilesIdentifiers.BOTTOMCORNERLEFT;
            }
            if (sides[1] == (int)TilesGroupIdentifiers.BOTTOMWALL && sides[2] == (int)TilesGroupIdentifiers.RIGHTWALL)
            {
                return (int)TilesIdentifiers.TOPCORNERLEFT;
            }
            if (sides[2] == (int)TilesGroupIdentifiers.LEFTWALL && sides[3] == (int)TilesGroupIdentifiers.BOTTOMWALL)
            {
                return (int)TilesIdentifiers.TOPCORNERRIGHT;
            }
            if (sides[3] == (int)TilesGroupIdentifiers.TOPWALL && sides[0] == (int)TilesGroupIdentifiers.LEFTWALL)
            {
                return (int)TilesIdentifiers.BOTTOMCORNERRIGHT;
            }

            // Its a LEFTTOP FLOOR corner
            if (sides[0] != (int)TilesGroupIdentifiers.FLOOR && sides[3] != (int)TilesGroupIdentifiers.FLOOR
            && sides[0] != -1 && sides[3] != -1)
            {
                return (int)TilesIdentifiers.TOPFLOORLEFT;
            } // Its a RIGHTTOP FLOOR corner
            else if (sides[0] != (int)TilesGroupIdentifiers.FLOOR && sides[1] != (int)TilesGroupIdentifiers.FLOOR
            && sides[0] != -1 && sides[1] != -1)
            {
                return (int)TilesIdentifiers.TOPFLOORRIGHT;
            }
            else if (sides[0] != (int)TilesGroupIdentifiers.FLOOR && sides[0] != -1) { return (int)TilesIdentifiers.TOPFLOORMIDDLE; }
            else if (sides[1] != (int)TilesGroupIdentifiers.FLOOR && sides[1] != -1) { return (int)TilesIdentifiers.MIDDLEFLOORRIGHT; }
            else if (sides[3] != (int)TilesGroupIdentifiers.FLOOR && sides[3] != -1) { return (int)TilesIdentifiers.MIDDLEFLOORLEFT; }
            return (int)TilesIdentifiers.MIDDLEFLOORMIDDLE;

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

            return wallSidesRooms;
        }

        public void PrintMap(Chunk[,]? mapLayout = null, bool mode = false)
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
                    if (mode == false) PrintChunkOnMap(mapMerged[y, x]);
                    else PrintChunkOnMapWithTiles(mapMerged[y, x]);
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
            if (identifier.ToString().Length == 1) Console.Write(identifier + "  ");
            else Console.Write(identifier + " ");
        }

        private static void PrintChunkOnMapWithTiles(int identifier)
        {
            switch (identifier)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 5:
                case 6:
                case 7:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 8:
                case 9:
                case 10:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case 11:
                case 12:
                case 13:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 14:
                case 15:
                case 16:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case 17:
                case 18:
                case 19:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 20:
                case 21:
                case 22:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case 23:
                case 24:
                case 25:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            if (identifier.ToString().Length == 1) Console.Write(identifier + "  ");
            else Console.Write(identifier + " ");
        }
    }

}
