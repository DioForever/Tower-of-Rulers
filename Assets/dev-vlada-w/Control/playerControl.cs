using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector2 moveDirection;

    public StatModifier speedModifier;

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
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        ProcessInputs();

        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            ApplyDamage();
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

    void ApplyDamage()
{
    Vector2 currentPosition = transform.position;
    Vector2 damageDirection = moveDirection.normalized * 10f; // 10 pixels in the last moved direction
    RaycastHit2D hit = Physics2D.Raycast(currentPosition, damageDirection);

    if (hit.collider != null)
    {
        CactusEnemy cactus = hit.collider.GetComponent<CactusEnemy>();
        if (cactus != null)
        {
            cactus.TakeDamage(10); // Assuming 10 damage for now, you can change it as needed
        }
    }
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
