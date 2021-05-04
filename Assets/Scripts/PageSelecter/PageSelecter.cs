using UnityEngine;

public class PageSelecter : MonoBehaviour
{
    public GameObject[] page;
    public GameObject[] button;
    private VariantButton[] scriptBut;
    private int selected = 0;

    public void Select(int id)
    {
        selected = id;
        for (int i = 0; i < page.Length; ++i)
        {
            if (i == id)
            {
                page[i].SetActive(true);
                scriptBut[i].Select(true);
            }
            else
            {
                page[i].SetActive(false);
                scriptBut[i].Select(false);
            }
        }
    }

    void Start()
    {
        scriptBut = new VariantButton[button.Length];
        for (int i = 0; i < button.Length; ++i)
            scriptBut[i] = button[i].GetComponent<VariantButton>();
        Select(selected);
    }


    void Update()
    {
        
    }
}
