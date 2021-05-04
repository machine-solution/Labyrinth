using UnityEngine;
using static System.Math;

public class Scroller : MonoBehaviour
{
    public int max_val = 10;
    public int min_val = 0;
    public int val;
    public GameObject line, point;
    float scroller_x;
    float local_broad = 2.8f, broad = 2.8f;

    public delegate void IntFunc(int value);
    public IntFunc Drag;

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

    }

    public void ResetLimits(int newMin, int newMax)
    {
        min_val = newMin;
        max_val = newMax;
        SetVal(val);
    }

    public void SetVal(int new_val) //set val
    {
        val = new_val;
        float x = -local_broad + (val - min_val) * (2 * local_broad) / (max_val - min_val);
        if (!point)
            Init();
        point.transform.localPosition = new Vector3(x, 0, 0);
    }

    public int GetVal()
    {
        return val;
    }

    void Start()
    {
        SetVal(val);
        Transform tr = GetComponent<Transform>();
        scroller_x = tr.position.x;
        broad *= line.transform.lossyScale.x;
    }

    public void OnMouseDownScroller()
    {
        drag = true;
    }

    public void OnMouseDragScroller()
    {
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - scroller_x;
        x = Max(x, -broad);
        x = Min(x, broad);
        SetVal(Mathf.RoundToInt((x - broad) / (2 * broad) * (max_val - min_val)) + max_val);
        Drag?.Invoke(val);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            drag = false;
        if (drag)
            OnMouseDragScroller();
    }
}
