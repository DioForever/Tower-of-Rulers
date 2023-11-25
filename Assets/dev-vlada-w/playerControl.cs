using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerControl : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    //public StatModifier speedModifier;
    
//speed
    //public delegate void MoveSpeedChanged(); //ostatní skripty reagují na změnu moveSpeedu
    //public event MoveSpeedChanged OnMoveSpeedChanged; //event pro speed
//Hp
    public delegate void HpChanged();
    public event HpChanged OnHpChanged; 
//mana
    public delegate void ManaChanged(); 
    public event ManaChanged OnManaChanged; 

    [Header("Hp Settings")]
    [SerializeField] float Hp = 100f;
    [Header("Mana Settings")]
    [SerializeField] float Mana = 100f;
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCooldown = 3f;
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

    // void Start()
    // {
    //     GameObject gunObject = Instantiate(Resources.Load("GunPrefab") as GameObject, transform.position, Quaternion.identity);
    //     currentWeapon = gunObject.GetComponent<Weapon>();
    //     currentWeapon.isPickedUp = true;
    //     availableWeapons.Add(currentWeapon);
    // }


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
    //  public void UpdateMoveSpeed(float newMoveSpeed) //funkce na updatnutí moveSpeedu
    // {
    //    speedModifier.UpdateMoveSpeed(newMoveSpeed);
    //     OnMoveSpeedChanged?.Invoke();
    // }
    // public void UpdateHp(float newHp)
    // {
    //    speedModifier.UpdateHp(newHp);
    //     OnHpChanged?.Invoke();
    // }
    // public void UpdateMana(float newMana) //funkce na updatnutí moveSpeedu
    // {
    //    speedModifier.UpdateMana(newMana);
    //     OnManaChanged?.Invoke();
    // }
}




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class playerControl : MonoBehaviour
// {
//     public float moveSpeed;
//     public Rigidbody2D rb;
//     private Vector2 moveDirection;
//     public bow bow;
//     private weapon currentWeapon;
//     public List<weapon> availableWeapons = new List<weapon>();

//     //speed
//     public delegate void MoveSpeedChanged();
//     public event MoveSpeedChanged OnMoveSpeedChanged;

//     //Hp
//     public delegate void HpChanged();
//     public event HpChanged OnHpChanged;

//     //mana
//     public delegate void ManaChanged();
//     public event ManaChanged OnManaChanged;

//     [Header("Hp Settings")]
//     [SerializeField] float Hp = 100f;
//     [Header("Mana Settings")]
//     [SerializeField] float Mana = 100f;
//     [Header("Dash Settings")]
//     [SerializeField] float dashSpeed = 5f;
//     [SerializeField] float dashDuration = 0.25f;
//     [SerializeField] float dashCooldown = 3f;
//     bool isDashing;
//     bool canDash = true;

//     void Start()
//     {
//         canDash = true;

//         GameObject gunObject = Instantiate(Resources.Load("GunPrefab") as GameObject, transform.position, Quaternion.identity);
//         currentWeapon = gunObject.GetComponent<weapon>();
//         currentWeapon.isPickedUp = true;
//         availableWeapons.Add(currentWeapon);
//     }

//     void Update()
//     {
//         if (isDashing)
//         {
//             return;
//         }

//         ProcessInputs();
//         if (Input.GetKeyDown(KeyCode.Space) && canDash)
//         {
//             StartCoroutine(Dash());
//         }

//         // Weapon pickup
//         if (Input.GetKeyDown(KeyCode.E))
//         {
//             PickUpWeapon();
//         }

//         // Weapon switching
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             SwitchWeapon(0);
//         }
//         else if (Input.GetKeyDown(KeyCode.Alpha2))
//         {
//             SwitchWeapon(1);
//         }
//     }

//     void FixedUpdate()
//     {
//         if (isDashing)
//         {
//             return;
//         }

//         Move();
//     }

//     void ProcessInputs()
//     {
//         float moveX = Input.GetAxisRaw("Horizontal");
//         float moveY = Input.GetAxisRaw("Vertical");
//         moveDirection = new Vector2(moveX, moveY).normalized;
//     }

//     void Move()
//     {
//         rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
//     }

//     void PickUpWeapon()
//     {
//         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

//         foreach (Collider2D collider in colliders)
//         {
//             weapon pickedUpWeapon = collider.GetComponent<weapon>();

//             if (pickedUpWeapon != null && !pickedUpWeapon.isPickedUp)
//             {
//                 if (availableWeapons.Count < 2)
//                 {
//                     pickedUpWeapon.isPickedUp = true;
//                     availableWeapons.Add(pickedUpWeapon);
//                     collider.gameObject.SetActive(false);
//                 }
//                 else
//                 {
//                 }
//             }
//         }
//     }

//     void SwitchWeapon(int index)
//     {
//         if (index >= 0 && index < availableWeapons.Count)
//         {
//             currentWeapon = availableWeapons[index];

//             currentWeapon.gameObject.SetActive(true);
//         }
//     }

//     private IEnumerator Dash()
//     {
//         canDash = false;
//         isDashing = true;
//         rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
//         yield return new WaitForSeconds(dashDuration);
//         isDashing = false;
//         yield return new WaitForSeconds(dashCooldown);
//         canDash = true;
//     }
// }
