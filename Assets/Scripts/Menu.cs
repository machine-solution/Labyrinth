using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [System.Obsolete]
    void OnceGame()
    {
        Base.is_tournament = false;
        Base.main.OnScene_Initialization();
    }

    [System.Obsolete]
    void TourGame()
    {
        Base.is_tournament = true;
        Base.main.OnScene_Initialization();
    }

    [System.Obsolete]
    void Start()
    {
        GameObject.Find("StartButton").GetComponent<Button>().click = OnceGame;
        GameObject.Find("SettingsButton").GetComponent<Button>().click = Base.main.OnScene_Settings;
        GameObject.Find("TournamentButton").GetComponent<Button>().click = TourGame;
        GameObject.Find("QuitButton").GetComponent<Button>().click = Application.Quit;
        GameObject.Find("RulesButton").GetComponent<Button>().click = Base.main.OnScene_Rules;
    }

    void Update()
    {
        
    }
}
