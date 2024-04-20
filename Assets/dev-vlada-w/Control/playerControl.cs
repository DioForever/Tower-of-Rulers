using System.Collections;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector2 moveDirection;

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

    private Animator animator;
    private Weapon weapon;

    private void Start()
    {
        canDash = true;

        // Get references to Animator and Weapon components
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        ProcessInputs();

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Attack();
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

    void Attack()
    {
        animator.SetTrigger("Attack");
        if (weapon != null)
        {
            weapon.PerformAttack();
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        MonsterBehavior monsterbehavior = other.GetComponent<MonsterBehavior>();
        if (monsterbehavior != null && weapon != null)
        {
            monsterbehavior.TakeDamage(weapon.damageAmount);
        }
    }
}
