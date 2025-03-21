using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player16 player = collision.gameObject.GetComponent<Player16>();
            player.kbFromRight = !player.facingLeft;
            StartCoroutine(player.DelayLose());
        }
        
        if (collision.gameObject.CompareTag("Head"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.kbFromRight = !enemy.facingLeft;
            enemy.EnemyDie();
        }
    }
}
