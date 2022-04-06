using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [System.Obsolete]
    void OnceGame()
    {
        Base.is_tournament = false;
        Base.main.OnScene(Scene.INITIALIZATION);
    }

    [System.Obsolete]
    void TourGame()
    {
        Base.is_tournament = true;
        Base.main.OnScene(Scene.TOUR_INITIALIZATION);
    }

    [System.Obsolete]
    void ToRules()
    {
        Base.main.OnScene(Scene.RULES);
    }

    [System.Obsolete]
    void ToSettings()
    {
        Base.main.OnScene(Scene.SETTINGS);
    }

    [System.Obsolete]
    void Start()
    {
        GameObject.Find("StartButton").GetComponent<Button>().click = OnceGame;
        GameObject.Find("SettingsButton").GetComponent<Button>().click = ToSettings;
        GameObject.Find("TournamentButton").GetComponent<Button>().click = TourGame;
        GameObject.Find("QuitButton").GetComponent<Button>().click = Application.Quit;
        GameObject.Find("RulesButton").GetComponent<Button>().click = ToRules;
    }

    void Update()
    {
        
    }
}
