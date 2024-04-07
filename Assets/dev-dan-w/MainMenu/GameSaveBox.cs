using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

using FloorSystem; 

public class GameSaveBox : MonoBehaviour
{
    public PlayerSaveSystem playerSaveSystem;
    public GenerationteInitiator generationteInitiator;

    public TextMeshProUGUI floorNumDisplay;
    public TextMeshProUGUI gameNameDisplay;
    public int floorAchieved;
    public string gameName;

    
    public void EditGameSave(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            gameNameDisplay.text = newName;
            gameName = newName;
        }
    }

    void Start()
    {
        floorNumDisplay.text = floorAchieved.ToString();
        gameNameDisplay.text = gameName;
    }

    // Update is called once per frame
    void Update()
    {
        floorNumDisplay.text = floorAchieved.ToString();
        gameNameDisplay.text = gameName;
    }

    public void onClick()
    {
        SceneManager.LoadScene("Floor"); 

       
        FloorSystem.Floor floor = playerSaveSystem.LoadFloorData(gameName);
        generationteInitiator.LoadFloor(false, false, floor.floorNumber);

        
        playerSaveSystem.LoadPlayerData(gameName);
    }
}