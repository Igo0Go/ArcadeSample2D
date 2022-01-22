using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : BaseEnemy
{
    [SerializeField]
    [Range(1, 5)]
    private float speed = 2;

    [SerializeField]
    private bool debug;

    protected Transform player;
    protected Transform myTransform;
    protected Rigidbody2D rb;

    private Vector3 target;

    public override void PrepareEnemy(Spawner spawner, ScoreHolder scoreHolder)
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
        player = spawner.playerStarShip;
        base.PrepareEnemy(spawner, scoreHolder);
        StartCoroutine(MoveToPlayerCoroutine());
    }

    private IEnumerator MoveToPlayerCoroutine()
    {
        while (player != null)
        {
            float distance = Vector3.Distance(player.position, myTransform.position);
            target = distance > 5? player.position + player.up * 5 : player.position + player.up * distance;

            if(Vector3.Distance(myTransform.position, target) < 1.5)
            {
                target = player.position;
            }

            Vector2 direction = (target - myTransform.position).normalized;
            myTransform.up = direction;
            rb.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (debug && player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(player.position, target);
            Gizmos.DrawWireSphere(target, 0.5f);
        }
    }
}
