using UnityEngine;
using static GenerationteInitiator;
using FloorSystem;

public class PlayerSaveSystem : MonoBehaviour
{
    // Define keys for saving data
    private const string HealthKeyPrefix = "PlayerHealth_";
    private const string ManaKeyPrefix = "PlayerMana_";
    private const string FloorKeyPrefix = "FloorData_";

    public playerControl playerControl;
    public FloorSystem.Floor floorSystem;

    // Save player data to a specific save slot
    public void SavePlayerData(string saveSlotName)
    {
        PlayerPrefs.SetFloat(HealthKeyPrefix + saveSlotName, playerControl.health);
        PlayerPrefs.SetFloat(ManaKeyPrefix + saveSlotName, playerControl.mana);
        PlayerPrefs.Save();
    }

    
    public void LoadPlayerData(string saveSlotName)
    {
        playerControl.health = PlayerPrefs.GetFloat(HealthKeyPrefix + saveSlotName, playerControl.health);
        playerControl.mana = PlayerPrefs.GetFloat(ManaKeyPrefix + saveSlotName, playerControl.mana);
    }

   
    public static void SaveFloorData(string saveSlotName)
    {
        string floorDataJson = JsonUtility.ToJson(GenerationteInitiator.floor_);
        PlayerPrefs.SetString(FloorKeyPrefix + saveSlotName, floorDataJson);
        PlayerPrefs.Save();
        Debug.Log("Floor data saved for slot " + saveSlotName);
        Debug.Log(floorDataJson);
    }

    
    public FloorSystem.Floor LoadFloorData(string saveSlotName)
    {
        string floorDataJson = PlayerPrefs.GetString(FloorKeyPrefix + saveSlotName);
        FloorSystem.Floor floor = JsonUtility.FromJson<FloorSystem.Floor>(floorDataJson);
        return floor;
    }
}