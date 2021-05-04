using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    float time = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1f)
            Destroy(gameObject);
    }
}
