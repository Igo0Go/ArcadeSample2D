using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrackerBullet : Bullet
{
    private Transform target;


    void Start()
    {
        OnStart();
    }

    void Update()
    {
        if(target == null)
        {
            StopAllCoroutines();
            target = FindObjectOfType<BaseEnemy>().transform;
            StartCoroutine(CorrectDirectionCorroutine());
        }

        MoveBullet();
    }

    private IEnumerator CorrectDirectionCorroutine()
    {
        yield return new WaitForSeconds(0.3f);
        float t = 0;
        while(t < 1)
        {
            Vector3 directionTotarget = (target.position - myTransform.position).normalized;
            t += Time.deltaTime / 4;
            bulletDirection = Vector3.Lerp(bulletDirection, directionTotarget, t);
            myTransform.up = bulletDirection;
            yield return null;
        }
    }
}
