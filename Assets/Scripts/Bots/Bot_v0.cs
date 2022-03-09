using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bot_v0
{
    int n; /// size of labyrinth

    Coord myCoord;
    int myIndex;

    DoublyLinkedList<string> myLifeWay;
    string typeOfMove = "step";
    string sideOfMove = "down";

    public Bot_v0()
    {

    }

    void FindWay(int x, int y)
    {
        myLifeWay = null;
        Coord[,] prev = new Coord[n, n];
        bool[,] used = new bool[n, n];
        Queue<Coord> order = new Queue<Coord>();
        int[,] distanse = new int[n, n];
        string[,] side = new string[n, n];

        for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                used[i, j] = false;
                distanse[i, j] = 0;
            }

        order.Enqueue(new Coord(x, y));

        Bfs(ref prev, ref used, ref order, ref distanse, ref side);

        float[,] rating = new float[n, n];

        Coord aim = new Coord(0, 0);

        for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                if (distanse[i, j] > 0)
                    rating[i, j] = Base.CountOfTreasures(new Coord(i, j)) * 1.0f / distanse[i, j];
                else
                    rating[i, j] = -1;

                if (rating[i, j] > rating[aim.x, aim.y])
                    aim = new Coord(i, j);
                else if (rating[i, j] == rating[aim.x, aim.y])
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        aim = new Coord(i, j);
                    }
                }
            }

        myLifeWay = BuildWay(new Coord(x, y), aim, ref prev, ref side);
    }

    DoublyLinkedList<string> BuildWay(Coord start, Coord finish, ref Coord[,] prev, ref string[,] side)
    {
        DoublyLinkedList<string> answer = new DoublyLinkedList<string>();
        if (start != finish)
        {
            answer = BuildWay(prev[start.x, start.y], finish, ref prev, ref side);
            answer.Push_front(side[start.x, start.y]);
        }
        return answer;
    }

    void Bfs(ref Coord[,] prev, ref bool[,] used, ref Queue<Coord> order, ref int[,] distanse, ref string[,] side)
    {
        Coord p = order.Dequeue();
        int i = p.x;
        int j = p.y;
        used[i, j] = true;

        Coord q;

        q = Base.Right(p);
        q = Base.Teleporting(q);
        if (!used[q.x, q.y] && q.x < n && q.x >= 0 && q.y < n && q.y >= 0)
        {
            order.Enqueue(q);
            prev[q.x, q.y] = p;
            distanse[q.x, q.y] = distanse[i, j] + 1;
            side[q.x, q.y] = "right";
        }

        q = Base.Up(p);
        q = Base.Teleporting(q);
        if (!used[q.x, q.y] && q.x < n && q.x >= 0 && q.y < n && q.y >= 0)
        {
            order.Enqueue(q);
            prev[q.x, q.y] = p;
            distanse[q.x, q.y] = distanse[i, j] + 1;
            side[q.x, q.y] = "up";
        }

        q = Base.Left(p);
        q = Base.Teleporting(q);
        if (!used[q.x, q.y] && q.x < n && q.x >= 0 && q.y < n && q.y >= 0)
        {
            order.Enqueue(q);
            prev[q.x, q.y] = p;
            distanse[q.x, q.y] = distanse[i, j] + 1;
            side[q.x, q.y] = "left";
        }

        q = Base.Down(p);
        q = Base.Teleporting(q);
        if (!used[q.x, q.y] && q.x < n && q.x >= 0 && q.y < n && q.y >= 0)
        {
            order.Enqueue(q);
            prev[q.x, q.y] = p;
            distanse[q.x, q.y] = distanse[i, j] + 1;
            side[q.x, q.y] = "down";
        }

        if (order.Count > 0)
            Bfs(ref prev, ref used, ref order, ref distanse, ref side);
    }

    public void DoMove()
    {
        myCoord = Base.currentPosition.players[myIndex].location;
        if (true)
        {
            FindWay(myCoord.x, myCoord.y);
            typeOfMove = "step";
            sideOfMove = myLifeWay.Front();
            myLifeWay.Pop_front();
        }
        Base.currentPosition.players[myIndex].actionSide = sideOfMove;
        Base.currentPosition.players[myIndex].actionType = typeOfMove;
    }

    public void Join(int index, Coord cor)
    {
        myIndex = index;
        myCoord = cor;
    }
}

public class Bot_Notebook : MonoBehaviour
{
    
}
