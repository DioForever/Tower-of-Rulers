using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    public playerControl playerController; 
    private float[] speedDebuffValues = {2.0f,0.5f};
    private float[] healthDebuffValues = {0.8f, 0.5f}
    private float[] strengthDebuffValues = {0.8f,0.5f}
    private float[] manaDebuffValues = {0.2f,4.0f}
    public float floornumber = 1;
    private float originalMoveSpeed;
    private float originalHealth;
    private float originalStrenght;
    private float originalMana;
    public bool BossIsDead = false;

    
    void Start()
    {
        originalStrenght = playerController.strenght;
        originalMana = playerController.mana; 
        originalHealth = playerController.health;
        originalMoveSpeed = playerController.moveSpeed;
    }

   

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        if(BossIsDead == false && floornumber >= 1 && floornumber <= 10)
        {
            
            ApplySpeedDebuff(speedDebuffValues[0]);
        }

        else if(BossIsDead == false && floornumber >= 11 && floornumber <= 20)
        {
            
            ApplyHealthDebuff(healthDebuffValues[0]);
        }

        else if(BossIsDead == false && floornumber >= 21 && floornumber <= 30)
        {
            
            ApplyStrengthDebuff(strengthDebuffValues[0]);
        }

        else if(BossIsDead == false && floornumber >= 31 && floornumber <= 40)
        {
            horizontal = -horizontal;
            vertical = -vertical;

            ApplyHealthDebuff(healthDebuffValues[0]);
        }

        else if(BossIsDead == false && floornumber >= 41 && floornumber <= 50)
        {
            ApplySpeedDebuff(speedDebuffValues[1]);
        }

        else if(BossIsDead == false && floornumber >= 51 && floornumber <= 60)
        {
            ApplyManaDebuff(manaDebuffValues[0]);
        }

        else if(BossIsDead == false && floornumber >= 61 && floornumber <=70)
        {
            ApplyHealthDebuff(healthDebuffValues[1]);
        }

        else if(BossIsDead == false && floornumber >= 71 && floornumber <= 80)
        {
            ApplyStrengthDebuff(strengthDebuffValues[1]);
        }

        else if(BossIsDead == false && floornumber >= 81 && floornumber <= 90)
        {
            ApplyManaDebuff(manaDebuffValues[1]);
        }

        else if(BossIsDead == false && floornumber >= 91 && floornumber <= 100)
        {
            ApplySpeedDebuff(speedDebuffValues[0]);
            ApplyHealthDebuff(healthDebuffValues[0]);
            ApplyManaDebuff(manaDebuffValues[0]);
        }
       
        else
        {
            RevertToOriginalSpeed();
            RevertToOriginalHealth();
            RevertToOriginalMana();
            RevertToOriginalStrength();
        }

        Vector2 move = new Vector2(horizontal, vertical);
        rb.velocity = move * playerController.moveSpeed; 
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        Vector2 movement = moveDirection * playerController.moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    // move speed debuffy
    void RevertToOriginalSpeed()
    {
        playerController.moveSpeed = originalMoveSpeed;
    }

    private void OnEnable()
    {
        playerController.OnMoveSpeedChanged += ApplySpeedDebuff;
    }

    private void OnDisable()
    {
        playerController.OnMoveSpeedChanged -= ApplySpeedDebuff;
    }

    public void UpdateMoveSpeed(float newMoveSpeed) //event na změnění debuffu u změny moveSpeedu
    {
        RevertToOriginalSpeed();
        originalMoveSpeed = newMoveSpeed;
        ApplySpeedDebuff();
    }

    void ApplySpeedDebuff(float speedDebuffValue)
    {
        playerController.moveSpeed = originalMoveSpeed / speedDebuffValue;
    }

   
    // health debuffy
    void RevertToOriginalHealth()
    {
        
        playerController.health = originalHealth;
    }

    private void OnEnable()
    {
        playerController.OnHealthChanged += ApplyHealthDebuff;
    }

    private void OnDisable()
    {
        playerController.OnHealthChanged -= ApplyHealthDebuff;
    }

    public void UpdateHealth(float newHealth) //event na změnění debuffu u změny života
    {
        RevertToOriginalHealth();
        originalHealth = newHealth;
        ApplyHealthDebuff();
    }

    void ApplyHealthDebuff(float healthDebuffValue)
    {
        playerController.health = originalHealth * healthDebuffValue;
    }

    // strenght debuffy

    void RevertToOriginalStrength()
    {
        playerController.strength = originalStrength;
    }

    void ApplyStrengthDebuff(float strengthDebuffValue)
    {
        playerController.strength = originalStrength / strengthDebuffValue;
    }

     private void OnEnable()
    {
        playerController.OnStrengthChanged += ApplyStrengthDebuff;
    }

    private void OnDisable()
    {
        playerController.OnStrengthChanged -= ApplyStrengthDebuff;
    }

    public void UpdateStrength(float newStrength) //event na změnění debuffu u změny Síly
    {
        RevertToOriginalStrength();
        originalStrenght = newStrength;
        ApplyStrengthDebuff();
    }

    // mana debuffy

    void RevertToOriginalMana()
    {
        playerController.mana = originalMana;
    }

    void ApplyManaDebuff(float manaDebuffValue)
    {
        playerController.mana = originalMana * manaDebuffValue;
    }

     private void OnEnable()
    {
        playerController.OnManaChanged += ApplyManaDebuff;
    }

    private void OnDisable()
    {
        playerController.OnManaChanged -= ApplyManaDebuff;
    }

    public void UpdateMana(float newMana) //event na změnění debuffu u změny Síly
    {
        RevertToOriginalMana();
        originalMana = newMana;
        ApplyManaDebuff();
    }
}