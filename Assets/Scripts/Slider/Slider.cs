using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Slider : MonoBehaviour
{
    public int max_val = 10;
    public int min_val = 0;
    public string[] specialSymbol;
    public int[] specialKey;
//    public int[] specialValue;
    public int val;
    public GameObject line, point, visualVal, minus, plus;
    float slider_x;
    float local_broad = 1.65f, broad = 1.65f;

    bool drag = false;

    void Inc()
    {
        SetVal(val + 1);
    }

    void Dec()
    {
        SetVal(val - 1);
    }

    void Init()
    {
        if (val <= min_val)
            minus.GetComponent<Button>().Block();
        else
            minus.GetComponent<Button>().Unlock();
        minus.GetComponent<Button>().click = Dec;
        if (val >= max_val)
            plus.GetComponent<Button>().Block();
        else
            plus.GetComponent<Button>().Unlock();
        plus.GetComponent<Button>().click = Inc;
    }

    public void SetVal(int new_val) //set val
    {
        val = new_val;
        float x = -local_broad + (val - min_val) * (2 * local_broad) / (max_val - min_val);
        if (!point)
            Init();
        point.transform.localPosition = new Vector3(x, 0, 0);

        visualVal.GetComponent<TMPro.TextMeshPro>().text = val.ToString();
        for (int i = 0; i < specialKey.Length; ++i)
        {
            if (specialKey[i] == val)
            {
//                val = specialValue[i];
                visualVal.GetComponent<TMPro.TextMeshPro>().text = specialSymbol[i];
            }
        }
        if (val <= min_val)
            minus.GetComponent<Button>().Block();
        else
            minus.GetComponent<Button>().Unlock();
        if (val >= max_val)
            plus.GetComponent<Button>().Block();
        else
            plus.GetComponent<Button>().Unlock();
    }

    public int GetVal()
    {
        return val;
    }

    void Start()
    {
        Init();
        SetVal(val);
        Transform tr = GetComponent<Transform>();
        slider_x = tr.position.x;
        broad *= line.transform.lossyScale.x;
    }

    public void OnMouseDownSlider()
    {
        drag = true;
    }

    public void OnMouseDragSlider()
    {
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - slider_x;
        x = Max(x, -broad);
        x = Min(x, broad);
        SetVal( Mathf.RoundToInt((x - broad) / (2 * broad) * (max_val - min_val)) + max_val);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            drag = false;
        if (drag)
            OnMouseDragSlider();
    }
}
