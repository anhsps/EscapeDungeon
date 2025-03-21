using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Head"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.kbFromRight = (collision.transform.position.x <= transform.position.x);
            enemy.EnemyDie();
        }
    }
}
