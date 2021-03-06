using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, ISpawnObject
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

    public virtual void Prepare(Spawner spawner, ScoreHolder scoreHolder)
    {
        this.spawner = spawner;
        this.scoreHolder = scoreHolder;
    }

    public void Kill()
    {
        spawner.SpawnNextEnemy();
        DestroyThis();
    }
    public void DestroyThis()
    {
        spawner.PlaySound(destroySound);
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
        else if(collision.CompareTag("DeadWave"))
        {
            DestroyThis();
        }
    }
}

public interface ISpawnObject
{
    void Prepare(Spawner spawner, ScoreHolder scoreHolder);
}
