using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourPlay : Play
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (Base.endOfGame)
        {
            Base.endOfGame = false;
            Base.SaveResults();
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
