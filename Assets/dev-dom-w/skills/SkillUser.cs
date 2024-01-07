using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Skills;

public class SkillUser : MonoBehaviour
{
    private bool isTextInputActive = false;
    private int userInput = 0;
    private string selectedSkillName = "";

    

    private Dictionary<int, Skill> skillDictionary = new Dictionary<int, Skill>();

    private Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();

    private SkillManager skillManager;

    private void Start()
    {
        
        

        
        foreach (Skill skill in skillManager.skills)
        {
            skillDictionary.Add(skillDictionary.Count + 1, skill);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && userInput != 0)
        {
            if (!IsOnCooldown(selectedSkillName))
            {
                CastSelectedSkill();
                StartCooldown(selectedSkillName);
            }
            else
            {
                Debug.Log("Skill is on cooldown. Wait for the cooldown to finish.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTextInputActive = !isTextInputActive;

            if (isTextInputActive)
            {
                StartCoroutine(DisplayTextAndPrompt());
            }
        }

        UpdateCooldowns();
    }

    private System.Collections.IEnumerator DisplayTextAndPrompt()
    {
        // ukáže text na obrazovce
        Debug.Log("Press Tab again to stop, and input a number from 1 to " + skillDictionary.Count + ".");

        yield return null;

        while (isTextInputActive)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isTextInputActive = false; // jenom jeden input
                break;
            }

            // input od hráče
            int newInput;
            if (int.TryParse(Input.inputString, out newInput))
            {
                if (newInput >= 1 && newInput <= skillDictionary.Count)
                {
                    
                    userInput = newInput;

                    // dostane skill z dictionary
                    Skill selectedSkill;
                    if (skillDictionary.TryGetValue(userInput, out selectedSkill))
                    {
                        selectedSkillName = selectedSkill.SkillName;
                        Debug.Log("Player selected skill: " + selectedSkillName);
                    }
                    else
                    {
                        Debug.LogError("Skill not found in the dictionary.");
                    }
                }
                else
                {
                    Debug.Log("Please enter a number between 1 and " + skillDictionary.Count + ".");
                }
            }

            yield return null;
        }
    }

    private void CastSelectedSkill()
    {
        if (!string.IsNullOrEmpty(selectedSkillName))
        {
            // reflexe pro dostání skilluscriptu
            Type scriptType = Type.GetType(selectedSkillName);
            if (scriptType != null)
            {
                
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

    private bool IsOnCooldown(string skillName)
    {
        return skillCooldowns.ContainsKey(skillName) && skillCooldowns[skillName] > 0;
    }

    private void StartCooldown(string skillName)
    {
        //dostanout cooldown value ze skillu
        float cooldown = skillDictionary.Values.First(skill => skill.SkillName == skillName).Cooldown;

        if (skillCooldowns.ContainsKey(skillName))
        {
            skillCooldowns[skillName] = cooldown;
        }
        else
        {
            skillCooldowns.Add(skillName, cooldown);
        }
    }

    private void UpdateCooldowns()
    {
        foreach (var skill in skillCooldowns.Keys.ToList())
        {
            if (skillCooldowns[skill] > 0)
            {
                skillCooldowns[skill] -= Time.deltaTime;
            }
            else
            {
                skillCooldowns[skill] = 0;
            }
        }
    }
}