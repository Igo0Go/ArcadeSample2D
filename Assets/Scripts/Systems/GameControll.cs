using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControll : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject deadPanel;

    [SerializeField]
    private AudioSource deadAudioSource;

    private void Awake()
    {
        deadPanel.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        PausePanelToggle();
    }

    public void ShowDeadPanel(int score)
    {
        deadAudioSource.Play();
        scoreText.text = score.ToString();
        deadPanel.SetActive(true);
    }

    private void PausePanelToggle()
    {
        if(Input.GetButtonDown("Cancel") && !deadPanel.activeSelf)
        {
            if(pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
