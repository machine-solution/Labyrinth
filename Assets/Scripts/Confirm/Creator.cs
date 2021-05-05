using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public VoidFunc_Int click_int;

    void Yes()
    {
        string arg_string = transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text;
        click_int?.Invoke(System.Convert.ToInt32(arg_string));
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
