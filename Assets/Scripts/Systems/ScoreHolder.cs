using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHolder : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    private int currentScore = 0;

    private void Awake()
    {
        AddScore(0);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        scoreText.text = currentScore.ToString();
    }
}
