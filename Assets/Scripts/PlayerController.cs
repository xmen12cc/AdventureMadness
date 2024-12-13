using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public float hp = 100f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public LayerMask enemyLayer;

    public int playerLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 60;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleAnimation();
        HandleAttack();
        HandleCrouching();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput); 
            transform.localScale = scale;
        }
    }

    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            AudioManager.Instance.PlaySound(AudioManager.Instance.jumpSound);
        }
    }

    void HandleCrouching(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            rb.gravityScale = 0.5f;
            animator.SetBool("Crouch", true);
            Debug.Log("Crouching");
        }
        else{
            rb.gravityScale = 1f;
            animator.SetBool("Crouch", false);
        }
    }

    void HandleAnimation()
    {
        animator.SetBool("Jumping", rb.linearVelocity.y > 0.1f);
        animator.SetBool("Fall", rb.linearVelocity.y < -0.1f && !isGrounded);
        animator.SetBool("Running", Input.GetKey(KeyCode.LeftShift) && isGrounded);
        animator.SetBool("Walking", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);
        animator.SetBool("Standing", Mathf.Abs(rb.linearVelocity.x) <= 0.1f && isGrounded);
        animator.SetBool("Dead", hp <= 0);
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Attack");
            AudioManager.Instance.PlaySound(AudioManager.Instance.attackSound);

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right * Mathf.Sign(transform.localScale.x), attackRange, enemyLayer);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<Enemy>().TakeDamage(attackDamage);

                }
            }
        }
    }

    public void GainXP(int amount)
    {
        Debug.Log($"Gaining XP: {amount}");
        
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        playerLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        
        hp += 10; 
        attackDamage += 2;
        moveSpeed += 0.5f; 
        jumpForce += 0.5f;

        GameManager.Instance.AddLevel(1);
        GameManager.Instance.UpdatePlayerStats();
        Debug.Log($"Level Up! New Level: {playerLevel}");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public float GetHp()
    {
        return hp;
    }
    
    public int GetLevel()
    {
        return playerLevel;
    }
    
    public int GetXp()
    {
        return currentXP;
    }
}
