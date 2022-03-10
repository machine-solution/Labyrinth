using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyFunctions;

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

    public void OnMouseDownChanger()
    {
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteDown[iter];
        }
    }

    /// <summary>
    /// change state of changer 'scroll' times
    /// </summary>
    /// <param name="scroll"></param>
    public void OnMouseUpChanger(int scroll)
    {
        if (!locked)
        {
            iter = TrueMod(iter + scroll, spriteUp.Length);
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
