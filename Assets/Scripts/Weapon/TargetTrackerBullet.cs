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
                var targets = FindObjectsOfType<BaseEnemy>();

                if(targets.Length > 0)
                {
                    target = targets[0].transform;
                    float distance = Vector3.Distance(target.position, target.position);
                    for (int i = 1; i < targets.Length; i++)
                    {
                        if (Vector3.Distance(target.position, targets[i].transform.position) < distance)
                        {
                            target = targets[i].transform;
                            distance = Vector3.Distance(target.position, target.position);
                        }
                    }
                    yield return null;
                }
            }
            float t = 0;
            while (target != null)
            {
                Vector3 directionTotarget = (target.position - myTransform.position).normalized;
                if(t < 1)
                {
                    t += GameTime.DeltaTime / 4;
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
