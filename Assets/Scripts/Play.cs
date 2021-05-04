using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static System.Math;
using static System.Array;

public class Play : MonoBehaviour
{
    //__________________________________
    GameObject[] playerFrame;
    public GameObject forwardBut, backBut, startBut, finishBut;
    public static GameObject actList;
    public int menuLeftPl;
    public int menuRightPl;
    public GameObject menuBut, enteringBut;

#if UNITY_ANDROID
    TouchScreenKeyboard keyBoard;
#endif

    //_______________________________
    public float menuLeftX = -5.63f;
    public float menuLeftY = -3f;
    public float menuDeltaX = 2.9f;
    //_______________________________
    public void TypeStep()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionType = "step";
        Base.actType.SetActive(false);
        Base.actSide.SetActive(true);
    }

    public void TypeStrike()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionType = "strike";
        Base.actType.SetActive(false);
        Base.actSide.SetActive(true);
    }

    public void TypeFire()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionType = "fire";
        Base.actType.SetActive(false);
        Base.actSide.SetActive(true);
    }

    public void TypeThrow()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionType = "throw";
        Base.actType.SetActive(false);
        Base.actSide.SetActive(true);
    }
    //_______________________________
    public void SideUp()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionSide = "up";
        Base.Next();
    }

    public void SideRight()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionSide = "right";
        Base.Next();
    }

    public void SideDown()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionSide = "down";
        Base.Next();
    }

    public void SideLeft()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionSide = "left";
        Base.Next();
    }

    public void SideCancel()
    {
        Base.currentPosition.players[Base.currentPosition.index].actionType = "";
        Base.actType.SetActive(true);
        Base.actSide.SetActive(false);
    }

    //_______________________________

    public void Finish()
    {
        Base.endOfGame = true;
    }

    public void SweepPlayerStats(int len)
    {
        if (Base.numberOfPlayers <= 3)
            return;
        menuLeftPl = ((menuLeftPl + len) % Base.numberOfPlayers + Base.numberOfPlayers) % Base.numberOfPlayers;
        menuRightPl = ((menuRightPl + len) % Base.numberOfPlayers + Base.numberOfPlayers) % Base.numberOfPlayers;
        for (int i = 0; i < Base.numberOfPlayers; ++i)
            Base.playersText[i].statsList.SetActive(false);
        for (int i = menuLeftPl; i != (menuRightPl + 1) % Base.numberOfPlayers; i = (i + 1) % Base.numberOfPlayers)
        {
            Base.playersText[i].statsList.SetActive(true);
        }
        for (int i = menuLeftPl; i != (menuRightPl + 1) % Base.numberOfPlayers; i = (i + 1) % Base.numberOfPlayers)
        {
            Base.playersText[i].statsList.transform.position = new Vector3(
                menuLeftX + menuDeltaX * (((i - menuLeftPl) % Base.numberOfPlayers + Base.numberOfPlayers) % Base.numberOfPlayers), -2.9f, 0);
        }
    }

    public void SweepBack()
    {
        SweepPlayerStats(-1);
    }

    public void SweepForward()
    {
        SweepPlayerStats(1);
    }
    //_______________________________
