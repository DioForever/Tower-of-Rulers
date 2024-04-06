using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    // Reference to the in-game menu GameObject
    public GameObject inGameMenu;

    void Start()
    {
        // Get the button component
        Button button = GetComponent<Button>();

        // Add a listener to the button's click event
        button.onClick.AddListener(OnClick);
    }

    // Function to call when the button is clicked
    void OnClick()
    {
      
            // Deactivate the in-game menu
            inGameMenu.SetActive(false);
        
    }
}