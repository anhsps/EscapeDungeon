using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player16 : Character
{
    private float horiInput;
    private float moveInput;
    private float moveDir;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float moveSP = 7f;

    [Header("KnockBack")]
    [SerializeField] private float kbForce = 3f;
    [SerializeField] private float kbDuration = 0.4f;
    [HideInInspector] public bool kbFromRight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;

        if (!IsOnScreen(transform.position)) StartCoroutine(DelayLose());

        horiInput = Input.GetAxisRaw("Horizontal");
        moveInput = (moveDir != 0) ? moveDir : horiInput;

        rb.velocity = new Vector2(moveInput * moveSP, rb.velocity.y);

        if (CheckWall()) rb.velocity = new Vector2(0, rb.velocity.y);

        animator.SetBool("Run", moveInput != 0);

        if (moveInput > 0 && facingLeft || moveInput < 0 && !facingLeft)
            Flip();
    }

    public void SetMoveDir(float dir) => moveDir = dir;

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    private bool CheckWall()
    => Physics2D.OverlapPoint(wallCheck.position, LayerMask.GetMask("Ground", "Wall"));

    private IEnumerator Winner()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Win");
        GameManager16.Instance.GameWin();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            SoundManager16.Instance.PlaySound(5);
            GameManager16.Instance.IncreaseScore(1);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("Open");
            if (IsGrounded())
            {
                collision.GetComponent<Collider2D>().enabled = false;
                animator.Rebind();
                rb.velocity = Vector2.zero;
                StartCoroutine(Winner());
                done = true;
            }
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            collision.GetComponent<Collider2D>().enabled = false;
            kbFromRight = !facingLeft;
            StartCoroutine(DelayLose());
        }
    }

    public IEnumerator DelayLose()
    {
        HandlerDie(true, 4);

        float x = kbForce * (kbFromRight ? -1 : 1);
        yield return KnockBack(rb, kbDuration, x, kbForce / 3);

        yield return new WaitForSeconds(1f);
        GameManager16.Instance.GameLose();
    }

    private bool IsOnScreen(Vector3 pos)
    {
        // convert pos: world space -> viewpost space
        Vector3 screenPos = Camera.main.WorldToViewportPoint(pos);
        // check pos vs range [0,1] trong Viewpost space
        return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
    }

    public void Revive()
    {
        HandlerDie(false, -1);
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 5.5f, 0);
    }
}
