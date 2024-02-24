using UnityEngine;

using FloorSystem;


namespace WorldGeneration
{
    /// <summary>
    /// Represents a World floor, consisting of chunks, forests, plains and villages.
    /// </summary>
    public class WorldFloor : Floor
    {
        private static int[,] Layout;

        private float magnification = 40.0f;

        private int groundTypes = 2;

        public WorldFloor( int floorNumber, int SizeY, int SizeX, Chunk[,] fMap = null) : base(SizeX, SizeY, floorNumber)
        {
            if(fMap != null) floorMap = fMap;
            spawnX = (int)SizeX/2;
            spawnY = (int)SizeY/2;


        }

        public void GenerateMap(){

            (float[] offsetsX, float[] offsetsY) = GenOffsets();

            for(int chunkX = 0; chunkX < floorMap.GetLength(1); chunkX++){
                for(int chunkY = 0; chunkY < floorMap.GetLength(0); chunkY++){
                    Chunk chunk = new Chunk();

                    GenerateChunkGround(offsetsX[0], offsetsY[0], chunk, chunkX, chunkY);
                    
                    floorMap[chunkY,chunkX] = chunk;
                }
            }


            // Set teleport to the middle of the middle chunk
            floorMap[(int)floorMap.GetLength(0)/2, (int)floorMap.GetLength(1)/2].decorationMap[3,3] = 3;
        }

        /// <summary>
        /// Generates Ground for the chunk.
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="chunk">Chunk that will be edited</param>
        /// <param name="chunkX">X location of the chunk in the map</param>
        /// <param name="chunkY">Y location of the chunk in the map</param>
        private void GenerateChunkGround(float offsetX, float offsetY, Chunk chunk, int chunkX, int chunkY){
            // Set ground by ground groups
            for(int y = 0; y < chunk.map.GetLength(1); y++){
                for(int x = 0; x < chunk.map.GetLength(0); x++){
                    chunk.map[y,x] = GetIdUsingPerlinNM(x+chunkX*5, offsetX, y+chunkY*5, offsetY, groundTypes);
                    int groundTypeId = GetIdUsingPerlinNM(x+chunkX*5, offsetX, y+chunkY*5, offsetY, groundTypes);
                    int[] neighbours = GetNeighboursGroupIDs(x+chunkX*5, offsetX, y+chunkY*5, offsetY, groundTypes);
                    neighbours = GetGroundIDs(neighbours, groundTypeId);
                }
            }
        }

        // Generate TilesetCount-times a 2D map of noise, for ground, ground features, forests/rocks
        /// <summary>
        /// Generates groundTypes-times offset for perlin noise maps.
        /// </summary>
        /// <returns>Tuple of x and y offset float arrays.</returns>
        private (float[] offsetsX, float[] offsetsY) GenOffsets(){
            int offsetAmount = groundTypes;

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
        private int GetIdUsingPerlinNM(int x, float x_offset, int y, float y_offset, int typeAmmount){

            float raw_perlin = Mathf.PerlinNoise(
                (x+x_offset) / magnification, 
                (y+y_offset) / magnification
            );

            float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
            float scaled_perlin = clamp_perlin * typeAmmount; // to make it into only these choices

            // if(scaled_perlin == 4) scaled_perlin = 3;

            return Mathf.FloorToInt(scaled_perlin);
        }

        /// <summary>
        /// Gets group IDs for horizontal and vertical directions (TOP, RIGHT, BOTTOM, LEFT) in this specific order.
        /// </summary>
        /// <param name="x">X location on map</param>
        /// <param name="x_offset"></param>
        /// <param name="y">Y location on map</param>
        /// <param name="y_offset"></param>
        /// <returns>Array of 4 ints, each representing group ID of (TOP, RIGHT, BOTTOM, LEFT) neighbour</returns>
        private int[] GetNeighboursGroupIDs(int x, float x_offset, int y, float y_offset, int groundTypes){
            
            int[] neighbours = new int[] {0,0,0,0};

            neighbours[0] = GetIdUsingPerlinNM(x, x_offset, y + 1, y_offset, groundTypes);
            neighbours[1] = GetIdUsingPerlinNM(x + 1, x_offset, y + 1, y_offset, groundTypes);
            neighbours[2] = GetIdUsingPerlinNM(x, x_offset, y - 1, y_offset, groundTypes);
            neighbours[3] = GetIdUsingPerlinNM(x - 1, x_offset, y, y_offset, groundTypes);
            
            return neighbours;
        }
    
        private int[] GetGroundIDs(int[] neighbours, int groundTypeId){
            for(int i = 0; i < neighbours.Length; i++){
                // if()
            }
            return new int[] {0,0,0,0,0};
        }
    }
}