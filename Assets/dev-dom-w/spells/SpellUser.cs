using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spells;

public class SpellUser : MonoBehaviour
{
    public SpellManager spellManager; // Reference to the SpellManager
    private Spell selectedSpellQ;
    private Spell selectedSpellE;
    private Dictionary<Spell, float> spellCooldowns = new Dictionary<Spell, float>();

    private void Start()
    {
        // Assume you have a SpellManager component in the scene or set it programmatically
        // You can drag and drop the SpellManager object into the inspector field
        // Or set it programmatically using spellManager = someGameObject.GetComponent<SpellManager>();

        // Set default spells for Q and E
        SetSpellForQ("Fireball");
        SetSpellForE("Iceball");
    }

    private void Update()
    {
        // Update cooldowns
        UpdateCooldowns();

        // Check for input to cast selected spells
        if (Input.GetKeyDown(KeyCode.Q) && selectedSpellQ != null )
        {
            // Cast the spell
            CastSpell(selectedSpellQ);
        }
        else if (Input.GetKeyDown(KeyCode.E) && selectedSpellE != null )
        {
            // Cast the spell
            CastSpell(selectedSpellE);
        }
    }

    private void CastSpell(Spell spell)
{
    // Check if the spell is on cooldown
    if (spellCooldowns.TryGetValue(spell, out float cooldown) && cooldown > 0)
    {
        Debug.Log($"Spell {spell.spellName} is on cooldown!");
    }
    else
    {
        // Cast the spell
        Debug.Log($"Casting {spell.spellName}!");

        // Instantiate the spell prefab at the player's position with the player's rotation
        //GameObject spellInstance = Instantiate(spell.prefab, transform.position, transform.rotation);

        // Access the Spell component attached to the instantiated prefab
        

        

        // Optionally, you can do more with the spellInstance, such as set its owner, apply effects, etc.

        // Start the spell cooldown
        StartSpellCooldown(spell);
    }
}

    private void StartSpellCooldown(Spell spell)
    {
        if (spellCooldowns.ContainsKey(spell))
        {
            spellCooldowns[spell] = spell.cooldown;
        }
        else
        {
            spellCooldowns.Add(spell, spell.cooldown);
        }
    }

    private void UpdateCooldowns()
    {
        // Update cooldowns for all spells
        foreach (var spell in spellCooldowns.Keys.ToList())
        {
            if (spellCooldowns[spell] > 0)
            {
                spellCooldowns[spell] -= Time.deltaTime;
            }
        }
    }

    private void SetSpellForQ(string spellName)
    {
        selectedSpellQ = GetSpellByName(spellName);
    }

    private void SetSpellForE(string spellName)
    {
        selectedSpellE = GetSpellByName(spellName);
    }

    private Spell GetSpellByName(string spellName)
    {
        Spell spell = spellManager.spells.Find(s => s.spellName == spellName);
        if (spell == null)
        {
            Debug.LogError($"Spell with name {spellName} not found!");
        }
        return spell;
    }
}