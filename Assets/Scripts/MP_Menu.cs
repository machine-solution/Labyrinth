using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Menu : MonoBehaviour
{
    IEnumerable WaitCreation(int n)
    {
        while (Web.res == "")
            yield return null;

        if (Web.res.Substring(0,8) == "Успешно!")
        {
            // переходим в меню для хоста.
        }
        else if (Web.res == "-1")
        {
            // отсутствует интернет
        }
    }

    void CreateRoom(int n)
    {
        WaitCreation(n);
    }

    void CreateButton()
    {
        GameObject creator = Instantiate(Resources.Load<GameObject>("creator_room"));
        creator.GetComponent<Creator>().click_int = CreateRoom;
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
