using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : BaseEnemy
{
    [SerializeField]
    [Range(1, 10)]
    private float attackSpeed = 5;

    [SerializeField]
    private GameObject attackSygnal;

    [SerializeField]
    private LayerMask ignoreMask;

    [SerializeField]
    [Range(0.1f, 3)]
    private float attackPrepareTime = 1;
    [SerializeField]
    [Range(0.1f, 1)]
    private float attackSygnalTime = 1;

    [SerializeField]
    private bool debug;

    protected Transform player;
    protected Transform myTransform;
    protected Rigidbody2D rb;

    private Vector2 moveVector;
    private RaycastHit2D hit2D;
    private float currentSpeed;
    private const float stopSpeed = 1.5f;
    Vector3 debugVector;
    Vector3 debugtargetPos;

    public override void PrepareEnemy(Spawner spawner, ScoreHolder scoreHolder)
    {
        attackSygnal.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
        player = spawner.playerStarShip;
        base.PrepareEnemy(spawner, scoreHolder);
        StartCoroutine(MoveToPlayerCoroutine());
    }

    private IEnumerator MoveToPlayerCoroutine()
    {
        while(player != null)
        {
            float t = attackPrepareTime;

            while (t > 0)
            {
                t -= GameTime.DeltaTime;
                moveVector = (player.position - myTransform.position);
                debugVector = moveVector;
                moveVector.Normalize();
                myTransform.up = moveVector;
                debugtargetPos = player.position;
                yield return null;
            }
            attackSygnal.SetActive(true);
            currentSpeed = attackSpeed;

            debugVector = debugVector.normalized * currentSpeed;

            yield return new WaitForSeconds(attackSygnalTime);

            while(currentSpeed > stopSpeed)
            {
                rb.position += moveVector * currentSpeed * GameTime.DeltaTime;

                hit2D = Physics2D.Raycast(myTransform.position, moveVector, 0.4f, ~ignoreMask);

                if (hit2D.collider != null)
                {
                    currentSpeed /= 2;
                    moveVector = Vector2.Reflect(moveVector, hit2D.normal);
                    debugVector = moveVector.normalized * currentSpeed;
                    myTransform.up = moveVector;
                }
                yield return null;
            }

            currentSpeed = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (debug && player != null)
        {
            Gizmos.color = Color.red;

            if(currentSpeed > 0)
            {
                Gizmos.DrawSphere(transform.position + debugVector.normalized * stopSpeed, 0.2f);
            }
            Gizmos.DrawLine(transform.position, transform.position + debugVector);
            Gizmos.DrawWireSphere(debugtargetPos, 0.5f);
        }
    }
}
