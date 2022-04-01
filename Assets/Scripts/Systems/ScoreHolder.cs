using UnityEngine;
using UnityEngine.UI;

public class ScoreHolder : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameControll gameControll;

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

    public void PrepareScoreForDeadPanel()
    {
        gameControll.ShowFinalPanel(currentScore, true);
    }

    public void PrepareScoreForWinPanel()
    {
        gameControll.ShowFinalPanel(currentScore, false);
    }
}
