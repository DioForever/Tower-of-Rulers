using UnityEngine;

using FloorSystem;


namespace WorldGeneration
{
    /// <summary>
    /// Represents a World floor, consisting of chunks, forests, plains and villages.
    /// </summary>
    public class WorldFloor : Floor
    {
        public Chunk[,] WorldMap;
        private static int[,] Layout;

        public WorldFloor(int SizeX, int SizeY, int floorNumber, Chunk[,] floorMap = null) : base(SizeX, SizeY, floorNumber, floorMap)
        {


        }

        private void GenerateMap(int mapWidth, int mapHeight){

            (float[] offsetsX, float[] offsetsY) = GenOffsets();

            for(int chunkX = 0; chunkX < mapWidth; chunkX++){
                for(int chunkY = 0; chunkY < mapHeight; chunkY++){
                    Chunk chunk = new Chunk();

                }
            }
        }

        private void GenerateChunkGround(float offsetX, float offsetY, Chunk chunk){
            // for(int y = 0; y < chunk.map.GetLength(1); y++){
            //     for(int x = 0; x < chunk.map.GetLength(0); x++){
                    
            //     }
            // }
        }

        // Generate 3x a 2D map of noise, for ground, ground features, forests/rocks
        private (float[] offsetsX, float[] offsetsY) GenOffsets(){
            int offsetAmmount = 3;

            // We will keep track of the offset in two arrays, X and Y
            float[] NMOffsetsX = new float[offsetAmmount];
            float[] NMOffsetsY = new float[offsetAmmount];

            // We initialize the offsets
            for(int i = 0; i < offsetAmmount; i++){
                NMOffsetsX[i] = Random.Range(0f, 999999f);
                NMOffsetsY[i] = Random.Range(0f, 999999f);
            }

            return (NMOffsetsX, NMOffsetsY);
        }

        private int GetIdUsingPerlinNM(int x,int x_offset, int y, int y_offset, float magnification,int tilesetCount){

            float raw_perlin = Mathf.PerlinNoise(
                (x) / magnification, 
                (y) / magnification
            );

            float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
            float scaled_perlin = clamp_perlin * tilesetCount; // to make it into only these choices

            if(scaled_perlin == 4) scaled_perlin = 3;

            return Mathf.FloorToInt(scaled_perlin);
        }


    }
}