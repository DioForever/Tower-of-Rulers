using System.Collections.Generic;
using UnityEngine;
using Spell;

public class SpellManager : MonoBehaviour
{
    public List<Spell> spells = new List<Spell>();

    private void Awake()
    {
        InitializeSpells();
    }

    private void InitializeSpells()
    {
        // Create and add FireSpells to the list
        spells.Add(new FireSpell("Fireball", 8.0f, 7.0f, 15.0f, 20.0f, 1.0f, null, 20.0f));
        spells.Add(new FireSpell("Firelance", 18.0f, 10.0f, 35.0f, 20.0f, 0.0f, null, 25.0f));
        spells.Add(new FireSpell("Fireboomerang", 10.0f, 8.0f, 12.0f, 8.0f, 2.0f, null, 15.0f));
        spells.Add(new FireSpell("Fire3shooter", 10.0f, 7.0f, 13.0f, 9.0f, 0.0f, null, 22.0f));
        spells.Add(new FireSpell("Fireaura", 13.0f, 10.0f, 5.0f, 5.0f, 3.0f, null, 13.0f));

        // Create and add IceSpells to the list
        spells.Add(new IceSpell("Iceball", 10.0f, 10.0f, 30.0f, 12.0f, 1.5f, 0.0f, null));
        spells.Add(new IceSpell("Ice3shooter", 15.0f, 8.0f, 13.0f, 6.0f, 0.5f, 0.2f, null));
        spells.Add(new IceSpell("Icedestroy", 18.0f, 500.0f, 250.0f, 0.0f, 0.0f, 0.0f, null));
        spells.Add(new IceSpell("Icefreeze", 20.0f, 200.0f, 55.0f, 0.0f, 0.0f, 3.0f, null));
        spells.Add(new IceSpell("Iceslow", 15.0f, 200.0f, 55.0f, 0.0f, 5.0f, 0.0f, null));
    }
}