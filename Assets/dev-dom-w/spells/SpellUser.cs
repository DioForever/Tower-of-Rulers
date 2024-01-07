using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Spells;

public class SpellUser : MonoBehaviour
{
    private bool isTextInputActive = false;
    private int qInput = 0;
    private int eInput = 0;

    private string selectedQSpellName = "";
    private string selectedESpellName = "";

    private Dictionary<int, Spell> spellDictionary = new Dictionary<int, Spell>();

    private Dictionary<string, float> spellCooldowns = new Dictionary<string, float>();

    private SpellManager spellManager;

    private void Start()
    {
        spellManager = GetComponent<SpellManager>();

        foreach (Spell spell in spellManager.spells)
        {
            spellDictionary.Add(spellDictionary.Count + 1, spell);
        }
    }

    private void Update()
    {
        // Q key input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isTextInputActive = true;
            StartCoroutine(DisplayTextAndPrompt('Q'));
        }

        // E key input
        if (Input.GetKeyDown(KeyCode.E))
        {
            isTextInputActive = true;
            StartCoroutine(DisplayTextAndPrompt('E'));
        }

        // Check pokud je Q spell zakasten
        if (Input.GetKeyDown(KeyCode.Space) && qInput != 0)
        {
            if (!IsOnCooldown(selectedQSpellName))
            {
                CastSelectedSpell(selectedQSpellName);
                StartCooldown(selectedQSpellName);
            }
            else
            {
                Debug.Log("Spell is on cooldown. Wait for the cooldown to finish.");
            }
        }

        // Check pokud je e spell zakasten
        if (Input.GetKeyDown(KeyCode.Space) && eInput != 0)
        {
            if (!IsOnCooldown(selectedESpellName))
            {
                CastSelectedSpell(selectedESpellName);
                StartCooldown(selectedESpellName);
            }
            else
            {
                Debug.Log("Spell is on cooldown. Wait for the cooldown to finish.");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTextInputActive = false;
        }

        // Update cooldowns
        UpdateCooldowns();
    }

    private System.Collections.IEnumerator DisplayTextAndPrompt(char key)
    {
        Debug.Log($"Press Tab again to stop, and input a number from 1 to {spellDictionary.Count} for spell on key {key}.");

        while (isTextInputActive)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isTextInputActive = false;
                break;
            }

            int newInput;
            if (int.TryParse(Input.inputString, out newInput))
            {
                if (newInput >= 1 && newInput <= spellDictionary.Count)
                {
                    if (key == 'Q')
                    {
                        qInput = newInput;
                        selectedQSpellName = spellDictionary[qInput].SpellName;
                        Debug.Log($"Player selected Q spell: {selectedQSpellName}");
                    }
                    else if (key == 'E')
                    {
                        eInput = newInput;
                        selectedESpellName = spellDictionary[eInput].SpellName;
                        Debug.Log($"Player selected E spell: {selectedESpellName}");
                    }
                    else
                    {
                        Debug.LogError("Invalid key selection.");
                    }
                }
                else
                {
                    Debug.Log($"Please enter a number between 1 and {spellDictionary.Count}.");
                }
            }

            yield return null;
        }
    }

    private void CastSelectedSpell(string spellName)
    {
        if (!string.IsNullOrEmpty(spellName))
        {
            Type scriptType = Type.GetType(spellName);
            if (scriptType != null)
            {
                Component newScriptInstance = gameObject.AddComponent(scriptType);
            }
            else
            {
                Debug.LogError($"Script type not found: {spellName}");
            }
        }
        else
        {
            Debug.LogError("No spell selected to cast.");
        }
    }

    private bool IsOnCooldown(string spellName)
    {
        return spellCooldowns.ContainsKey(spellName) && spellCooldowns[spellName] > 0;
    }

    private void StartCooldown(string spellName)
    {
        float cooldown = spellDictionary.Values.First(spell => spell.SpellName == spellName).Cooldown;

        if (spellCooldowns.ContainsKey(spellName))
        {
            spellCooldowns[spellName] = cooldown;
        }
        else
        {
            spellCooldowns.Add(spellName, cooldown);
        }
    }

    private void UpdateCooldowns()
    {
        foreach (var spell in spellCooldowns.Keys.ToList())
        {
            if (spellCooldowns[spell] > 0)
            {
                spellCooldowns[spell] -= Time.deltaTime;
            }
            else
            {
                spellCooldowns[spell] = 0;
            }
        }
    }
}