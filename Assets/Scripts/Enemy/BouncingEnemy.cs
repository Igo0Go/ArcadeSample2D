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

    protected Transform player;
    protected Transform myTransform;
    protected Rigidbody2D rb;

    private Vector2 moveVector;
    private RaycastHit2D hit2D;
    private float currentSpeed;
    private const float stopSpeed = 1.5f;


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
                t -= Time.deltaTime;
                moveVector = (player.position - myTransform.position).normalized;
                myTransform.up = moveVector;
                yield return null;
            }
            attackSygnal.SetActive(true);
            currentSpeed = attackSpeed;
            yield return new WaitForSeconds(attackSygnalTime);

            while(currentSpeed > stopSpeed)
            {
                rb.position += moveVector * currentSpeed * Time.deltaTime;

                hit2D = Physics2D.Raycast(myTransform.position, moveVector, 0.4f, ~ignoreMask);

                if (hit2D.collider != null)
                {
                    currentSpeed /= 2;
                    moveVector = Vector2.Reflect(moveVector, hit2D.normal);
                    myTransform.up = moveVector;
                }
                yield return null;
            }
        }
    }
}
