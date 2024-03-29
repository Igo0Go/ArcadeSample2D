﻿using System.Collections.Generic;
using UnityEngine;

public class LaserRenderer : MonoBehaviour
{
    [SerializeField]
    private List<Transform> controlPoints = null;
    [SerializeField, Min(1)]
    private float textureOffsetSpeed = 1;
    [SerializeField]
    private LineRenderer centerRenderer = null;
    [SerializeField]
    private LineRenderer outlineRenderer = null;

    private float textureOffset = 0;

    private void Start()
    {
        centerRenderer.positionCount = controlPoints.Count;
        if(outlineRenderer != null)
        {
            outlineRenderer.positionCount = controlPoints.Count;
        }
    }

    void Update()
    {
        CheckLineRenderer();
        MoveTexture();
    }

    private void CheckLineRenderer()
    {
        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (outlineRenderer != null)
            {
                outlineRenderer.SetPosition(i, controlPoints[i].localPosition);
            }
            centerRenderer.SetPosition(i, controlPoints[i].localPosition);
        }
    }

    private void MoveTexture()
    {
        if(outlineRenderer != null)
        {
            textureOffset -= Time.deltaTime * textureOffsetSpeed;
            if (textureOffset < -10)
            {
                textureOffset += 10;
            }
            for (int i = 0; i < outlineRenderer.sharedMaterials.Length; i++)
            {
                outlineRenderer.sharedMaterials[i].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0));
            }
        }
    }
}
