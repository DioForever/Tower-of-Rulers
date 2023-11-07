using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    public playerControl playerController; 
    public float speedDebuff = 2.0f; // 50% rychlosti
    public float floornumber = 1;
    private float originalMoveSpeed;
    void Start()
    {
        
        originalMoveSpeed = playerController.moveSpeed;
    }

   

    void Update()
    {
        if (floornumber >= 1 && floornumber <= 10)
        {
            
            ApplySpeedDebuff();
        }
        else if (floornumber >= 11)
        {
            
            RevertToOriginalSpeed();
        }

        
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        Vector2 movement = moveDirection * playerController.moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }


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

    void ApplySpeedDebuff()
    {
        playerController.moveSpeed = originalMoveSpeed / speedDebuff;
    }
}