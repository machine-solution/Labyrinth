using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNumber : InputText
{
    public new string GetChar()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        { return "0"; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { return "1"; }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        { return "2"; }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        { return "3"; }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        { return "4"; }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        { return "5"; }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        { return "6"; }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        { return "7"; }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        { return "8"; }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        { return "9"; }
        if (Input.GetKeyDown(KeyCode.Return))
            return "return";
        if (Input.GetKeyDown(KeyCode.Backspace))
            return "delete";
        if (Input.GetKeyDown(KeyCode.Space))
            return " ";
        return "";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
