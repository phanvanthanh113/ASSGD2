using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
            Debug.LogError("Lỗi: Không tìm thấy Rigidbody2D trên " + gameObject.name);

        if (groundCheck == null)
            Debug.LogError("Lỗi: Chưa gán GroundCheck!");
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D hoặc ←/→
        float moveY = Input.GetAxis("Vertical");   // W/S hoặc ↑/↓

        // Di chuyển 4 hướng
        Vector3 movement = new Vector3(moveX, moveY, 0);
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Animation đi bộ (nếu đang di chuyển)
        animator.SetBool("isWalking", movement.magnitude > 0.01f);

        // Quay mặt nhân vật trái/phải
        if (moveX > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveX < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Chém khi giữ Space
        animator.SetBool("isAttacking", Input.GetKey(KeyCode.Space));

        // Kiểm tra mặt đất (chỉ để nhảy nếu muốn giữ lại)
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Nhảy (nếu giữ lại tính năng này)
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }
         private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem") || collision.CompareTag("Cherry"))
        {
            // Nhặt đồ vật
            Destroy(collision.gameObject); // Xóa đối tượng
            // Bạn có thể thêm logic để tăng điểm số ở đây
            Debug.Log("Nhặt: " + collision.gameObject.name);
        }
        
    }
}
