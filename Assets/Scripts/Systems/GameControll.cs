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

    [SerializeField]
    private AudioSource musicAudioSource;

    private void Awake()
    {
        deadPanel.SetActive(false);
        pausePanel.SetActive(false);
        GameTime.Pause = false;
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
                musicAudioSource.UnPause();
                GameTime.Pause = false;
            }
            else
            {
                pausePanel.SetActive(true);
                musicAudioSource.Pause();
                GameTime.Pause = true;
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
