using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxElements : MonoBehaviour
{
    [SerializeField]
    private Transform playerStarShip;
    [SerializeField]
    [Range(0,1)]
    private float paralaxForce = 0.3f;


    private Transform myTransform;
    private Vector3 playerDitrection;

    private void Awake()
    {
        myTransform = transform;
    }

    void Update()
    {
        if(playerStarShip != null)
        {
            playerDitrection = playerStarShip.position - Vector3.zero;
            myTransform.position = Vector3.zero + (playerDitrection * -1 * paralaxForce);
        }
    }
}
