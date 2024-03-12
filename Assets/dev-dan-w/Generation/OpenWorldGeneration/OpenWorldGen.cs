using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;
using FloorSystem;


namespace WorldGeneration
{
    /// <summary>
    /// Represents a World floor, consisting of chunks - forests, plains and villages.
    /// </summary>
    public class WorldFloor : Floor
    {
        private static int[,] Layout;

        // private float magnification = 40.0f;

        private int groundTypes = 2;

        public WorldFloor(int floorNumber, int SizeY, int SizeX, Chunk[,] fMap = null) : base(SizeX, SizeY, floorNumber)
        {
            if (fMap != null) floorMap = fMap;
            else
            {
                spawnX = (int)SizeX / 2;
                spawnY = (int)SizeY / 2;

                playerX = spawnX;
                playerY = spawnY;
            }


        }

        public void GenerateMap()
        {

            // Generate offsets for the generations by the noise maps.
            (float[] offsetsX, float[] offsetsY) = GenOffsets(3);

            /*
                Chunk generate order:
                1. Buildings
                2. Ground
                3. Trees
                4. Grass
                5. Monster camps
            */

            int[] villageSpecs = GenerateVillage();

            for (int chunkX = 0; chunkX < floorMap.GetLength(1); chunkX++)
            {
                for (int chunkY = 0; chunkY < floorMap.GetLength(0); chunkY++)
                {
                    Chunk chunk = floorMap[chunkY, chunkX];
                    if (chunk == null) chunk = new Chunk();
                    floorMap[chunkY, chunkX] = chunk;

                    GenerateChunkGround(offsetsX[0], offsetsY[0], chunk, chunkX, chunkY);

                    if ((int)floorMap.GetLength(1) / 2 != chunkX && (int)floorMap.GetLength(0) / 2 != chunkY)
                    {
                        GenerateTrees(offsetsX[1], offsetsY[1], chunk, chunkX, chunkY, floorMap);
                    }

                    GenerateGrass(offsetsX[2], offsetsY[2], chunk, chunkX, chunkY);

                }
            }

            GenerateMonsterCamps(10, 25, villageSpecs);


            // Set teleport to the middle of the middle chunk
            floorMap[(int)floorMap.GetLength(0) / 2, (int)floorMap.GetLength(1) / 2].decorationMap[3, 3] = 3;
        }

        /// <summary>
        /// Generates Ground for the chunk.
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="chunk">Chunk that will be edited</param>
        /// <param name="chunkX">X location of the chunk in the map</param>
        /// <param name="chunkY">Y location of the chunk in the map</param>
        private void GenerateChunkGround(float offsetX, float offsetY, Chunk chunk, int chunkX, int chunkY)
        {
            // Set ground by ground groups
            for (int y = 0; y < chunk.map.GetLength(1); y++)
            {
                for (int x = 0; x < chunk.map.GetLength(0); x++)
                {
                    chunk.map[y, x] = GetIdUsingPerlinNM(x + chunkX * 5, offsetX, y + chunkY * 5, offsetY, groundTypes);
                }
            }
        }

