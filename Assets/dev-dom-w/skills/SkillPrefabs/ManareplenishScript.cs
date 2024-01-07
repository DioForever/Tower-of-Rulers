using UnityEngine;
using Skills;

public class ManaReplenishScript : MonoBehaviour
{
 
    private float manaboost;
    private SkillManager mySkillManager;
    private PlayerControl playercontrol;

    private void Start()
    { 
        // kde v listu je dany spell
        StatBoostSkill StatBoostskill = mySkillManager.skills[0] as StatBoostSkill;

       
       
       manaboost = StatBoostskill.StatBoost;
       playercontrol.mana = playercontrol.mana + 100.0f * manaboost;
      
       
        
    }
}