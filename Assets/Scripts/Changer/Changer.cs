using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changer : MonoBehaviour
{
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLock;
    public int[] mass;
    public int iter = 0;
    public int val;
    public bool locked = false;

    public bool haveText;
    public string text_name;
    public string[] text_change;
    public TMPro.TextMeshPro text;

    public void Lock()
    {
        locked = true;
        transform.GetComponent<SpriteRenderer>().sprite = spriteLock[iter];
    }

    public void Unlock()
    {
        locked = false;
        transform.GetComponent<SpriteRenderer>().sprite = spriteUp[iter];
    }

    void Start()
    {
        val = mass[iter];
        transform.GetComponent<SpriteRenderer>().sprite = spriteUp[iter];
        if (haveText)
        {
            text.text = text_change[iter];
        }
    }

    public void OnMouseDownChan()
    {
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteDown[iter];
        }
    }

    public void OnMouseUpChan()
    {
        if (!locked)
        {
            iter = (iter + 1) % spriteUp.Length;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteUp[iter];
            val = mass[iter];
            if (haveText)
            {
                text.text = text_change[iter];
            }
        }
    }

    void Update()
    {

    }
}
