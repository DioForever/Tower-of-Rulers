using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Spells;

public class SpellUser : MonoBehaviour
{

    private SpellCaster spellCaster;
    private bool isTextInputActive = false;
    private bool isWaitingForKeyPress = false;
    private int qInput = 0;
    private int eInput = 0;

    private string selectedQSpellName = "";
    private string selectedESpellName = "";

    private Dictionary<int, Spell> spellDictionary = new Dictionary<int, Spell>();

    private Dictionary<string, float> spellCooldowns = new Dictionary<string, float>();

    private SpellManager spellManager;

    private void Awake()
    {
        spellManager = GetComponent<SpellManager>();

        spellCaster = GetComponent<SpellCaster>();

        foreach (Spell spell in spellManager.spells)
        {
            spellDictionary.Add(spellDictionary.Count + 1, spell);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isWaitingForKeyPress = true;
            Debug.Log("press Q or E to select spells on said keys");
        }

        // Check for key press to start the spell selection process
        if (isWaitingForKeyPress)
        {
            // Check if Q key is pressed
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isWaitingForKeyPress = false; // Reset flag
                isTextInputActive = true; // Start the spell selection process
                StartCoroutine(DisplayTextAndPrompt('Q'));
            }
            // Check if E key is pressed
            else if (Input.GetKeyDown(KeyCode.E))
            {
                isWaitingForKeyPress = false; // Reset flag
                isTextInputActive = true; // Start the spell selection process
                StartCoroutine(DisplayTextAndPrompt('E'));
            }
        }

       

        // Check if Tab key is released to deactivate selection
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTextInputActive = false;
        }

        // Check pokud je Q spell zakasten
        if (Input.GetKeyDown(KeyCode.Q) && qInput != 0)
        {
            
            if (!IsOnCooldown(selectedQSpellName))
            {
                CastSelectedSpell(qInput);
                StartCooldown(selectedQSpellName);
            }
            else
            {
                Debug.Log("Spell is on cooldown. Wait for the cooldown to finish.");
            }
        }

        // Check pokud je e spell zakasten
        if (Input.GetKeyDown(KeyCode.E) && eInput != 0)
        {
            if (!IsOnCooldown(selectedESpellName))
            {
                CastSelectedSpell(eInput);
                StartCooldown(selectedESpellName);
            }
            else
            {
                Debug.Log("Spell is on cooldown. Wait for the cooldown to finish.");
            }
        }

        // Update cooldowns
        UpdateCooldowns();
    }

    private System.Collections.IEnumerator DisplayTextAndPrompt(char key)
    {
        Debug.Log($"Press Tab again to stop, or input a number from 1 to {spellDictionary.Count} for spell on key {key}.");

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

    private void CastSelectedSpell(int spellIndex)
    {
        if (spellDictionary.ContainsKey(spellIndex))
        {
            Spell spell = spellDictionary[spellIndex];
            if (spell != null)
            {
                spellCaster.CastSpell(spellIndex);
                StartCooldown(spell.SpellName);
            }
            else
            {
                Debug.LogError($"Spell not found at index: {spellIndex}");
            }
        }
        else
        {
            Debug.LogError($"Spell index not found: {spellIndex}");
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