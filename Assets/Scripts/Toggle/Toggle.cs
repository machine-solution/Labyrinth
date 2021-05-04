using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    GameObject square, tick;
    public Sprite square_unlocked, square_locked, tick_unlocked, tick_locked;
    public bool condition, locked;

    public void Init()
    {
        square = transform.GetChild(0).gameObject;
        tick = transform.GetChild(1).gameObject;
    }

    public void Set(bool Condition, bool Locked)
    {
        if (!square)
            Init();
        condition = Condition;
        locked = Locked;
        tick.SetActive(condition);
        if (locked)
        {
            square.GetComponent<SpriteRenderer>().sprite = square_locked;
            tick.GetComponent<SpriteRenderer>().sprite = tick_locked;
        }
        else
        {
            square.GetComponent<SpriteRenderer>().sprite = square_unlocked;
            tick.GetComponent<SpriteRenderer>().sprite = tick_unlocked;
        }
    }

    void Start()
    {
        if (!square)
            Init();
    }

    public void OnMouseUpToggle()
    {
        if (!locked)
            Set(!condition, locked);
    }

    void Update()
    {
        
    }
}
