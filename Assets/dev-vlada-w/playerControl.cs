using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public SpeedModifier speedModifier;

    public delegate void MoveSpeedChanged(); //ostatní skripty reagují na změnu moveSpeedu
    public event MoveSpeedChanged OnMoveSpeedChanged; //event pro speed


    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;

    private void Start()
    {
        canDash = true;
    }

    void Update()
    {

        if(isDashing)
        {
            return;            
        }

        ProcessInputs();

        if(Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if(isDashing)
        {
            return;            
        }


        Move();
    }

    void ProcessInputs()
    {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private IEnumerator Dash()
    {

        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

     public void UpdateMoveSpeed(float newMoveSpeed) //funkce na updatnutí moveSpeedu
    {
       speedModifier.UpdateMoveSpeed(newMoveSpeed);
        OnMoveSpeedChanged?.Invoke();
    }

}