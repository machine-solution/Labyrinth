using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static System.Math;
using static System.Array;

public class TourEnd : End
{
    // butons
    [SerializeField]
    GameObject StartButton, LocalResultBut, GlobalResultBut, SaveTournamentBut;

    // makes variables visible in inspector
    [SerializeField]
    float distS, distW;

    [System.Obsolete]
    void PlayAgain()
    {
        ShufflePlayers();
        Base.is_fresh_res = false;
        Base.Initialisation(Base.main.n, Base.main.k, Base.main.t, Base.main.f_tp, Base.tourNames, Base.tourTypes, Random.Range(0, 1000000));
        Base.main.OnScene(Scene.TOUR_PLAY);
    }


    public void ShowLocalResults()
    {
        HideResults();
        base.ShowResults();
    }
    public void ShowGlobalResults()
    {
        HideResults();
        lines = new GameObject[Base.numberOfPlayers];
        medals = new GameObject[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            lines[i] = Instantiate(Resources.Load<GameObject>("lineTextTour"));
            lines[i].transform.position = new Vector3(3.5f, ressY + i * resdY, 0);
        }
        keys = new int[Base.numberOfPlayers];
        items = new int[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            // bad code, remove magic constant
            keys[i] = -Base.tourPoints[i] * 1000000 - Base.tourTreasures[i];
            items[i] = i;
        }
        Sort(keys, items, 0, Base.numberOfPlayers);
        int place = -1;
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            if (i == 0 || keys[i] > keys[i - 1])
                place = i;
            medals[i] = Instantiate(Resources.Load<GameObject>("medal_" + (place + 1).ToString()));
            medals[i].transform.position =
                lines[i].transform.position + new Vector3(-4.1f, 0, 0);
            lines[i].transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = Base.currentPosition.players[items[i]].name;
            lines[i].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = Base.tourPoints[items[i]].ToString();
            lines[i].transform.GetChild(2).GetComponent<TMPro.TextMeshPro>().text = Base.tourTreasures[items[i]].ToString();
        }
        showed_result = true;
    }

    //________________________________
    void FreshResults()
    {
        if (Base.is_fresh_res)
            return;
        Base.is_fresh_res = true;
        int[] keys = new int[Base.numberOfPlayers];
        int[] items = new int[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            keys[i] = -Base.results[i];
            items[i] = i;
        }
        Sort(keys, items, 0, Base.numberOfPlayers);
        int[] place = new int[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            if (i == 0 || keys[i] > keys[i - 1])
                place[i] = i + 1;
            else
                place[i] = place[i - 1];
            Base.tourPoints[items[i]] += Base.numberOfPlayers - place[i];
            Base.tourTreasures[items[i]] += Base.results[items[i]];
        }
    }
    void ShufflePlayers()
    {
        int[] newTourTreasures = new int[Base.numberOfPlayers];
        int[] newTourPoints = new int[Base.numberOfPlayers];
        string[] newTourNames = new string[Base.numberOfPlayers];
        int[] newTourTypes = new int[Base.numberOfPlayers];
        int[] mass = new int[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
            mass[i] = 1;
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            int ind = Base.MassRand(mass);
            mass[ind] = 0;
            newTourTreasures[i] = Base.tourTreasures[ind];
            newTourNames[i] = Base.tourNames[ind];
            newTourPoints[i] = Base.tourPoints[ind];
            newTourTypes[i] = Base.tourTypes[ind];
        }

        Copy(newTourTreasures, Base.tourTreasures,Base.numberOfPlayers);
        Copy(newTourPoints, Base.tourPoints, Base.numberOfPlayers);
        Copy(newTourNames, Base.tourNames, Base.numberOfPlayers);
        Copy(newTourTypes, Base.tourTypes, Base.numberOfPlayers);

    }

    //________________________________
    void SaveCurrentTournament()
    {
        GameObject saver = Instantiate(Resources.Load<GameObject>("saver_tournament"));
        saver.GetComponent<Saver>().click_string = Base.main.SaveTournament;
    }

    [System.Obsolete]
    void ToMenu()
    {
        Base.main.OnScene(Scene.MENU);
    }

    [System.Obsolete]
    void ToMap()
    {
        Base.main.OnScene(Scene.MAP);
    }

    [System.Obsolete]
    void Start()
    {
        StartButton.GetComponent<Button>().click = PlayAgain;
        MenuBut.GetComponent<Button>().click = ToMenu;
        MapBut.GetComponent<Button>().click = ToMap;
        LocalResultBut.GetComponent<Button>().click = ShowLocalResults;
        GlobalResultBut.GetComponent<Button>().click = ShowGlobalResults;
        SaveTournamentBut.GetComponent<Button>().click = SaveCurrentTournament;

        FreshResults();
        Base.tourNum++;
        ShowGlobalResults();
        if (Base.tourNum % 1 > 0)
            PlayAgain();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
