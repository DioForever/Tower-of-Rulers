using UnityEngine;
using Skills;

public class HealthReplenishScript : MonoBehaviour
{
 
    private float healthboost;
    private SkillManager mySkillManager;
    private playerControl playercontrol;

    private void Start()
    { 
        // kde v listu je dany spell
        StatBoostSkill StatBoostskill = mySkillManager.skills[2] as StatBoostSkill;

       
       
       healthboost = StatBoostskill.StatBoost;
       playercontrol.mana = playercontrol.health + 100.0f * healthboost;
    }
}