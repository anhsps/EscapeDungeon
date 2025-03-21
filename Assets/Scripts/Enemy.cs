using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float kbForce = 3f;
    [SerializeField] private float kbDuration = 0.4f;
    [HideInInspector] public bool kbFromRight;
    //[HideInInspector] public bool done;

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
        Move();
        if (!CheckGround() || CheckWall()) Flip();
    }

    private void Move()
    {
        animator.SetTrigger("Run");
        float moveDir = facingLeft ? -1 : 1;
        transform.Translate(Vector2.right * moveDir * speed * Time.deltaTime);
    }

    private bool CheckGround()
        => Physics2D.OverlapPoint(groundCheck.position, LayerMask.GetMask("Ground"));

    private bool CheckWall()
        => Physics2D.OverlapPoint(wallCheck.position, LayerMask.GetMask("Wall"));

    public void EnemyDie()
    {
        HandlerDie(true, 6);

        float x = kbForce * (kbFromRight ? -1 : 1);
        StartCoroutine(KnockBack(rb, kbDuration, x, kbForce / 3));
    }
}