        private int[] GenerateVillage()
        {
            int villageChunkWidth = Random.Range(6, 10);
            if (villageChunkWidth % 2 == 0) villageChunkWidth++;
            int villageChunkHeight = Random.Range(6, 10);
            if (villageChunkHeight % 2 == 0) villageChunkHeight++;

            // Generate the village layout
            int[,] layoutVillage = GenerateVillageLayout(villageChunkWidth, villageChunkHeight);

            bool wideHouse = false;

            // Generate x, y of the middle of the village, but at least 10 chunks away from the middle and width+1 and height+1 away from the edges
            int villageX = Random.Range(10, floorMap.GetLength(1) - 10);
            int villageY = Random.Range(10, floorMap.GetLength(0) - 10);
            // While villageX and villageY is not 10 chunks away from the middle, generate new ones
            int count = 0;
            while (GetDistance(spawnX, spawnY, villageX, villageY) < 10 && count <= 10000)
            {
                villageX = Random.Range(10, floorMap.GetLength(1) - 10);
                villageY = Random.Range(10, floorMap.GetLength(0) - 10);
                count++;

                if (count >= 10000) { Debug.LogError("Could not find a suitable village location"); }
            }

            // Lets go through the layout and place the village into the floorMap
            for (int y = 0; y < layoutVillage.GetLength(0); y++)
            {
                for (int x = 0; x < layoutVillage.GetLength(1); x++)
                {
                    // We skip the middle chunk
                    if (layoutVillage[y, x] == 0) continue;
                    Chunk chunk = new Chunk();

                    // if its a building we place it in the middle bottom
                    if (layoutVillage[y, x] != 0)
                    {
                        if (wideHouse) { wideHouse = false; continue; }
                        chunk.decorationMap[4, 2] = layoutVillage[y, x];
                        // We place the chunk into the floorMap
                        // Debug.Log($"{(int)(villageX + y - villageChunkHeight / 2)}, {(int)(villageY + x - villageChunkWidth / 2)} = {layoutVillage[y, x]}");
                        floorMap[(int)(villageX + y - villageChunkHeight / 2), (int)(villageY + x - villageChunkWidth / 2)] = chunk;
                        if (layoutVillage[y, x] == 10) wideHouse = true;
                    }
                }
            }

            return new int[] { villageX, villageY, villageChunkWidth, villageChunkHeight };
        }

        private int[,] GenerateVillageLayout(int width, int height)
        {
            int[,] layout = new int[height, width];
            // Make ?x (1-2 chunk wide buildings), 1x(2 chunks with stores)
            // Connect them with roads = empty chunks
            int shopTypes = 6;

            // Generate the village layout
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // if its vertical or horizontal middle, make it a ROAD/EMPTY
                    if (x == width / 2 || y == height / 2)
                    {
                        continue;
                    }

                    // if distance form middle is less than 2, make it a ROAD/EMPTY
                    if (GetDistance(width / 2, height / 2, x, y) < 2)
                    {
                        continue;
                    }

                    // if distance from middle is less than 3 and its above the teleport, make it a SHOP or a HOUSE

                    if (GetDistance(width / 2, height / 2, x, y) < 3 && y < height / 2)
                    {
                        // 90% chance to make it a shop if its under 3 shops
                        int buildingId = GenerateShops(shopTypes);
                        if (buildingId != -1) shopTypes++;
                        if (buildingId == -1) buildingId = GenerateHouses(x, y, width, true);

                        if (buildingId != -1) layout[y, x] = buildingId;
                    }

                    // if distance from middle is more than 3, make it a HOUSE
                    if (GetDistance(width / 2, height / 2, x, y) >= 3)
                    {
                        int buildingId = GenerateHouses(x, y, width);

                        if (buildingId != -1) layout[y, x] = buildingId;
                    }

                }

            }

