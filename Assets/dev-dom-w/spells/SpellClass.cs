using UnityEngine;
using System.Collections.Generic;


namespace Spells
{
    public enum SpellType
    {
        Fire,
        Ice
    }

    // Base class for spells
    [System.Serializable]
    public abstract class Spell
    {
        public string spellName;
        public float cooldown;
        public float spellSpeed;
        public float travelDistance;
        public float damage;
        public GameObject prefab;
        public SpellType spellType;
        public float manacost;

        public float Manacost
        {
            get {return manacost; }
            set{manacost = value; }

        }
        public string SpellName
        {
            get { return spellName; }
            set { spellName = value; }
        }

        public float Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }

        public float SpellSpeed
        {
            get { return spellSpeed; }
            set { spellSpeed = value; }
        }

        public float TravelDistance
        {
            get { return travelDistance; }
            set { travelDistance = value; }
        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public GameObject Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        public SpellType SpellType
        {
            get { return spellType; }
            set { spellType = value; }
        }

        public Spell(string name, float cooldown, float speed, float distance, float damage, GameObject prefab, SpellType type, float manacost)
        {
              SpellName = name;
            Cooldown = cooldown;
            SpellSpeed = speed;
            TravelDistance = distance;
            Damage = damage;
            Prefab = prefab;
            SpellType = type;
            Manacost = manacost;
        }
    }

    // Fire spell class
    public class FireSpell : Spell
    {
        public float burnDuration;

        public float BurnDuration
        {
            get { return burnDuration; }
            set { burnDuration = value; }
        }

        public FireSpell(string name, float cooldown, float speed, float distance, float damage, float burnDuration, GameObject prefab, float manacost)
            : base(name, cooldown, speed, distance, damage, prefab, SpellType.Fire,manacost)
        {
            BurnDuration = burnDuration;
        }
    }

    // Ice spell class
    public class IceSpell : Spell
    {
        public float slowDuration;
        public float freezeDuration;

         public float SlowDuration
        {
            get { return slowDuration; }
            set { slowDuration = value; }
        }

        public float FreezeDuration
        {
            get { return freezeDuration; }
            set { freezeDuration = value; }
        }

       public IceSpell(string name, float cooldown, float speed, float distance, float damage, float slowDuration, float freezeDuration, GameObject prefab, float manacost)
            : base(name, cooldown, speed, distance, damage, prefab, SpellType.Ice, manacost)
        {
            SlowDuration = slowDuration;
            FreezeDuration = freezeDuration;
        }
    }
}
