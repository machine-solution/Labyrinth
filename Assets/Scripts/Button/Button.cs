using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Sprite spriteUp, spriteDown, spriteLock;
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

    void Update()
    {

    }
}
