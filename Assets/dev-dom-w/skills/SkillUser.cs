using UnityEngine;
using System.Collections.Generic;
using System;

public class SkillUser : MonoBehaviour
{
    private bool isTextInputActive = false;
    private int userInput = 0;

    private Dictionary<int, string> skillDictionary = new Dictionary<int, string>
    {
        { 1, "ManaReplenish" },
        { 2, "DamageBoost" },
        { 3, "HealthReplenish" },
        { 4, "Flash" },
        { 5, "Dash" }
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && userInput !=0)
        {
            DashScript otherScriptInstance = gameObject.AddComponent<DashScript>();
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
        // display text na obrazovce
        Debug.Log("Press Tab again to stop, and input a number from 1 to 5.");

        
        yield return null;

        while (isTextInputActive)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isTextInputActive = false; //aby se nezmačknul tab výckrát
                break;
            }

            //tady se dostane input
            int newInput;
            if (int.TryParse(Input.inputString, out newInput))
            {
                if (newInput >= 1 && newInput <= 5)
                {
                    // save input playera
                    userInput = newInput;

                    // dastaneš value podle key inputu
                    string selectedSkillName;
                    if (skillDictionary.TryGetValue(userInput, out selectedSkillName))
                    {
                        Debug.Log("Player selected skill: " + selectedSkillName);

                        // použitá reflexe aby se dostal chtěný script name
                        Type scriptType = Type.GetType(selectedSkillName);
                        if (scriptType != null)
                        {
                            Component otherScriptInstance = gameObject.AddComponent(scriptType);
                           
                        }
                        else
                        {
                            Debug.LogError("Script type not found: " + selectedSkillName);
                        }
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
}