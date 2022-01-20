using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    [Range(1,10)]
    private int score = 1;

    [SerializeField]
    private AudioClip destroySound;

    private Spawner spawner;
    private ScoreHolder scoreHolder;

    [SerializeField]
    private GameObject decal;

    public void PrepareEnemy(Spawner spawner, ScoreHolder scoreHolder)
    {
        this.spawner = spawner;
        this.scoreHolder = scoreHolder;
    }

    public void Kill()
    {
        spawner.PlaySound(destroySound);
        spawner.SpawnNextEnemy();
        scoreHolder.AddScore(score);
        Instantiate(decal, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void FindPlayer()
    {
        scoreHolder.PrepareScoreForDeadPanel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            Kill();
        }
    }
}
