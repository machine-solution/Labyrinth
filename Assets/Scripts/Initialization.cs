using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    GameObject startBut;
    GameObject loadBut;
    //______________________________
    GameObject[] playerInit;

    //______________________________
    GameObject loadList;
    GameObject[] loader;

    [System.Obsolete]
    void OpenLoadList()
    {
        loadList = Instantiate(Resources.Load<GameObject>("loadList"));
        loadList.transform.GetChild(1).GetComponent<Button>().click = CloseLoadList;
        loader = new GameObject[Base.savedTournaments.Size()];
        for (int i = 0; i < Base.savedTournaments.Size(); ++i)
        {
            loader[i] = Instantiate(Resources.Load<GameObject>("gameLoad"));
            loader[i].transform.position = new Vector3(0.8f, 4 - i, 0);
            loader[i].transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text =
                Base.savedTournaments[i];

            loader[i].transform.GetChild(1).GetComponent<Button>().click_int = LoadTour;
            loader[i].transform.GetChild(1).GetComponent<Button>().arg_int = i;

            loader[i].transform.GetChild(2).GetComponent<Button>().click_int = DeleteTour;
            loader[i].transform.GetChild(2).GetComponent<Button>().arg_int = i;
        }
    }

    void CloseLoadList()
    {
        Destroy(loadList);
        for (int i = 0; i < loader.Length; ++i)
            Destroy(loader[i]);
    }

    [System.Obsolete]
    void LoadTour(int n)
    {
        Base.main.LoadTournament(Base.savedTournaments[n]);

        Base.is_fresh_res = false;
        /// Временная инициализация
        Base.Initialisation(Base.main.n, Base.main.k, Base.main.t, Base.main.f_tp, Base.tourNames, Base.tourTypes, Random.Range(0, 1000000));
        Base.main.OnScene_Play();
    }

    [System.Obsolete]
    void DeleteTour(int n)
    {
        Base.main.EraseTournament(Base.savedTournaments[n]);
        CloseLoadList();
        OpenLoadList();
    }

    [System.Obsolete]
    public void StartGame()
    {
        if (Base.is_tournament)
            Base.NewTournament(Base.main.k);
        int[] type = new int[Base.main.k]; 
        string[] name = new string[Base.main.k];
        for (int i = 0; i < Base.main.k; ++i)
        {
            type[i] = playerInit[i].transform.GetChild(1).GetComponent<Changer>().val;
            name[i] = playerInit[i].transform.GetChild(0).GetComponent<InputText>().text;
            if (Base.is_tournament)
            {
                Base.tourNames[i] = name[i];
                Base.tourTypes[i] = type[i];
            }
        }
        Base.Initialisation(Base.main.n, Base.main.k, Base.main.t, Base.main.f_tp, name, type, Random.Range(0, 1000000));
        Base.main.OnScene_Play();
    }

    [System.Obsolete]
    void Start()
    { 
        startBut = GameObject.Find("StartButton");
        loadBut = GameObject.Find("LoadBut");
        GameObject.Find("MenuBut").GetComponent<Button>().click = Base.main.OnScene_Menu;
        startBut.SetActive(true);
        startBut.GetComponent<Button>().click = StartGame;
        loadBut.SetActive(false);
        if (Base.is_tournament)
        {
            loadBut.SetActive(true);
            loadBut.GetComponent<Button>().click = OpenLoadList;
        }
        playerInit = new GameObject[Base.main.k];
        for (int i = 0; i < Base.main.k; ++i)
        {
            if (Base.main.f_tp)
                playerInit[i] = Instantiate(Resources.Load<GameObject>("competitor(teleport) Variant"));
            else
                playerInit[i] = Instantiate(Resources.Load<GameObject>("competitor"));
            playerInit[i].transform.position = new Vector3(-7f, 3.9f - 1.1f * i, 0f);
        }

    }

    void Update()
    {
        
    }
}
