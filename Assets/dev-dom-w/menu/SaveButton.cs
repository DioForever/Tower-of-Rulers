using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    public GameSaveBox gameSaveBox;

    public PlayerSaveSystem playerSaveSystem;


    void Start()
    {
        // Get the button component
        Button button = GetComponent<Button>();

        // Add a listener to the button's click event
        button.onClick.AddListener(SavePlayerData);
    }

    // Function to call when the button is clicked
    void SavePlayerData()
    {
        // Call the SavePlayerData function from PlayerSaveSystem
        //    playerSaveSystem.SavePlayerData();
        PlayerSaveSystem.SaveFloorData(gameSaveBox.gameName);

    }
}