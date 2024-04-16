using UnityEngine;
using System.Collections.Generic;

namespace Spells
{
    public class SpellManager : MonoBehaviour
    {
        public List<Spell> spells = new List<Spell>();

        private void Awake()
        {
            InitializeSpells();
        }

        private void InitializeSpells()
        {
            // Add FireSpells to the list
            spells.Add(CreateFireSpell("Fireball", 8.0f, 7.0f, 15.0f, 20.0f, 1.0f, 20.0f));
            spells.Add(CreateFireSpell("Firelance", 18.0f, 10.0f, 35.0f, 20.0f, 0.0f, 25.0f));
            spells.Add(CreateFireSpell("Fireboomerang", 10.0f, 8.0f, 12.0f, 8.0f, 2.0f, 15.0f));
            spells.Add(CreateFireSpell("Fire3shooter", 10.0f, 7.0f, 13.0f, 9.0f, 0.0f, 22.0f));
            spells.Add(CreateFireSpell("Fireaura", 13.0f, 10.0f, 5.0f, 5.0f, 3.0f, 13.0f));
            
            // Add IceSpells to the list
            spells.Add(CreateIceSpell("Iceball", 10.0f, 10.0f, 30.0f, 12.0f, 1.5f, 0.0f, 20.0f));
            spells.Add(CreateIceSpell("Ice3shooter", 15.0f, 8.0f, 13.0f, 6.0f, 0.5f, 0.2f, 15.0f));
            spells.Add(CreateIceSpell("Icedestroy", 18.0f, 500.0f, 250.0f, 0.0f, 0.0f, 0.0f, 35.0f));
            spells.Add(CreateIceSpell("Icefreeze", 20.0f, 200.0f, 55.0f, 0.0f, 0.0f, 3.0f, 28.0f));
            spells.Add(CreateIceSpell("Iceslow", 15.0f, 200.0f, 55.0f, 0.0f, 5.0f, 0.0f, 26.0f));
        }

        private FireSpell CreateFireSpell(string name, float cooldown, float speed, float distance, float damage, float burnDuration, float manacost)
        {
            FireSpell fireSpell = ScriptableObject.CreateInstance<FireSpell>();
            fireSpell.SpellName = name;
            fireSpell.Cooldown = cooldown;
            fireSpell.SpellSpeed = speed;
            fireSpell.TravelDistance = distance;
            fireSpell.Damage = damage;
            fireSpell.BurnDuration = burnDuration;
            fireSpell.Manacost = manacost;
            return fireSpell;
        }

        private IceSpell CreateIceSpell(string name, float cooldown, float speed, float distance, float damage, float slowDuration, float freezeDuration, float manacost)
        {
            IceSpell iceSpell = ScriptableObject.CreateInstance<IceSpell>();
            iceSpell.SpellName = name;
            iceSpell.Cooldown = cooldown;
            iceSpell.SpellSpeed = speed;
            iceSpell.TravelDistance = distance;
            iceSpell.Damage = damage;
            iceSpell.SlowDuration = slowDuration;
            iceSpell.FreezeDuration = freezeDuration;
            iceSpell.Manacost = manacost;
            return iceSpell;
        }
    }
}