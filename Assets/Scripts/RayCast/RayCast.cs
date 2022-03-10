using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class RayCast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = new RaycastHit2D[228];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = false;
            

            int cnt = Physics2D.Raycast(ray, Vector2.zero, filter, hit);

            int maxLayer = 0; //default
            for (int i = 0; i < cnt && i < 228; ++i)
            {
                maxLayer = Max(maxLayer, hit[i].transform.gameObject.layer);
            }
            for (int i = 0; i < cnt; ++i)
            {
                if (hit[i].transform.gameObject.layer == maxLayer)
                { 
                    if (hit[i].transform.tag == "button")
                    {
                        hit[i].transform.gameObject.GetComponent<Button>().OnMouseDownBut();
                    }
                    if (hit[i].transform.tag == "changer")
                    {
                        hit[i].transform.gameObject.GetComponent<Changer>().OnMouseDownChanger();
                    }
                    if (hit[i].transform.tag == "slider")
                    {
                        hit[i].transform.gameObject.GetComponent<Slider>().OnMouseDownSlider();
                    }
                    if (hit[i].transform.tag == "scroller")
                    {
                        hit[i].transform.gameObject.GetComponent<Scroller>().OnMouseDownScroller();
                    }
                    if (hit[i].transform.tag == "background")
                    {
//                        GameObject click = Instantiate<GameObject>(Resources.Load<GameObject>("Click effect"));
//                        click.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                        click.transform.position += new Vector3(0, 0, 10);
                    }
                }
            }
        }
        // left click
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = new RaycastHit2D[228];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = false;


            int cnt = Physics2D.Raycast(ray, Vector2.zero, filter, hit);

            int maxLayer = 0; //default
            for (int i = 0; i < cnt && i < 228; ++i)
            {
                maxLayer = Max(maxLayer, hit[i].transform.gameObject.layer);
            }
            for (int i = 0; i < cnt; ++i)
            {
                if (hit[i].transform.gameObject.layer == maxLayer)
                {
                    if (hit[i].transform.tag == "button")
                    {
                        hit[i].transform.gameObject.GetComponent<Button>().OnMouseUpBut();
                    }
                    if (hit[i].transform.tag == "changer")
                    {
                        hit[i].transform.gameObject.GetComponent<Changer>().OnMouseUpChanger(1);
                    }
                    if (hit[i].transform.tag == "toggle")
                    {
                        hit[i].transform.gameObject.GetComponent<Toggle>().OnMouseUpToggle();
                    }
                    if (hit[i].transform.tag == "inputtext")
                    {
                        hit[i].transform.gameObject.GetComponent<InputText>().OnMouseUpInput();
                    }
                }
            }
        }
        // right click
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = new RaycastHit2D[228];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = false;


            int cnt = Physics2D.Raycast(ray, Vector2.zero, filter, hit);

            int maxLayer = 0; //default
            for (int i = 0; i < cnt && i < 228; ++i)
            {
                maxLayer = Max(maxLayer, hit[i].transform.gameObject.layer);
            }
            for (int i = 0; i < cnt; ++i)
            {
                if (hit[i].transform.gameObject.layer == maxLayer)
                {
                    if (hit[i].transform.tag == "changer")
                    {
                        hit[i].transform.gameObject.GetComponent<Changer>().OnMouseDownChanger();
                    }
                }
            }
        }
        // right click
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = new RaycastHit2D[228];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = false;


            int cnt = Physics2D.Raycast(ray, Vector2.zero, filter, hit);

            int maxLayer = 0; //default
            for (int i = 0; i < cnt && i < 228; ++i)
            {
                maxLayer = Max(maxLayer, hit[i].transform.gameObject.layer);
            }
            for (int i = 0; i < cnt; ++i)
            {
                if (hit[i].transform.gameObject.layer == maxLayer)
                {
                    if (hit[i].transform.tag == "changer")
                    {
                        hit[i].transform.gameObject.GetComponent<Changer>().OnMouseUpChanger(-1);
                    }
                }
            }
        }

    }
}
