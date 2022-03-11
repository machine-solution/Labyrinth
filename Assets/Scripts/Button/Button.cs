using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Button : MonoBehaviour
{
    public Sprite spriteUp, spriteDown, spriteLock;
    public KeyCode hotkey;
    public bool locked;
    public bool haveConfirm = false;
    //_____________________________
    public VoidFunc click;
    //_____________________________
    public VoidFunc_Int click_int;
    public int arg_int;

    void Start()
    {

    }

    public void Block()
    {
        locked = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteLock;
    }

    public void Unlock()
    {
        locked = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteUp;
    }

    public void OnMouseDownBut()
    {
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteDown;
        }
    }

    public void OnMouseUpBut()
    {
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteUp;
            if (!haveConfirm)
            {
                click?.Invoke();
                click_int?.Invoke(arg_int);
            }
            else
            {
                GameObject confirm = Instantiate(Resources.Load<GameObject>("confirm"));
                confirm.GetComponent<Confirm>().click = click;
                confirm.GetComponent<Confirm>().click_int = click_int;
                confirm.GetComponent<Confirm>().arg_int = arg_int;
            }
        }
    }

    bool Overshaded()
    {
        int maxSize = 228;
        Vector2 ray = gameObject.transform.position;
        RaycastHit2D[] hit = new RaycastHit2D[maxSize];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = false;

        int cnt = Physics2D.Raycast(ray, Vector2.zero, filter, hit);
        cnt = Min(cnt, maxSize);

        int maxLayer = 0; //default
        for (int i = 0; i < cnt; ++i)
        {
            maxLayer = Max(maxLayer, hit[i].transform.gameObject.layer);
        }

        for (int i = 0; i < cnt; ++i)
        {
            if (hit[i].transform.gameObject == gameObject &&
                maxLayer == hit[i].transform.gameObject.layer)
                return false;
        }

        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(hotkey))
            if (!Overshaded())
                OnMouseDownBut();
        if (Input.GetKeyUp(hotkey))
            if (!Overshaded())
                OnMouseUpBut();
    }
}
