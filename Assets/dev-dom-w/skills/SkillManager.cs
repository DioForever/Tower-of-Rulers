using System.Collections.Generic;
using UnityEngine;
using Skill;

public class SkillManager : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>();

    private void Awake()
    {
        InitializeSkills();
    }

    private void InitializeSkills()
    {
        skills.add(new StatBoostSkill("ManaReplenish", 25.0f,  null, 20.0f,0.5f))
        skills.add(new StatBoostSkill("DamageBoost", 30.0f, null, 20.0f,0.3f))
        skills.add(new StatBoostSkill("HealthReplenish", 40.0f, null, 20.0f,0.25f))


        skills.add(new MovementSkill("Flash", 15.0f, null, 20.0f,15.0f))
        skills.add(new MovementSkill("Dash", 10.0f, null, 20.0f,40.0f))
    }
}