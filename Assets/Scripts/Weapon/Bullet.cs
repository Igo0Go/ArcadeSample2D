using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10)]
    private float bulletSpeed = 5;

    [SerializeField]
    private LayerMask ignoreMask;

    [SerializeField]
    private GameObject decal;

    private Transform myTransform;
    private Vector3 oldpos;
    private ContactFilter2D filter2D;
    RaycastHit2D hit;

    private void Start()
    {
        myTransform = transform;
        oldpos = myTransform.position;
        Destroy(gameObject, 5);
    }

    void Update()
    {
        MoveBullet();
    }


    private void MoveBullet()
    {
        oldpos = myTransform.position;
        myTransform.position += myTransform.up * bulletSpeed * Time.deltaTime;
        CheckCollision();
    }

    private void CheckCollision()
    {
        hit = Physics2D.Linecast(oldpos, myTransform.position, ~ignoreMask);
        if (hit.collider != null)
        {
            //if(hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            //{

            //}


            Instantiate(decal, hit.point, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
