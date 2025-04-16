using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float patrolRange = 5f;
    public float detectionRange = 6f;
    public float attackRange = 1.5f;

    private Vector2 startPos;
    private bool movingRight = true;
    private Transform player;
    private Animator animator;
    private Vector3 originalScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPos = transform.position;
        originalScale = transform.localScale; // Ghi nhớ scale ban đầu (ví dụ: (4,4,1))
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isMoving", true);
            MoveTo(player.position);
        }
        else
        {
            animator.SetBool("isAttacking", false);
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isMoving", true);

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);

        // Lật theo hướng nhưng giữ nguyên kích thước ban đầu
        Vector3 scale = originalScale;
        scale.x *= movingRight ? 1 : -1;
        transform.localScale = scale;

        float moved = transform.position.x - startPos.x;
        if (movingRight && moved >= patrolRange)
            movingRight = false;
        else if (!movingRight && moved <= -patrolRange)
            movingRight = true;
    }

    void MoveTo(Vector2 targetPosition)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.position = newPos;

        // Lật hướng theo vị trí Player (vẫn giữ scale gốc)
        Vector3 scale = originalScale;
        scale.x *= (targetPosition.x < transform.position.x) ? -1 : 1;
        transform.localScale = scale;
    }
}
