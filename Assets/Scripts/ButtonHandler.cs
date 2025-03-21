using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float dir;
    private Player16 player => FindObjectOfType<Player16>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.SetMoveDir(dir);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.SetMoveDir(0f);
    }
}