/*    public void enterName()
    {
        Base.players[playerNumber].name = enteringText.text;
        Base.players[playerNumber].nameText.text = (enteringText.text + " [0]");
        ++playerNumber;
        if (playerNumber < Base.numberOfBots)
            enteringText.text = "II";
        else
            enteringText.text = ""; 
        if (playerNumber == Base.numberOfPlayers)
        {
            enteringName.SetActive(false);
            enteringBut.SetActive(false);
            Base.actType.SetActive(true);
            finishBut.SetActive(true);
            if (Base.players[Base.index].knifes == 0)
                Base.strikeBut.GetComponent<Button>().block();
            else
                Base.strikeBut.GetComponent<Button>().unlock();
            if (Base.players[Base.index].bullets == 0)
                Base.fireBut.GetComponent<Button>().block();
            else
                Base.fireBut.GetComponent<Button>().unlock();
            Base.playProcess = true;
            StopCoroutine("playerInit");
        }
#if UNITY_ANDROID
        keyBoard = TouchScreenKeyboard.Open(enteringText.text,
                TouchScreenKeyboardType.Default, false, false, false, false, enteringText.text, 9);
#endif
    }*/

    public void WorkableInit()
    {
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            Base.playersText[i] = new PlayerText();
            Base.playersText[i].statsList =
                Instantiate(Resources.Load<GameObject>("StatsOfPlayer"));
            Base.playersText[i].statsList.SetActive(true);
            if (i < 3)
                Base.playersText[i].statsList.transform.Translate(
                    new Vector3(menuLeftX + menuDeltaX * i, -2.9f, 0));
            else
                Base.playersText[i].statsList.SetActive(false);
            Base.playersText[i].nameText =
                Base.playersText[i].statsList.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshPro>();
            Base.playersText[i].nameText.text = Base.currentPosition.players[i].name + " [0]";
            Base.playersText[i].knifeText =
                Base.playersText[i].statsList.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshPro>();
            Base.playersText[i].knifeText.text = "knifes: 1";
            Base.playersText[i].bulletText =
                Base.playersText[i].statsList.transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshPro>();
            Base.playersText[i].bulletText.text = "bullets: 0";
            Base.playersText[i].crackerText =
                Base.playersText[i].statsList.transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshPro>();
            Base.playersText[i].crackerText.text = "crackers: 0";
            Base.playersText[i].armorText =
                Base.playersText[i].statsList.transform.GetChild(4).gameObject.GetComponent<TMPro.TextMeshPro>();
            Base.playersText[i].armorText.text = "armors: 0";
            Base.playersText[i].treasureText =
                Base.playersText[i].statsList.transform.GetChild(5).gameObject.GetComponent<TMPro.TextMeshPro>();
            Base.playersText[i].treasureText.text = "treasures: 0";

        }
        playerFrame = new GameObject[Min(Base.numberOfPlayers, 3)];
        menuLeftPl = 0;
        menuRightPl = Min(Base.numberOfPlayers, 3) - 1;
        for (int j = 0; j < Min(Base.numberOfPlayers, 3); ++j)
        {
            playerFrame[j] = Instantiate(Resources.Load<GameObject>("frame"));
            playerFrame[j].transform.position = new Vector3(menuLeftX - 0.4f + j * menuDeltaX, menuLeftY, 0);
        }
        forwardBut.SetActive(true);
        forwardBut.transform.position = new Vector3((Min(Base.numberOfPlayers, 3) - 1) * menuDeltaX - 4.19f, -3f, 0);
        backBut.SetActive(true);
        for (int i = 0; i < 5; ++i)
        {
            Base.actList[i] = actList.transform.GetChild(i).gameObject;
        }
        if (Base.numberOfPlayers <= 3)
        {
            forwardBut.GetComponent<Button>().Block();
            backBut.GetComponent<Button>().Block();
        }
        else
        {
            forwardBut.GetComponent<Button>().Unlock();
            backBut.GetComponent<Button>().Unlock();
        }
        actList.SetActive(true);

        Base.NormalizeButtons();

    }

    [System.Obsolete]
    void Start()
    {
        forwardBut = GameObject.Find("ForwardBut");
        GameObject.Find("ForwardBut").GetComponent<Button>().click = SweepForward;
        backBut = GameObject.Find("BackBut");
        GameObject.Find("BackBut").GetComponent<Button>().click = SweepBack;
        actList = GameObject.Find("ActionList");
        Base.actType = GameObject.Find("ActType");
        Base.actType.transform.GetChild(0).GetComponent<Button>().click = TypeStep;
        Base.actType.transform.GetChild(1).GetComponent<Button>().click = TypeFire;
        Base.actType.transform.GetChild(2).GetComponent<Button>().click = TypeStrike;
        Base.actType.transform.GetChild(3).GetComponent<Button>().click = TypeThrow;
        Base.actSide = GameObject.Find("ActSide");
        Base.actSide.transform.GetChild(0).GetComponent<Button>().click = SideUp;
        Base.actSide.transform.GetChild(1).GetComponent<Button>().click = SideRight;
        Base.actSide.transform.GetChild(2).GetComponent<Button>().click = SideDown;
        Base.actSide.transform.GetChild(3).GetComponent<Button>().click = SideLeft;
        Base.actSide.transform.GetChild(4).GetComponent<Button>().click = SideCancel;
        Base.strikeBut = GameObject.Find("Strike");
        Base.fireBut = GameObject.Find("Fire");
        Base.throwBut = GameObject.Find("Throw");
        startBut = GameObject.Find("StartButton");
        menuBut = GameObject.Find("MenuBut");
        menuBut.GetComponent<Button>().click = Base.main.OnScene_Menu;
        finishBut = GameObject.Find("FinishBut");
        finishBut.GetComponent<Button>().click = Finish;

        forwardBut.SetActive(true);
        backBut.SetActive(true);
        actList.SetActive(true);
        Base.actType.SetActive(true);
        Base.actSide.SetActive(false);
        finishBut.SetActive(true);

        WorkableInit();
    }

    [System.Obsolete]
    void Update()
    {
        if (Base.endOfGame)
        {
            Base.endOfGame = false;
            Base.SaveResults();
            if (!Base.is_tournament)
                Base.main.OnScene_End();
            else
                Base.main.OnScene_TourEnd();
        }
        else if (!Base.endOfGame && (Base.currentPosition.players[Base.currentPosition.index].bot != null))
        {
            
            Base.currentPosition.players[Base.currentPosition.index].actionType = Base.currentPosition.players[Base.currentPosition.index].bot.ansType;
            Base.currentPosition.players[Base.currentPosition.index].actionSide = Base.currentPosition.players[Base.currentPosition.index].bot.ansSide;
            Base.Next();
        }
    }
}
