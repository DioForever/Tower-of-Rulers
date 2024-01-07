using UnityEngine;
using System.Collections.Generic;
using System;

public class SpellUser : MonoBehaviour
{
    private bool isTextInputActive = false;
    private int userInput = 0;
    private string selectedSkillName = "";

    private Dictionary<int, string> skillDictionary = new Dictionary<int, string>
    {
        { 1, "FireballScript" },
        { 2, "FirelanceScript" },
        { 3, "FireboomerangScript" },
        { 4, "Fire3shooterScript" },
        { 5, "FireauraScript" },
        { 6, "IceballScript" },
        { 7, "Ice3shooterScript" },
        { 8, "IcedestroyScript" },
        { 9, "IcefreezeScript" },
        { 10, "IceslowScript" }
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && userInput != 0)
        {
            CastSelectedSkill();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTextInputActive = !isTextInputActive;

            if (isTextInputActive)
            {
                StartCoroutine(DisplayTextAndPrompt());
            }
        }
    }

    private System.Collections.IEnumerator DisplayTextAndPrompt()
    {
        // Display text on the screen
        Debug.Log("Press Tab again to stop, and input a number from 1 to 5.");

        yield return null;

        while (isTextInputActive)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isTextInputActive = false; // Stop the text input
                break;
            }

            // Get input from the player
            int newInput;
            if (int.TryParse(Input.inputString, out newInput))
            {
                if (newInput >= 1 && newInput <= 10)
                {
                    // Save the player's input
                    userInput = newInput;

                    // Retrieve the script name from the dictionary
                    if (skillDictionary.TryGetValue(userInput, out selectedSkillName))
                    {
                        Debug.Log("Player selected skill: " + selectedSkillName);
                    }
                    else
                    {
                        Debug.LogError("Skill not found in the dictionary.");
                    }
                }
                else
                {
                    Debug.Log("Please enter a number between 1 and 5.");
                }
            }

            yield return null;
        }
    }

    private void CastSelectedSkill()
    {
        if (!string.IsNullOrEmpty(selectedSkillName))
        {
            // Use reflection to dynamically instantiate the script based on the dictionary value
            Type scriptType = Type.GetType(selectedSkillName);
            if (scriptType != null)
            {
                // Add the component for casting the skill
                Component newScriptInstance = gameObject.AddComponent(scriptType);
            }
            else
            {
                Debug.LogError("Script type not found: " + selectedSkillName);
            }
        }
        else
        {
            Debug.LogError("No skill selected to cast.");
        }
    }
}