using UnityEngine;

public class Light : MonoBehaviour
{

    public float x, y; 
    public static float dx = 0.13f, dy = 0.97f;
    public static float ybound = (5.2f + 4.51f + dy)/2, xbound = (11.82f + 12.475f + dy)/2;

    void NormX()
    {
        if (x > xbound)
        {
            x -= xbound * 2;
            y += xbound * 2 * dx / dy;
        }
        if (x < -xbound)
        {
            x += xbound * 2;
            y -= xbound * 2 * dx / dy;
        }
    }

    void NormY()
    {
        if (y > ybound)
        {
            y -= ybound * 2;
            x -= ybound * 2 * dx / dy;
        }
        if (y < -ybound)
        {
            y += ybound * 2;
            x += ybound * 2 * dx / dy;
        }
    }

    void Norm()
    {
        NormX();
        NormY();
        NormX();
    }

    void Move(string side)
    {
        if (side == "up")
        {
            x += dx / cntIt;
            y += dy / cntIt;
            Norm();
        }
        if (side == "right")
        {
            x += dy / cntIt;
            y -= dx / cntIt;
            Norm();
        }
        if (side == "down")
        {
            x -= dx / cntIt;
            y -= dy / cntIt;
            Norm();
        }
        if (side == "left")
        {
            x -= dy / cntIt;
            y += dx / cntIt;
            Norm();
        }
    }

    const int cntIt = 256;
    int it = 0;

    void Start()
    {
        ind = Random.Range(0, 4);
    }

    string[] sides = new string[4] { "up", "right", "down", "left" };
    int ind = 0;

    void Update()
    {
        if (it++ < cntIt)
            Move(sides[ind]);
        else
        {
            int[] mass = { 1, 1, 1, 1 };
            if (ind == 0)
                --mass[2];
            if (ind == 1)
                --mass[3];
            if (ind == 2)
                --mass[0];
            if (ind == 3)
                --mass[1];
            it = 0;
            ind = Base.MassRand(mass);
            Move(sides[ind]);
        }

        transform.localPosition = new Vector3(x, y, 0);
    }
}
