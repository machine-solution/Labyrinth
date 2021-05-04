using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        GameObject.Find("MenuBut").GetComponent<Button>().click = Base.main.OnScene_Menu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
