using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOffset : MonoBehaviour
{
    // Scroll main texture based on time

    public float SpeedX = 0.1f;
    public float SpeedY = 0.1f;
    private float CurX;
    private float CurY;

    void Start()
    {
        CurX = GetComponent<Renderer>().material.mainTextureOffset.x;
        CurY = GetComponent<Renderer>().material.mainTextureOffset.y;
    }

    void FixedUpdate()
    {
        CurX += Time.deltaTime * SpeedX;
        CurY += Time.deltaTime * SpeedY;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(CurX, CurY));
    }
}
