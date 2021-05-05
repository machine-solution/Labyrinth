using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{
    //_____________________________
    public VoidFunc click;
    //_____________________________
    public VoidFunc_Int click_int;
    public int arg_int;
    //_____________________________
    public VoidFunc_String click_string;
    public string arg_string;

    void Yes()
    {
        click?.Invoke();
        click_int?.Invoke(arg_int);
        click_string?.Invoke(arg_string);
        Destroy(gameObject);
    }

    void No()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        transform.GetChild(0).GetComponent<Button>().click = Yes;
        transform.GetChild(1).GetComponent<Button>().click = No;
    }

    private void OnMouseDown()
    {
        Debug.Log("!click on confirm shade!");
    }


    void Update()
    {
        
    }
}
