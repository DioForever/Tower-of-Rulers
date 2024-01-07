using UnityEngine;
using Skills;

public class DamageboostSkill : MonoBehaviour
{
 
    private float damageboost;
    private SkillManager mySkillManager;
    private PlayerControl playercontrol;

    private void Start()
    { 
        // kde v listu je dany spell
        StatBoostSkill StatBoostskill = mySkillManager.skills[1] as StatBoostSkill;

       
       
       damageboost = StatBoostskill.StatBoost;
       //potrebuju damage value
       //playercontrol.damage = playercontrol.health + 100.0f * damageboost;
    }
}