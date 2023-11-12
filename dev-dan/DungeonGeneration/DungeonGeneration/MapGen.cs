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
            if (Layout == null)
            {
                throw new Exception("Layout is not generated, please generate layout first.");
            }


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


        private enum TilesIdentifiers
        {
            PUREWALL,
            FLOOR,
            LEFTWALL,
            TOPWALL,
            RIGHTWALL,
            BOTTOMWALL

        }


        private void PrintMap(Chunk[,] mapLayout)
        {
            // string[] mapResult = new string[(mapLayout.Length * 5 * 5)];
            int[,] mapMerged = new int[mapLayout.GetLength(0) * 5, mapLayout.GetLength(1) * 5];
            System.Console.WriteLine(mapMerged.Length);
            System.Console.WriteLine();
            // int index = 0;
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
                            // System.Console.WriteLine($"{i} - {j}: {y} {x}");
                            int indexY = i * 5 + y;
                            int indexX = j * 5 + x;
                            // System.Console.WriteLine($"{indexY} - {indexX}");
                            mapMerged[indexY, indexX] = chunk.map[y, x];
                            // System.Console.WriteLine(index);
                            // mapResult[index] = chunk.map[y, x].ToString();
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
            // System.Console.WriteLine();
            // string str = string.Join("", mapResult);
            // int chunkSize = mapLayout.Length;
            // System.Console.WriteLine(mapResult.Length);
            // int stringLength = str.Length;
            // System.Console.WriteLine($"{chunkSize} - {stringLength}");
            // for (int i = 0; i < stringLength; i += chunkSize)
            // {
            //     Console.WriteLine(str.Substring(i, chunkSize));
            // }

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