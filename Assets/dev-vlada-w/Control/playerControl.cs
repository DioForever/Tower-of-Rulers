using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigidbody2D;
    private Vector2 moveDirection;
    public StatModifier speedModifier;
    // private Inventory inventory;
    
//speed
    public delegate void MoveSpeedChanged(); //ostatní skripty reagují na změnu moveSpeedu
    public event MoveSpeedChanged OnMoveSpeedChanged; //event pro speed
//Hp
    public delegate void HpChanged();
    public event HpChanged OnHpChanged; 
//mana
    public delegate void ManaChanged(); 
    public event ManaChanged OnManaChanged; 

    [Header("Hp Settings")]
    [SerializeField] public float health = 100f;
    [Header("Mana Settings")]
    [SerializeField] public float mana = 100f;
    [Header("Dash Settings")]
    [SerializeField] public float dashSpeed = 5f;
    [SerializeField] public float dashDuration = 0.25f;
    [SerializeField] public float dashCooldown = 3f;
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
        rigidbody2D.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rigidbody2D.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
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
    public void UpdateHp(float newHp)
    {
       //speedModifier.UpdateHp(newHp);
        OnHpChanged?.Invoke();
    }
    public void UpdateMana(float newMana)
    {
       speedModifier.UpdateMana(newMana);
        OnManaChanged?.Invoke();
    }

    // private void Awake()
    // {
    //     inventory = new Inventory();
    // }

}