using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [SerializeField]
    private GameObject startPausePanelSelectedObject;

    [SerializeField]
    private GameObject startDeadPanelSelectedObject;

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
        SetSelectedUI(startDeadPanelSelectedObject);
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
                SetSelectedUI(startPausePanelSelectedObject);
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

    private void SetSelectedUI(GameObject UIgameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(UIgameObject);
    }
}
