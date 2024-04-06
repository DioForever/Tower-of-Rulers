using UnityEngine;
using Floor;

public class PlayerSaveSystem : MonoBehaviour
{
    // klíče na saving dat
    private const string HealthKey = "PlayerHealth";
    private const string ManaKey = "PlayerMana";
    private const string FloorKey = "FloorData";

    
    public PlayerControl playerControl;
    public FloorSystem.Floor floorSystem;

    void OnApplicationQuit()
    {
        SavePlayerData();
        SaveFloorData();
    }

   

    // Funkce SAVE playera
    public void SavePlayerData()
    {
        
        PlayerPrefs.SetFloat(HealthKey, playerControl.health);
        PlayerPrefs.SetFloat(ManaKey, playerControl.mana);
        PlayerPrefs.Save();
    }

    // Funkce load Playera
    public void LoadPlayerData()
    {
        
        playerControl.health = PlayerPrefs.GetFloat(HealthKey, playerControl.health);
        playerControl.mana = PlayerPrefs.GetFloat(ManaKey, playerControl.mana);
    }

    // Funkce na SAVE floor
    
    public void SaveFloorData()
    {
        // convertuje floor objekt na JSON string
        string floorDataJson = JsonUtility.ToJson(floorSystem.floor);
        
       
        PlayerPrefs.SetString(FloorKey, floorDataJson);
        PlayerPrefs.Save();
    }

    // Funkce na load Floor
    public Floor LoadFloorData()
    {
        
        string floorDataJson = PlayerPrefs.GetString(FloorKey);
        
        // Convert JSON string to floor data
        Floor floor = JsonUtility.FromJson<Floor>(floorDataJson);
        
        
        return floor;
    }
    
   
}