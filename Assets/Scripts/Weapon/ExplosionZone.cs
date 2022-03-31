using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10)]
    private float scaleSpeed;

    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        myTransform.localScale += Vector3.one * scaleSpeed * GameTime.DeltaTime;
    }
}
