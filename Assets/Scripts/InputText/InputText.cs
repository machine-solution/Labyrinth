using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputText : MonoBehaviour
{
    public string text;
    public int limit;
    int id;
    static int idgen = 0;
    static public int act_id = -1;
    TMPro.TextMeshPro picture;
#if UNITY_ANDROID
    static TouchScreenKeyboard keyboard;
#endif


    public string GetChar()
    {
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (Input.GetKeyDown(KeyCode.A))
        { if (shift) return "A"; return "a"; }
        if (Input.GetKeyDown(KeyCode.B))
        { if (shift) return "B"; return "b"; }
        if (Input.GetKeyDown(KeyCode.C))
        { if (shift) return "C"; return "c"; }
        if (Input.GetKeyDown(KeyCode.D))
        { if (shift) return "D"; return "d"; }
        if (Input.GetKeyDown(KeyCode.E))
        { if (shift) return "E"; return "e"; }
        if (Input.GetKeyDown(KeyCode.F))
        { if (shift) return "F"; return "f"; }
        if (Input.GetKeyDown(KeyCode.G))
        { if (shift) return "G"; return "g"; }
        if (Input.GetKeyDown(KeyCode.H))
        { if (shift) return "H"; return "h"; }
        if (Input.GetKeyDown(KeyCode.I))
        { if (shift) return "I"; return "i"; }
        if (Input.GetKeyDown(KeyCode.J))
        { if (shift) return "J"; return "j"; }
        if (Input.GetKeyDown(KeyCode.K))
        { if (shift) return "K"; return "k"; }
        if (Input.GetKeyDown(KeyCode.L))
        { if (shift) return "L"; return "l"; }
        if (Input.GetKeyDown(KeyCode.M))
        { if (shift) return "M"; return "m"; }
        if (Input.GetKeyDown(KeyCode.N))
        { if (shift) return "N"; return "n"; }
        if (Input.GetKeyDown(KeyCode.O))
        { if (shift) return "O"; return "o"; }
        if (Input.GetKeyDown(KeyCode.P))
        { if (shift) return "P"; return "p"; }
        if (Input.GetKeyDown(KeyCode.Q))
        { if (shift) return "Q"; return "q"; }
        if (Input.GetKeyDown(KeyCode.R))
        { if (shift) return "R"; return "r"; }
        if (Input.GetKeyDown(KeyCode.S))
        { if (shift) return "S"; return "s"; }
        if (Input.GetKeyDown(KeyCode.T))
        { if (shift) return "T"; return "t"; }
        if (Input.GetKeyDown(KeyCode.U))
        { if (shift) return "U"; return "u"; }
        if (Input.GetKeyDown(KeyCode.V))
        { if (shift) return "V"; return "v"; }
        if (Input.GetKeyDown(KeyCode.W))
        { if (shift) return "W"; return "w"; }
        if (Input.GetKeyDown(KeyCode.X))
        { if (shift) return "X"; return "x"; }
        if (Input.GetKeyDown(KeyCode.Y))
        { if (shift) return "Y"; return "y"; }
        if (Input.GetKeyDown(KeyCode.Z))
        { if (shift) return "Z"; return "z"; }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        { if (shift) return ")"; return "0"; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { if (shift) return "!"; return "1"; }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        { if (shift) return "@"; return "2"; }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        { if (shift) return "#"; return "3"; }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        { if (shift) return "$"; return "4"; }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        { if (shift) return "%"; return "5"; }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        { if (shift) return "^"; return "6"; }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        { if (shift) return "&"; return "7"; }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        { if (shift) return "*"; return "8"; }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        { if (shift) return "("; return "9"; }
        if (Input.GetKeyDown(KeyCode.Return))
            return "return";
        if (Input.GetKeyDown(KeyCode.Backspace))
            return "delete";
        if (Input.GetKeyDown(KeyCode.Space))
            return " ";
        return "";
    }

    void Start()
    {
        id = idgen++;
        picture = GetComponent<TMPro.TextMeshPro>();
        picture.text = text;
    }

    public void OnMouseUpInput()
    {
        act_id = id;
#if UNITY_ANDROID
        keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default, false, false,false,false,"InputField",limit);
#endif
    }

    string RedactStr(string s, string c)
    {
        if (c == "delete")
        {
            s = s.Substring(0, s.Length - 1);
        }
        else if (c == "return")
        {
            act_id++;
        }
        else
        {
            s += c;
        }
        return s;
    }


    void Update()
    {
        if (id == act_id)
        {
#if UNITY_STANDALONE
            text = RedactStr(text, GetChar());
            if (text.Length>limit)
                text = text.Substring(0, limit);
#endif
#if UNITY_ANDROID
            text = keyboard.text;
#endif
            picture.text = text;
        }
    }
}
