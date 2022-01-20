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

    protected Vector3 bulletDirection;
    protected Transform myTransform;

    private Vector3 oldpos;
    RaycastHit2D hit;

    private void Start()
    {
        OnStart();
    }

    protected void OnStart()
    {
        myTransform = transform;
        oldpos = myTransform.position;
        Destroy(gameObject, 5);
        bulletDirection = myTransform.up;
    }

    void Update()
    {
        MoveBullet();
    }


    protected void MoveBullet()
    {
        oldpos = myTransform.position;
        myTransform.position += bulletDirection * bulletSpeed * Time.deltaTime;
        CheckCollision();
    }

    private void CheckCollision()
    {
        hit = Physics2D.Linecast(oldpos, myTransform.position, ~ignoreMask);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {
                enemy.Kill();
            }

            Instantiate(decal, hit.point, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
