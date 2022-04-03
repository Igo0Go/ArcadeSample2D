using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : BaseEnemy
{
    [SerializeField]
    [Range(1,5)]
    private float speed = 2;

    [SerializeField]
    private bool debug;

    protected Transform player;
    protected Transform myTransform;
    protected Rigidbody2D rb;

    public override void Prepare(Spawner spawner, ScoreHolder scoreHolder)
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
        base.Prepare(spawner, scoreHolder);
        player = spawner.playerStarShip;
        StartCoroutine(MoveToPlayerCoroutine());
    }

    private IEnumerator MoveToPlayerCoroutine()
    {
        while(player != null)
        {
            try
            {
                Vector2 direction = (player.position - myTransform.position).normalized;
                myTransform.up = direction;
                rb.position += direction * speed * GameTime.DeltaTime;
            }
            catch (System.NullReferenceException)
            {
                break;
            }
            catch (MissingReferenceException)
            {
                break;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(debug && player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, 0.5f);   
        }
    }
}
