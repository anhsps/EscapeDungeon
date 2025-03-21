using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBlock : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] private Sprite sr4_2, sr4_3;
    [SerializeField] private Collider2D colCheck;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Changes());
        }
    }

    private IEnumerator Changes()
    {
        sr.sprite = sr4_2;
        yield return new WaitForSeconds(1f);
        sr.sprite = sr4_3;
        colCheck.enabled = true;
    }
}
