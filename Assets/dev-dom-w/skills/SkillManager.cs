using System.Collections.Generic;
using UnityEngine;
using Skills;

public class SkillManager : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>();

    private void Awake()
    {
        InitializeSkills();
    }

    private void InitializeSkills()
    {
        skills.Add(new StatBoostSkill("ManaReplenish", 25.0f, null,20.0f, 0.5f));
        skills.Add(new StatBoostSkill("DamageBoost", 30.0f, null, 20.0f, 0.3f));
        skills.Add(new StatBoostSkill("HealthReplenish", 40.0f, null, 20.0f, 0.25f));

        skills.Add(new MovementSkill("Flash", 15.0f, null, 20.0f, 15.0f));
        skills.Add(new MovementSkill("Dash", 10.0f, null, 20.0f, 40.0f));
    }
}