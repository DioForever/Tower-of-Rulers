using UnityEngine;
using System.Collections.Generic;


namespace Skills
{
    public enum SkillType
    {
        StatBoost,
        Movement
    }
   

    // Base class for skills
    [System.Serializable]
    public abstract class Skill
    {
        private string skillName;
        private float cooldown;
        private GameObject prefab;
        private float manacost;

        public SkillType skillType;

        public float Manacost
        {
            get {return manacost; }
            set{manacost = value; }

        }
        public string SkillName
        {
            get { return skillName; }
            set { skillName = value; }
        }

        public float Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }

        public GameObject Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

         public SkillType SkillType
        {
            get { return skillType; }
            set { skillType = value; }
        }


        public Skill(string name, float cooldown, GameObject prefab, SkillType type, float manacost)
        {
            SkillName = name;
            Cooldown = cooldown;
            Prefab = prefab;
            SkillType = type;
            Manacost = manacost;
        }
    }

    public class StatBoostSkill : Skill
    {

        private float statboostvalue;
        public float StatBoost
        {
            get{return statboostvalue;}
            set{statboostvalue = value;}
        }

        public StatBoostSkill(string name, float cooldown, GameObject prefab, float manacost,float statBoost)
        :base(name, cooldown, prefab, SkillType.StatBoost,manacost)
        {
            SkillName = name;
            Cooldown = cooldown;
            Prefab = prefab;
            
            Manacost = manacost;
            StatBoost = statBoost;
        }

    }

     public class MovementSkill : Skill
    {

       private float travelDistance;

       public float TravelDistance
       {
        get{return travelDistance;}
        set{travelDistance = value;}
       }

        public MovementSkill(string name, float cooldown, GameObject prefab, float manacost,float travelDistance)
        :base(name, cooldown, prefab, SkillType.Movement,manacost)
        {
            SkillName = name;
            Cooldown = cooldown;
            Prefab = prefab;
            
            Manacost = manacost;
            TravelDistance = travelDistance;
        }

    }
}