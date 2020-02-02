using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    TextMesh txt;
    private float deltaTotal = 0;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTotal += (Time.unscaledDeltaTime - deltaTotal) * 0.1f;
        float msec = deltaTotal * 1000.0f;
        float fps = 1.0f / deltaTotal;
        txt.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }
}



