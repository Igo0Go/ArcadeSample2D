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

    protected override IEnumerator MoveBulletCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);

            while (target == null)
            {
                target = FindObjectOfType<BaseEnemy>().transform;
                yield return null;
            }
            float t = 0;
            while (target != null)
            {
                Vector3 directionTotarget = (target.position - myTransform.position).normalized;
                if(t < 1)
                {
                    t += Time.deltaTime / 4;
                }
                bulletDirection = Vector3.Lerp(bulletDirection, directionTotarget, t);
                myTransform.up = bulletDirection;
                yield return null;
                oldpos = myTransform.position;
                myTransform.position += bulletDirection * bulletSpeed * Time.deltaTime;
                CheckCollision();
            }
        }
    }
}
