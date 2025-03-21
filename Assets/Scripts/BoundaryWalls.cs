using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryWalls : MonoBehaviour
{
    [SerializeField] private GameObject leftPrefab, rightPrefab;
    [SerializeField] private GameObject gaiPrefab;

    // Start is called before the first frame update
    void Start()
    {
        float leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float rightEdge = Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x;
        float upEdge = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y - 0.5f;

        Instantiate(leftPrefab, new Vector3(leftEdge, 0, 0), Quaternion.identity, transform);
        Instantiate(rightPrefab, new Vector3(rightEdge, 0, 0), Quaternion.identity, transform);
        Instantiate(gaiPrefab, new Vector3(0, upEdge, 0), Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
