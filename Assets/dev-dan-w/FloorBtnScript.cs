using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloorSystem;
public class FloorBtnScript : MonoBehaviour
{
    public Floor floor;
    public GameObject floorMenu;
    public int floorNumber = 1;
    public bool newFloor = false;
    private GenerationteInitiator generationInitiator;

    void Start()
    {
        // Get the GenerationInitiator component from the current scene
        generationInitiator = FindObjectOfType<GenerationteInitiator>();
        if (generationInitiator == null)
        {
            Debug.LogError("GenerationInitiator not found in the scene.");
        }
    }

    public void OnClick()
    {
        LoadNextFloor();
    }

    void LoadNextFloor()
    {
        if(newFloor)
        {
            // Create a new floor
            generationInitiator.GenerateFloor(GetHighestFloor() + 1);

        }


        if (generationInitiator != null)
        {
            // Check if the floor is already generated
            bool generated = false;
            generationInitiator.LoadFloor(generated, GetHighestFloor() + 1);
            floorMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("GenerationInitiator not found.");
        }
    }

    public static int GetHighestFloor()
    {
        return PlayerPrefs.GetInt("highestFloor", 1);
    }
}
