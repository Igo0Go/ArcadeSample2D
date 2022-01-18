using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bonus : MonoBehaviour
{
    [Range(1,10)]
    public int score = 1;

    private ScoreHolder holder;

    private void Start()
    {
        holder = FindObjectOfType<ScoreHolder>();
    }

    public void TakeBonus()
    {
        holder.AddScore(score);
        Destroy(gameObject, Time.deltaTime);
    }
}
