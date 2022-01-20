using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bonus : MonoBehaviour
{
    [Range(1,10)]
    public int score = 1;

    public BonusType type;

    public AudioClip bonusClip;

    private ScoreHolder holder;
    private Spawner gameplaySpawner;

    public void PrepareBonus(ScoreHolder scoreHolder, Spawner spawner)
    {
        holder = scoreHolder;
        gameplaySpawner = spawner;
    }

    public void TakeBonus()
    {
        holder.AddScore(score);
        gameplaySpawner.source.PlayOneShot(bonusClip);
        gameplaySpawner.SpawnNextBonus();
        Destroy(gameObject);
    }
}

public enum BonusType
{
    ScoreOnly,
    Shot,
    TargetTracker,
    Laser,
    Shield
}