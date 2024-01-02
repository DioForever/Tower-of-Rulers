using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class StatModifier : MonoBehaviour
{
    private PlayerControl playerController; 

    private FloorSetting FloorSetting;

    //private monsterStats monsterStats;
    private float originalMoveSpeed;
    
    private float originalHealth;
    
    private float originalStrength;
    
    private float originalMana;

    public bool BossIsDead = false;

    void Start()
    {
        OriginalValues();
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (BossIsDead)
        {
            RevertAllToOriginal();
        }
        else
        {
            switch (FloorSetting.Floornumber)
            {
                case int n when n >= 1 && n <= 10:

                ApplySpeedDebuff(FloorSetting.SpeedDebuffValues[0]); // 50% rychlosti
                ApplyHealthDebuff(FloorSetting.HealthDebuffValues[2]); // 2x života

                break;

                case int n when n >= 11 && n <= 20:

                ApplyHealthDebuff(FloorSetting.HealthDebuffValues[0]); // 80% života
                ApplyStrengthDebuff(FloorSetting.StrengthDebuffValues[2]); // 5x síly

                break;

                case int n when n >= 21 && n <= 30:

                ApplyStrengthDebuff(FloorSetting.StrengthDebuffValues[0]); //125% síly
                ApplyManaDebuff(FloorSetting.ManaDebuffValues[2]); //5x many

                break;

                case int n when n >= 31 && n <= 40:

                horizontal = -horizontal;
                vertical = -vertical;

                ApplyHealthDebuff(FloorSetting.HealthDebuffValues[0]); //80% života
                DisableMana(); //disable Mana usage

                break;

                case int n when n >= 41 && n <= 50:

                ApplySpeedDebuff(FloorSetting.SpeedDebuffValues[1]);// 2x rychlosti

                //playerController.isCasting = false;//unbind skill

                break;

                case int n when n >= 51 && n <= 60:

                ApplyManaDebuff(FloorSetting.ManaDebuffValues[0]); // 20% many
                ApplyStrengthDebuff(FloorSetting.StrengthDebuffValues[2]); // 5x síly

                break;

                case int n when n >= 61 && n <= 70:

                ApplyHealthDebuff(FloorSetting.HealthDebuffValues[1]); // 50% života
                
                //playerController.canDash = false; //unbind dash

                break;

                case int n when n >= 71 && n <= 80:

                ApplyStrengthDebuff(FloorSetting.StrengthDebuffValues[1]); //2x síly
                ApplyManaDebuff(FloorSetting.ManaDebuffValues[2]); // 5x many

                horizontal = -horizontal;
                vertical = -vertical;

                break;

                case int n when n >= 81 && n <= 90:

                ApplyManaDebuff(FloorSetting.ManaDebuffValues[1]); //4x many

                break;

                case int n when n >= 91 && n <= 100:

                ApplySpeedDebuff(FloorSetting.SpeedDebuffValues[0]); // 50% ryhclosti
                ApplyHealthDebuff(FloorSetting.HealthDebuffValues[0]); // 80% života
                ApplyManaDebuff(FloorSetting.ManaDebuffValues[0]); // 20% many

                break;
            }
            
        }
    }

    // move speed debuffy
    void RevertToOriginalSpeed()
    {
        playerController.moveSpeed = originalMoveSpeed;
    }

    

    public void UpdateMoveSpeed(float newMoveSpeed) //event na změnění debuffu u změny moveSpeedu
    {
        RevertToOriginalSpeed();
        originalMoveSpeed = newMoveSpeed;   
    }

    void ApplySpeedDebuff(float speedDebuffValue)
    {
        playerController.moveSpeed = originalMoveSpeed / speedDebuffValue;
        //monsterStats.moveSpeed = monsterStats.moveSpeed / speedDebuffValue;
    }

   
    // health debuffy
    void RevertToOriginalHealth()
    {
        playerController.health = originalHealth;
    }

    public void UpdateHp(float newHp) //event na změnění debuffu u změny života
    {
        RevertToOriginalHealth();
        originalHealth = newHp;
    }

    void ApplyHealthDebuff(float healthDebuffValue)
    {
        playerController.health = originalHealth / healthDebuffValue;
        //monsterStats.health = monsterStats.health / healthDebuffValue;
    }

    // strenght debuffy

    void RevertToOriginalStrength()
    {
       // playerController.strength = originalStrength;
    }

    void ApplyStrengthDebuff(float strengthDebuffValue)
    {
        //playerController.strength = originalStrength / strengthDebuffValue;
        //monsterStats.strength = monsterStats.strength / strengthDebuffValue;
    }

    public void UpdateStrength(float newStrength) //event na změnění debuffu u změny Síly
    {
        RevertToOriginalStrength();
        originalStrength = newStrength;   
    }

    // mana debuffy

    void RevertToOriginalMana()
    {
        playerController.mana = originalMana;
    }

    void ApplyManaDebuff(float manaDebuffValue)
    {
        playerController.mana = originalMana / manaDebuffValue;
        //monsterStats.mana = monsterStats.mana / manaDebuffValue;
    }

    void DisableMana()
    {
        playerController.mana = 0;
        //monsterStats.mana = 0;
    }

    public void UpdateMana(float newMana) //event na změnění debuffu u změny Síly
    {
        RevertToOriginalMana();
        originalMana = newMana;
    }

    void OriginalValues()
    {
        //originalStrength = playerController.strenght;
        originalMana = playerController.mana; 
        originalHealth = playerController.health;
        originalMoveSpeed = playerController.moveSpeed;
    }

    void RevertAllToOriginal()
    {
        playerController.moveSpeed = originalMoveSpeed;
        playerController.mana = originalMana;
        //playerController.strength = originalStrength;
        playerController.health = originalHealth;
    }
}
