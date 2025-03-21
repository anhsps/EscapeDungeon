using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;

    public bool facingLeft;
    public bool done;

    protected void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    protected IEnumerator KnockBack(Rigidbody2D rb, float duration, float x, float y)
    {
        Vector2 from = rb.position;
        Vector2 to = from + new Vector2(x, 0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float newX = Mathf.Lerp(from.x, to.x, t);
            float newY = Mathf.Lerp(from.y, from.y + y, t * (1 - t) * 6);

            rb.position = new Vector2(newX, newY);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    protected void HandlerDie(bool state, int index)
    {
        SoundManager16.Instance.PlaySound(index);
        done = state;
        animator.Rebind();
        if (state) animator.SetTrigger("Die");
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D item in cols)
            item.enabled = !state;
    }
}
