using System.IO;
using UnityEngine;

public class LoadGameSaves : MonoBehaviour
{
    public Object prefabGameSave;

    public delegate void OnManageGameSaves();
    public static OnManageGameSaves onManageGameSaves;

    
    // Start is called before the first frame update
    void Start()
    {
        SpawnGameSaves();
        onManageGameSaves += SaveManager.Instance.ClearSpawnedObjects;
        onManageGameSaves += SpawnGameSaves;
        
    }


    void ReloadGameSaves()
    {
        SaveManager.Instance.ClearSpawnedObjects();
        SpawnGameSaves();
    }



    private void SpawnGameSaves(){
        if(File.Exists("./GameSaves.save")){
        string[] gameSaves = File.ReadAllLines("./GameSaves.save");

        // If its less than 3, there isnt even 1 valid game save

        if(gameSaves.Length < 3) return;

        // We parse input from the file and setup game save prefab for each one
        int gameSaveCount = 0;
        int.TryParse(gameSaves[0], out gameSaveCount);

        int counter = 1;
        for(int gs = 0; gs < gameSaveCount; gs++){
            int floorNumber = 0;
            int gameId;
            string[] gameNameArray;
            string gId = gameSaves[counter+gs*3].Split(" ")[1];
            string gFloor = gameSaves[counter+gs*3+1].Split(" ")[1];
            int.TryParse(gId, out gameId);
            int.TryParse(gFloor, out floorNumber);
            if(floorNumber <= 0 || gameId == null) continue;

            gameNameArray = gameSaves[counter+gs*3+2].Split(" ");
            gameNameArray[0] = "";
            string gameName = string.Join(" ", gameNameArray);
            


            Debug.Log($"Spawned {gameId} {floorNumber} {gameName}");

            SaveManager.Instance.Spawn(gameId, floorNumber, gameName);
        }

        }else{
            File.Create("./GameSaves.save");
        }

    }
}
