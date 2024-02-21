using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameSaveBox : MonoBehaviour
{
    public TextMeshProUGUI  floorNumDisplay;
    public TextMeshProUGUI  gameNameDisplay;
    public int floorAchieved;
    public string gameName;
    // Start is called before the first frame update
    public void EditGameSave(string newName){
        if(newName != "" || newName != null){
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
}
