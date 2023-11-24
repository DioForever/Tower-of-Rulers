using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier : MonoBehaviour
{
    private playerControl playerController; 
    private monsterStats monsterStats;


    private float[] speedDebuffValues = {2.0f,0.5f};
    private float originalMoveSpeed;
    private float[] healthDebuffValues = {1.25f, 2.0f,0.5f};
    private float originalHealth;
    private float[] strengthDebuffValues = {0.8f,0.5f,0.2f};
    private float originalStrength;
    private float[] manaDebuffValues = {5.0f,0.25f,0.2f};
    private float originalMana;

    public float floornumber = 1;
    public bool BossIsDead = false;

    void Start()
    {
        

        OriginalValues();
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(BossIsDead == false && floornumber >= 1 && floornumber <= 10)
        {
            ApplySpeedDebuff(speedDebuffValues[0]); // 50% rychlosti
            ApplyHealthDebuff(healthDebuffValues[2]); // 2x života
        }

        else if(BossIsDead == false && floornumber >= 11 && floornumber <= 20)
        {    
            ApplyHealthDebuff(healthDebuffValues[0]); // 80% života
            ApplyStrengthDebuff(strengthDebuffValues[2]); // 5x síly
        }

        else if(BossIsDead == false && floornumber >= 21 && floornumber <= 30)
        {
            ApplyStrengthDebuff(strengthDebuffValues[0]); //125% síly
            ApplyManaDebuff(manaDebuffValues[2]); //5x many
        }

        else if(BossIsDead == false && floornumber >= 31 && floornumber <= 40)
        {
            horizontal = -horizontal;
            vertical = -vertical;

            ApplyHealthDebuff(healthDebuffValues[0]); //80% života
            DisableMana(); //disable Mana usage
        }

        else if(BossIsDead == false && floornumber >= 41 && floornumber <= 50)
        {
            ApplySpeedDebuff(speedDebuffValues[1]);// 2x rychlosti

             playerController.isCasting = false;//unbind skill
        }

        else if(BossIsDead == false && floornumber >= 51 && floornumber <= 60)
        {
            ApplyManaDebuff(manaDebuffValues[0]); // 20% many
            ApplyStrengthDebuff(strengthDebuffValues[2]); // 5x síly
        }

        else if(BossIsDead == false && floornumber >= 61 && floornumber <=70)
        {
            ApplyHealthDebuff(healthDebuffValues[1]); // 50% života
            
            playerController.isDashing = false; //unbind dash
        }

        else if(BossIsDead == false && floornumber >= 71 && floornumber <= 80)
        {
            ApplyStrengthDebuff(strengthDebuffValues[1]); //2x síly
            ApplyManaDebuff(manaDebuffValues[2]); // 5x many

            horizontal = -horizontal;
            vertical = -vertical;
        }

        else if(BossIsDead == false && floornumber >= 81 && floornumber <= 90)
        {
            ApplyManaDebuff(manaDebuffValues[1]); //4x many
        }

        else if(BossIsDead == false && floornumber >= 91 && floornumber <= 100)
        {
            ApplySpeedDebuff(speedDebuffValues[0]); // 50% ryhclosti
            ApplyHealthDebuff(healthDebuffValues[0]); // 80% života
            ApplyManaDebuff(manaDebuffValues[0]); // 20% many
        }
        else
        {
           RevertAllToOriginal();
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
        monsterStats.moveSpeed = monsterStats.moveSpeed / speedDebuffValue;
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
        monsterStats.health = monsterStats.health / healthDebuffValue;
    }

    // strenght debuffy

    void RevertToOriginalStrength()
    {
        playerController.strength = originalStrength;
    }

    void ApplyStrengthDebuff(float strengthDebuffValue)
    {
        playerController.strength = originalStrength / strengthDebuffValue;
        monsterStats.strength = monsterStats.strength / strengthDebuffValue;
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
        monsterStats.mana = monsterStats.mana / manaDebuffValue;
    }

    void DisableMana()
    {
        playerController.mana = 0;
        monsterStats.mana = 0;
    }

    public void UpdateMana(float newMana) //event na změnění debuffu u změny Síly
    {
        RevertToOriginalMana();
        originalMana = newMana;
    }

    void OriginalValues()
    {
        originalStrength = playerController.strenght;
        originalMana = playerController.mana; 
        originalHealth = playerController.health;
        originalMoveSpeed = playerController.moveSpeed;
    }

    void RevertAllToOriginal()
    {
        playerController.moveSpeed = originalMoveSpeed;
        playerController.mana = originalMana;
        playerController.strength = originalStrength;
        playerController.health = originalHealth;
    }
}