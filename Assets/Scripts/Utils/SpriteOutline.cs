using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;
    public int outLineSize = 1;

    private SpriteRenderer spriteRenderer;

    private float waitTime = .5f;
    private float timer = 0.0f;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        AnimateMaterial();
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        if (outLineSize == 0) outline = false;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1 : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outline ? outLineSize : 1);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    void AnimateMaterial()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            outLineSize -= 1;
        }
        else
        {
            outLineSize += 1;
        }

        if (outLineSize < 0)
            timer = 0;
    }
}
