using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    public GameObject inGameMenu;

    void Start()
    {
        // Make sure the in-game menu is initially disabled
        inGameMenu.SetActive(false);
    }

    void Update()
    {
        // Check if the player presses the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the in-game menu
            inGameMenu.SetActive(!inGameMenu.activeSelf);
        }
    }
}