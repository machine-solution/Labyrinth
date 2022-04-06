using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static System.Math;
using static System.Array;
using System;

public class End : MonoBehaviour
{

    public bool is_result = false;
    GameObject[] lines, medals;

    int[] keys, items;
    //__________________________________
    public float resmX = -4.1f;
    public float ressY = 4.1f;
    public float resdY = -1.02f;

    public void ShowResults()
    {
        if (is_result)
            return;
//        HideMap();
        is_result = true;
        lines = new GameObject[Base.numberOfPlayers];
        medals = new GameObject[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            lines[i] = Instantiate(Resources.Load<GameObject>("lineText"));
            lines[i].transform.position = new Vector3(3.5f, ressY + i * resdY, 0);
        }
        keys = new int[Base.numberOfPlayers];
        items = new int[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            keys[i] = -Base.results[i];
            items[i] = i;
        }
        Sort(keys, items, 0, Base.numberOfPlayers);
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            keys[i] = -keys[i];
        }
        int place = -1;
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            if (i == 0 || keys[i] < keys[i - 1])
                place = i;
            medals[i] = Instantiate(Resources.Load<GameObject>("medal_" + (place + 1).ToString()));
            medals[i].transform.position =
                lines[i].transform.position + new Vector3(-4.1f, 0, 0);
            lines[i].transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = Base.currentPosition.players[items[i]].name;
            lines[i].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = keys[i].ToString();
        }
        is_result = true;
    }
    public void HideResults()
    {
        if (!is_result)
            return;
        is_result = false;
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            Destroy(lines[i]);
            Destroy(medals[i]);
        }
        lines = medals = new GameObject[0];
        keys = items = new int[0];
    }

    [Obsolete]
    void ToMap()
    {
        Base.main.OnScene(Scene.MAP);
    }

    [Obsolete]
    void ToMenu()
    {
        Base.main.OnScene(Scene.MENU);
    }

    //________________________________
    [Obsolete]
    void Start()
    {
        
        GameObject.Find("MapBut").GetComponent<Button>().click = ToMap;
//        GameObject.Find("ResultBut").GetComponent<Button>().click = ShowResults;
        GameObject.Find("MenuBut").GetComponent<Button>().click = ToMenu;
        ShowResults();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
