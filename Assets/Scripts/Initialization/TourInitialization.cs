using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourInitialization : Initialization
{
    [System.Obsolete]
    public new void StartGame()
    {
        Base.NewTournament(Base.main.k);
        int[] type = new int[Base.main.k];
        string[] name = new string[Base.main.k];
        for (int i = 0; i < Base.main.k; ++i)
        {
            type[i] = playerInit[i].transform.GetChild(1).GetComponent<Changer>().val;
            name[i] = playerInit[i].transform.GetChild(0).GetComponent<InputText>().text;
            Base.tourNames[i] = name[i];
            Base.tourTypes[i] = type[i];
        }
        Base.Initialisation(Base.main.n, Base.main.k, Base.main.t, Base.main.f_tp, name, type, Random.Range(0, 1000000));
        Base.main.OnScene_TourPlay();
    }
    // Start is called before the first frame update
    void Start()
    {
        startBut = GameObject.Find("StartButton");
        loadBut = GameObject.Find("LoadBut");
        GameObject.Find("MenuBut").GetComponent<Button>().click = Base.main.OnScene_Menu;
        startBut.SetActive(true);
        startBut.GetComponent<Button>().click = StartGame;
        loadBut.SetActive(true);
        loadBut.GetComponent<Button>().click = OpenLoadList;
        playerInit = new GameObject[Base.main.k];
        for (int i = 0; i < Base.main.k; ++i)
        {
            /*if (Base.main.f_tp)
                playerInit[i] = Instantiate(Resources.Load<GameObject>("competitor(teleport) Variant"));
            else*/
            playerInit[i] = Instantiate(Resources.Load<GameObject>("competitor"));
            playerInit[i].transform.position = new Vector3(-7f, 3.9f - 1.1f * i, 0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
