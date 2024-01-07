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
    public abstract class Skill : MonoBehaviour
    {
        private string skillName;
        private float cooldown;
        

        public SkillType skillType;

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

        
         public SkillType SkillType
        {
            get { return skillType; }
            set { skillType = value; }
        }


        public Skill(string name, float cooldown, SkillType type)
        {
            SkillName = name;
            Cooldown = cooldown;
            
            SkillType = type;
           
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

        public StatBoostSkill(string name, float cooldown, float statBoost)
        :base(name, cooldown, SkillType.StatBoost)
        {
            SkillName = name;
            Cooldown = cooldown;
            
            
            
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

        private float travelSpeed;

       public float TravelSpeed
       {
        get{return travelSpeed;}
        set{travelSpeed = value;}
       }

        public MovementSkill(string name, float cooldown,float travelDistance, float travelSpeed)
        :base(name, cooldown, SkillType.Movement)
        {
            SkillName = name;
            Cooldown = cooldown;
            
            TravelDistance = travelDistance;
            TravelSpeed = travelSpeed;
        }

       

    }
}