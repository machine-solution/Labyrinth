using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    [System.Obsolete]
    void ToMenu()
    {
        Base.main.OnScene(Scene.MENU);
    }

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        GameObject.Find("MenuBut").GetComponent<Button>().click = ToMenu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
