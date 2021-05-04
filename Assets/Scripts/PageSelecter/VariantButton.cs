using UnityEngine;

public class VariantButton : MonoBehaviour
{
    public int id;
    public Sprite selected, notselected;

    public void Select(bool yes)
    {
        if (yes)
            GetComponent<SpriteRenderer>().sprite = selected;
        else
            GetComponent<SpriteRenderer>().sprite = notselected;
    }

    void Start()
    {
        
    }

    private void OnMouseUp()
    {
        transform.parent.GetComponent<PageSelecter>().Select(id);
    }


    void Update()
    {
        
    }
}
