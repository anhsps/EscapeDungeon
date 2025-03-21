using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player16 player = collision.gameObject.GetComponent<Player16>();
            player.kbFromRight = (collision.transform.position.x <= transform.position.x)
                ? true : false;

            StartCoroutine(player.DelayLose());
        }
    }
}
