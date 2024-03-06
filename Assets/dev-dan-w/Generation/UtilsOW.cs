using UnityEngine;
using UnityEngine.Tilemaps;

using System.Collections.Generic;

public class UtilsOW
{
    public static void LoadTree(Tilemap baseMap, Tilemap leafMap, Tile[] baseSet, Tile[] leafSet, int x_base, int y_base, int baseType, int leafsType)
    {
        int y_offset = 0;
        int tile_offset = 0;
        bool noLeafs = false;

        int leaf_width = 6;
        int leaf_height = 0;
        int leaf_x_offset = 0;
        int leaf_y_offset = 0;
        int leaf_offset = 0;

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

                Vector3Int position = new Vector3Int(totalX, totalY, 0);
                if ((y * 2 + x + tile_offset) >= baseSet.Length) continue;
                baseMap.SetTile(position, baseSet[y * 2 + x + tile_offset]);
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
                leaf_offset = 0;
                break;
            case 2:
                leaf_height = 3;

                leaf_x_offset = 1;
                leaf_offset = 24;
                break;
            case 3:
                leaf_height = 4;

                leaf_x_offset = 1;
                leaf_offset = 24 + 18;
                break;
            case 4:
                leaf_height = 4;

                leaf_x_offset = 1;
                leaf_offset = 24 + 18 + 24;
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

                    Vector3Int position = new Vector3Int(totalX, totalY, 0);
                    leafMap.SetTile(position, leafSet[y * 6 + x + leaf_offset]);
                }
            }
        }
    }


}