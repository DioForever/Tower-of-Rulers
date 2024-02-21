using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDirection;

    public StatModifier speedModifier;

    public IWeapon meleeWeapon;
    public IWeapon bow;    
    public IWeapon currentWeapon;

    public delegate void MoveSpeedChanged();
    public event MoveSpeedChanged OnMoveSpeedChanged;

    public delegate void HpChanged(float newHp);
    public event HpChanged OnHpChanged;

    public delegate void ManaChanged(float newMana);
    public event ManaChanged OnManaChanged;

    [Header("Hp Settings")]
    [SerializeField] public float health = 100f;

    [Header("Mana Settings")]
    [SerializeField] public float mana = 100f;

    [Header("Dash Settings")]
    [SerializeField] public float dashSpeed = 5f;
    [SerializeField] public float dashDuration = 0.25f;
    [SerializeField] public float dashCooldown = 3f;

    public bool isDashing;
    public bool canDash = true;

    private void Start()
    {
        canDash = true;
        meleeWeapon = new MeleeWeapon();
        bow = new Bow();
        currentWeapon = meleeWeapon;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        ProcessInputs();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Switch to melee weapon
            currentWeapon = meleeWeapon;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Switch to bow
            currentWeapon = bow;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Attack with current weapon
            currentWeapon.Attack();

            if (canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
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
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        GetComponent<Rigidbody2D>().velocity = moveDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void UpdateMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
        OnMoveSpeedChanged?.Invoke();
    }

    public void UpdateHp(float newHp)
    {
        health = newHp;
        OnHpChanged?.Invoke(newHp);
    }

    public void UpdateMana(float newMana)
    {
        mana = newMana;
        OnManaChanged?.Invoke(newMana);
    }

}