            return layout;
        }

        private int GenerateShops(int shopTypes)
        {
            int buildingId = -1;
            if (shopTypes <= 8)
            {
                //90 % chance to make it a shop if its under 3 shops
                if (Random.Range(0, 10) != 0)
                {
                    // There are 3 types of shops
                    buildingId = shopTypes;
                }
            }
            // 40% chance to make it a shop if its over 3 shops
            else
            {
                if (Random.Range(0, 5) != 0)
                {
                    // Make the shop random one
                    buildingId = Random.Range(6, 9);
                }
            }

            return buildingId;
        }

        private int GenerateHouses(int x, int y, int width, bool smallOnly = false)
        {
            int buildingId = -1;
            // 80% chance to make it a house
            if (Random.Range(0, 5) != 0)
            {
                // 50% chance to make it a 1x2 house
                if (Random.Range(0, 2) == 1)
                {
                    buildingId = 9;
                }
                else if (!smallOnly)
                {
                    if (x + 1 < width) { buildingId = 10; }
                    else { buildingId = 9; }
                }
                else
                {
                    buildingId = 9;
                }
            }

            return buildingId;
        }

        /// <summary>
        /// Generates trees in a chunk.
        /// </summary>
        /// <param name="offsetX">Tree noise map x-offset</param>
        /// <param name="offsetY">Tree noise map y-offset</param>
        /// <param name="chunk">Chunk we wish to generate trees into</param>
        /// <param name="chunkX">X coordinate of the chunk</param>
        /// <param name="chunkY">Y coordinate of the chunk</param>
        private void GenerateTrees(float offsetX, float offsetY, Chunk chunk, int chunkX, int chunkY, Chunk[,] floorMap)
        {
            // We dont want to spawn tree into teleport
            if (chunkX == spawnX && chunkY == spawnY) return;
            int tree_count = 0;

            int cooldown = 8;

            // Trees have id of XY, X represents the base and Y represents the leafs, X is 1-5, Y is 0-4
            for (int y = 1; y < chunk.map.GetLength(1) - 2; y++)
            {
                for (int x = 1; x < chunk.map.GetLength(0) - 2; x++)
                {
                    // Lets get if there even are trees (chance 66%, 0-2)
                    int forestChance = GetIdUsingPerlinNM(x + chunkX * 5, offsetX, y + chunkY * 5, offsetY, 3);
                    if (forestChance == 2) continue;

                    // If we already spawned 2 trees in the chunk, return
                    if (tree_count == 2) return;
                    if (tree_count == 1) cooldown--;
                    if (cooldown == 0) continue;

                    int chance = Random.Range(0, 3);
                    if (chance == 2)
                    {

                        // Spawn a tree
                        int groundType = chunk.map[y, x];
                        int randomBase = Random.Range(1, 6);
                        int randomLeafs = 0;
                        if (groundType == 0)
                        {
                            randomLeafs = Random.Range(1, 3);
                        }
                        else
                        {
                            randomLeafs = Random.Range(3, 5);
                        }

                        tree_count++;
                        if (randomBase == 5) randomLeafs = 0;

                        // Check if we can spawn the tree
                        bool overlaping = false;
                        int[,] currTree = GetTreeTilesPositions(x, y, randomBase, randomLeafs);

                        // Check the chunks around as well
                        for (int chunkY1 = chunkY - 1; chunkY1 < chunkY + 1; chunkY1++)
                        {
                            // Check boundaries
                            if (chunkY1 < 0 || chunkY1 >= floorMap.GetLength(0)) continue;

                            for (int chunkX1 = chunkX - 1; chunkX1 < chunkX + 1; chunkX1++)
                            {
                                // Check boundaries
                                if (chunkX1 < 0 || chunkX1 >= floorMap.GetLength(1)) continue;
                                Chunk currChunk = floorMap[chunkY1, chunkX1];
                                if (currChunk == null) continue;

                                // Check if we found a tree
                                // Now we go through the chunk
                                for (int y1 = 0; y1 < currChunk.decorationMap.GetLength(0); y1++)
                                {
                                    if (overlaping) break;
                                    for (int x1 = 0; x1 < currChunk.decorationMap.GetLength(1); x1++)
                                    {
                                        // Skip if there is something other than a tree
                                        if (overlaping) break;
                                        if (currChunk.decorationMap[y1, x1] == 0) continue;
                                        if (x1 == x && y1 == y) continue;

                                        // Its a tree
                                        if (currChunk.decorationMap[y1, x1] >= 10 && currChunk.decorationMap[y1, x1] <= 54)
                                        {
                                            string treeIds = currChunk.decorationMap[y1, x1].ToString();
                                            int baseId = int.Parse(treeIds[0].ToString());
                                            int leafId = int.Parse(treeIds[1].ToString());
                                            int[,] foundTree = GetTreeTilesPositions(x1, y1, baseId, leafId);
                                            if (!CompareTrees(currTree, foundTree)) overlaping = true;
                                        }
                                    }
                                }

                            }
                        }
                        if (!overlaping) { chunk.decorationMap[y, x] = int.Parse($"{randomBase}{randomLeafs}"); }
                    }
                }
            }
        }

        private void GenerateMonsterCamps(int minCamps, int maxCamps, int[] vilageSpecs)
        {
            // Generate monster camps
            int camps = Random.Range(minCamps, maxCamps);

            // Generate random position of the camp, and check if its at least 10 chunks away from the village
            for (int i = 0; i < camps; i++)
            {
                int x = Random.Range(0, floorMap.GetLength(1));
                int y = Random.Range(0, floorMap.GetLength(0));

                // if (GetDistance(floorMap.GetLength(1) / 2, floorMap.GetLength(0) / 2, x, y) < 10) { i--; continue; }
                // if its in the village or 10 chunks away from the village, skip
                if (GetDistance(vilageSpecs[0], vilageSpecs[1], x, y) < 10) { i--; continue; }

                // Generate the camp
                Chunk chunk = floorMap[y, x];
                if (chunk == null) chunk = new Chunk();
                floorMap[y, x] = chunk;

                // Generate the camp
                GenerateCamp(chunk);
            }

        }

        private void GenerateCamp(Chunk chunk)
        {
            // Generate the camp
            chunk.decorationMap[2, 2] = 15;
        }
        private void GenerateGrass(float offsetX, float offsetY, Chunk chunk, int chunkX, int chunkY)
        {
            // We dont want to spawn grass into teleport
            if (chunkX == spawnX && chunkY == spawnY) return;

            for (int y = 0; y < chunk.map.GetLength(1); y++)
            {
                for (int x = 0; x < chunk.map.GetLength(0); x++)
                {
                    // Check if its not already taken
                    if (chunk.decorationMap[y, x] != 0) continue;


                    // Debug.Log($"GRASS");
                    int grassChance = GetIdUsingPerlinNM(x + chunkX * 5, offsetX, y + chunkY * 5, offsetY, 5, 20);
                    // Debug.Log($"CHANCE {grassChance}");

                    int chance = Random.Range(0, 5);
                    int chanceRock = Random.Range(0, 21); // 5% to spawn a rock

                    if (chanceRock == 20)
                    {
                        // Spawn a rock
                        continue;
                    }

                    // 20% chance to spawn (0-4)
                    if (chance == 1)
                    {
                        switch (grassChance)
                        {
                            case 0:
                                // Red flowers
                                chunk.decorationMap[y, x] = 1;
                                break;
                            case 1:
                                // Purple flowers
                                chunk.decorationMap[y, x] = 2;
                                break;
                            case 2:
                            case 3:
                                // Grass
                                int groundType = chunk.map[y, x];
                                switch (groundType)
                                {
                                    case 0:
                                        // Light Grass
                                        chunk.decorationMap[y, x] = 4;
                                        break;
                                    case 1:
                                        // Dark Grass
                                        chunk.decorationMap[y, x] = 5;
                                        break;
                                }
                                break;
                            case 4:
                                // Yellow flowers
                                chunk.decorationMap[y, x] = 3;
                                break;
                        }
                    }
                }
            }

            return;
        }
        // Generate TilesetCount-times a 2D map of noise, for ground, ground features, forests/rocks
        /// <summary>
        /// Generates groundTypes-times offset for perlin noise maps.
        /// </summary>
        /// <returns>Tuple of x and y offset float arrays.</returns>
        private (float[] offsetsX, float[] offsetsY) GenOffsets(int offsetAmount)
        {
            // We will keep track of the offset in two arrays, X and Y
            float[] NMOffsetsX = new float[offsetAmount];
            float[] NMOffsetsY = new float[offsetAmount];

            // Seed the random number generator once
            Random.InitState((int)System.DateTime.Now.Ticks);

            // We initialize the offsets
            for (int i = 0; i < offsetAmount; i++)
            {
                NMOffsetsX[i] = Random.Range(0f, 999999f);
                NMOffsetsY[i] = Random.Range(0f, 999999f);
            }

            return (NMOffsetsX, NMOffsetsY);
        }

        /// <summary>
        /// Gets group ID for specific location with offset, returns an ID ranging from 0 to typeAmmount.
        /// </summary>
        /// <param name="x">X location on map</param>
        /// <param name="x_offset"></param>
        /// <param name="y">Y location on map</param>
        /// <param name="y_offset"></param>
        /// <param name="typeAmmount">Range of output, [0; typeAmmount]</param>
        /// <returns></returns>
        private int GetIdUsingPerlinNM(int x, float x_offset, int y, float y_offset, int typeAmmount, int magnification_ = 40)
        {

            float raw_perlin = Mathf.PerlinNoise(
                (x + x_offset) / magnification_,
                (y + y_offset) / magnification_
            );

            float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
            float scaled_perlin = clamp_perlin * typeAmmount; // to make it into only these choices

            if (scaled_perlin > typeAmmount) scaled_perlin = typeAmmount;

            return Mathf.FloorToInt(scaled_perlin);
        }

        /// <summary>
        /// Gets array of all y,x coordinates where the tree will be rendered at.
        /// </summary>
        /// <param name="x_base">X coordinate of the tree</param>
        /// <param name="y_base">Y coordinate of the tree</param>
        /// <param name="baseType">Index of the tree base type</param>
        /// <param name="leafsType">Index of the tree leaf type</param>
        /// <returns></returns>
        public static int[,] GetTreeTilesPositions(int x_base, int y_base, int baseType, int leafsType)
        {
            List<int[]> found = new List<int[]>();

            int y_offset = 0;
            int tile_offset = 0;
            bool noLeafs = false;

            int leaf_width = 6;
            int leaf_height = 0;
            int leaf_x_offset = 0;
            int leaf_y_offset = 0;

            // if (baseType != 4) return;
            // 6, 14, 22, 30
            // Setup leafs y_offset
            switch (baseType)
            {
                case 1:
                    y_offset = 2;
                    leaf_y_offset = 3;
                    break;
                case 4:
                    y_offset = 3;
                    leaf_y_offset = 4;
                    tile_offset = (baseType - 1) * 8 - 2;
                    break;
                case 2:
                case 3:
                    y_offset = 3;
                    leaf_y_offset = 3;
                    tile_offset = (baseType - 1) * 8 - 2;
                    break;
                case 5:
                    y_offset = 2;
                    leaf_y_offset = 4;
                    tile_offset = (baseType - 1) * 8 - 2;
                    noLeafs = true;
                    break;
            }

            for (int y = 0; y <= y_offset; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    int totalY = y_base + y_offset - y;
                    int totalX = x_base + y_offset + x;

                    int[] loc = new int[2] { totalY, totalX };
                    found.Add(loc);
                }
            }
            switch (leafsType)
            {
                case 0:
                    noLeafs = true;
                    break;
                case 1:
                    leaf_height = 4;

                    leaf_x_offset = 0;
                    break;
                case 2:
                    leaf_height = 3;

                    leaf_x_offset = 1;
                    break;
                case 3:
                    leaf_height = 4;

                    leaf_x_offset = 1;
                    break;
                case 4:
                    leaf_height = 4;

                    leaf_x_offset = 1;
                    break;
            }
            if (!noLeafs)
            {
                for (int y = 0; y < leaf_height; y++)
                {
                    for (int x = 0; x < leaf_width; x++)
                    {
                        int totalY = y_base - y + leaf_y_offset + 1;
                        int totalX = x_base + y_offset + x + leaf_x_offset - 2;

                        int[] loc = new int[2] { totalY, totalX };
                        found.Add(loc);
                    }
                }
            }
            int[,] foundArr = new int[found.Count, 2];

            for (int i = 0; i < found.Count; i++)
            {
                foundArr[i, 0] = found[i][0];
                foundArr[i, 1] = found[i][1];
            }

            return foundArr;
        }

        /// <summary>
        /// Compares two tree arrays to check whenever they overlap.
        /// </summary>
        /// <param name="tree0">Array of all y,x coordinates where the tree will be rendered at.</param>
        /// <param name="tree1">Array of all y,x coordinates where the tree will be rendered at.</param>
        /// <returns></returns>
        public static bool CompareTrees(int[,] tree0, int[,] tree1)
        {


            // Check each node of tree0 against each node of tree1
            for (int i = 0; i < tree0.GetLength(0); i++)
            {
                for (int j = 0; j < tree1.GetLength(0); j++)
                {
                    if (tree0[i, 0] == tree1[j, 0] && tree0[i, 1] == tree1[j, 1])
                    {
                        // If a matching node is found, return false
                        return false;
                    }
                }
            }

            // If no matching node is found, return true
            return true;
        }

        private float GetDistance(int x1, int y1, int x2, int y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
        }
    }
}