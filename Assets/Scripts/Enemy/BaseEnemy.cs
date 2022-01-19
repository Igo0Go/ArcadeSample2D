using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    [Range(1,10)]
    private int score = 1;

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
        spawner.SpawnNextEnemy();
        scoreHolder.AddScore(score);
        Instantiate(decal, transform.position, Quaternion.identity);
        Destroy(gameObject, Time.deltaTime);
    }
}
