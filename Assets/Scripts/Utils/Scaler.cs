﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public ScreenOrientation Orientation { get; private set; }
    private ScreenOrientation m_orientation;

    [SerializeField]
    private bool keepAspectRatio = true;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckOrientation();
    }

    private void Resize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        var worldSpaceWidth = topRightCorner.x * 2;
        var worldSpaceHeight = topRightCorner.y * 2;

        var spriteSize = spriteRenderer.bounds.size;

        var scaleFactorX = worldSpaceWidth / spriteSize.x;
        var scaleFactorY = worldSpaceHeight / spriteSize.y;

        if (keepAspectRatio)
        {
            if (scaleFactorX > scaleFactorY)
            {
                scaleFactorY = scaleFactorX;
            }
            else
            {
                scaleFactorX = scaleFactorY;
            }
        }

        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }

    public void CheckOrientation(bool resize = true)
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown || Screen.width < Screen.height)
        {
            Orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Orientation = ScreenOrientation.Landscape;
        }

        if (resize && m_orientation != Orientation)
        {
            OnOrientationChange();
        }

        m_orientation = Orientation;
    }


    private void OnOrientationChange()
    {
        Console.Log("OrientationChange");
        Resize();
    }
}
