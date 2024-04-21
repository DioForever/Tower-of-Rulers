using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class UtilsOW : MonoBehaviour
{
    public static void LoadTree(Tilemap baseMap, Tilemap leafMap, int x_base, int y_base, int baseType, int leafsType, GameObject prefabBase, GameObject prefabLeafs, List<GameObject> gameObjectsSpawned)
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
            GameObject gl = Instantiate(prefabLeafs, spawnPositionLeafs, Quaternion.identity);
            gameObjectsSpawned.Add(gl);
        }

        Vector3 spawnPositionBase = baseMap.GetCellCenterWorld(new Vector3Int((int)(x_base + 2), (int)(y_base + 1), 0));
        spawnPositionBase.x -= 0.5f;

        // Instantiate the base at the calculated position
        GameObject go = Instantiate(prefabBase, spawnPositionBase, Quaternion.identity);
        gameObjectsSpawned.Add(go);
    }

    public static void LoadBuilding(Tilemap wallMap, int x_base, int y_base, GameObject prefabBuilding, List<GameObject> gameObjectsSpawned)
    {
        Vector3 spawnPositionBuilding = wallMap.GetCellCenterWorld(new Vector3Int((int)(x_base + 2), (int)(y_base + 2), 0));
        // spawnPositionBuilding.x -= 0.5f;
        // spawnPositionBuilding.y -= 0.5f;

        // Instantiate the building at the calculated position
        GameObject go = Instantiate(prefabBuilding, spawnPositionBuilding, Quaternion.identity);
        gameObjectsSpawned.Add(go);
    }

    public static void LoadTeleport(Tilemap wallMap, float x_base, float y_base, GameObject prefabTeleport, List<GameObject> gameObjectsSpawned)
    {
        // Vector3 spawnPositionTeleport = wallMap.GetCellCenterWorld(new Vector3Int((int)(x_base + 2), (int)(y_base + 2), 0));
        Vector3 spawnPositionTeleport = new Vector3(x_base, y_base, 0);
        // spawnPositionBuilding.x -= 0.5f;
        // spawnPositionBuilding.y -= 0.5f;

        // Instantiate the building at the calculated position
        GameObject go = Instantiate(prefabTeleport, spawnPositionTeleport, Quaternion.identity);
        gameObjectsSpawned.Add(go);
    }

    public static void LoadDecoration(Tilemap wallMap, int x_base, int y_base, GameObject prefabDec, List<GameObject> gameObjectsSpawned)
    {
        Vector3 spawnPositionDecoration = wallMap.GetCellCenterWorld(new Vector3Int((int)(x_base + 2), (int)(y_base + 2), 0));
        // spawnPositionBuilding.x -= 0.5f;
        // spawnPositionBuilding.y -= 0.5f;

        // Instantiate the building at the calculated position
        GameObject go = Instantiate(prefabDec, spawnPositionDecoration, Quaternion.identity);
        gameObjectsSpawned.Add(go);
    }


}