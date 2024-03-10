using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class UtilsOW : MonoBehaviour
{
    public static void LoadTree(Tilemap baseMap, Tilemap leafMap, int x_base, int y_base, int baseType, int leafsType, GameObject prefabBase, GameObject prefabLeafs)
    {

        bool noLeafs = false;

        float leaf_x_offset = 0f;
        float base_y_offset = 3f;


        switch (baseType)
        {
            case 1:
                base_y_offset = 2;
                break;
            case 2:
            case 3:
            case 4:
                base_y_offset = 4;
                break;
            case 5:
                noLeafs = true;
                break;
        }

        switch (leafsType)
        {
            case 0:
                noLeafs = true;
                break;
            case 1:
                leaf_x_offset = 0;
                break;
            case 2:
                leaf_x_offset = 0;
                break;
            case 3:
                leaf_x_offset = 1.15f;
                break;
            case 4:
                leaf_x_offset = 0;
                break;
        }

        if (!noLeafs)
        {
            Vector3 spawnPositionLeafs = leafMap.GetCellCenterWorld(new Vector3Int((int)(x_base + leaf_x_offset + 1), (int)(y_base + base_y_offset + 1), 0));
            spawnPositionLeafs.x += 0.5f;

            // Instantiate the leafs at the calculated position
            Instantiate(prefabLeafs, spawnPositionLeafs, Quaternion.identity);
        }

        Vector3 spawnPositionBase = baseMap.GetCellCenterWorld(new Vector3Int((int)(x_base + 2), (int)(y_base + 1), 0));
        spawnPositionBase.x -= 0.5f;

        // Instantiate the base at the calculated position
        Instantiate(prefabBase, spawnPositionBase, Quaternion.identity);
    }


}