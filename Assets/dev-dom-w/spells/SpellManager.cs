using System.Collections.Generic;
using UnityEngine;
using Spells;

public class SpellManager : MonoBehaviour
{
    public List<Spell> spells = new List<Spell>();

    private void InitializeSpells()
    {
        // Add FireSpells to the list
        AddFireSpell("Fireball", 8.0f, 7.0f, 15.0f, 20.0f, 1.0f, null, 20.0f);
        AddFireSpell("Firelance", 18.0f, 10.0f, 35.0f, 20.0f, 0.0f, null, 25.0f);
        AddFireSpell("Fireboomerang", 10.0f, 8.0f, 12.0f, 8.0f, 2.0f, null, 15.0f);
        AddFireSpell("Fire3shooter", 10.0f, 7.0f, 13.0f, 9.0f, 0.0f, null, 22.0f);
        AddFireSpell("Fireaura", 13.0f, 10.0f, 5.0f, 5.0f, 3.0f, null, 13.0f);

        // Add IceSpells to the list
        AddIceSpell("Iceball", 10.0f, 10.0f, 30.0f, 12.0f, 1.5f, 0.0f, null, 20.0f);
        AddIceSpell("Ice3shooter", 15.0f, 8.0f, 13.0f, 6.0f, 0.5f, 0.2f, null, 15.0f);
        AddIceSpell("Icedestroy", 18.0f, 500.0f, 250.0f, 0.0f, 0.0f, 0.0f, null, 35.0f);
        AddIceSpell("Icefreeze", 20.0f, 200.0f, 55.0f, 0.0f, 0.0f, 3.0f, null, 28.0f);
        AddIceSpell("Iceslow", 15.0f, 200.0f, 55.0f, 0.0f, 5.0f, 0.0f, null, 26.0f);
    }

    private void AddFireSpell(string name, float cooldown, float speed, float distance, float damage, float burnDuration, GameObject prefab, float additionalProperty)
    {
        spells.Add(new FireSpell(name, cooldown, speed, distance, damage, burnDuration, prefab, additionalProperty));
    }

    private void AddIceSpell(string name, float cooldown, float speed, float distance, float damage, float slowDuration, float freezeDuration, GameObject prefab, float additionalProperty)
    {
        spells.Add(new IceSpell(name, cooldown, speed, distance, damage, slowDuration, freezeDuration, prefab, additionalProperty));
    }
}
