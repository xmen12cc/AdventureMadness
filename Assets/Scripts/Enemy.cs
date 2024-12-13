using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 20;
    public float hp = 30f;

    private Transform target;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    private bool isPatrolling = true;
    private Vector2 patrolStartPoint;
    private Vector2 patrolEndPoint;
    private float patrolRange = 5f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        patrolStartPoint = transform.position;
        patrolEndPoint = new Vector2(patrolStartPoint.x + patrolRange, patrolStartPoint.y);
    }

    void Update()
    {
        if (target != null && Vector2.Distance(transform.position, target.position) < 10f)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            isPatrolling = false;
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (transform.position.x <= patrolStartPoint.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolEndPoint, speed * Time.deltaTime);
        }
        else if (transform.position.x >= patrolEndPoint.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolStartPoint, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            GameManager.Instance.TakeDamage(damage);
            lastAttackTime = Time.time;
            AudioManager.Instance.PlaySound(AudioManager.Instance.enemyHitSound);
            Debug.Log("Enemy attacked player!");
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"Enemy took {damage} damage! Remaining HP: {hp}");

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        GameManager.Instance.AddXp(20);
        GameManager.Instance.UpdatePlayerStats();
        Destroy(gameObject);
        AudioManager.Instance.PlaySound(AudioManager.Instance.enemyDeathSound);
    }
}
