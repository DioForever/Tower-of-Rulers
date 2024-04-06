using UnityEngine;

public class PlayerSaveSystem : MonoBehaviour
{
    // The keys used to save and load player data
    private const string HealthKey = "PlayerHealth";
    private const string ManaKey = "PlayerMana";

    // Reference to the PlayerControl script
    public PlayerControl playerControl;

    void OnApplicationQuit()
    {
        SavePlayerData();
    }

   

    // Function to save player data
    public void SavePlayerData()
    {
        // Save health and mana to PlayerPrefs
        PlayerPrefs.SetFloat(HealthKey, playerControl.health);
        PlayerPrefs.SetFloat(ManaKey, playerControl.mana);
        PlayerPrefs.Save();
    }

    // Function to load player data
    public void LoadPlayerData()
    {
        // Load health and mana from PlayerPrefs
        playerControl.health = PlayerPrefs.GetFloat(HealthKey, playerControl.health);
        playerControl.mana = PlayerPrefs.GetFloat(ManaKey, playerControl.mana);
    }

   
}