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
    private GameObject finalPanel;

    [SerializeField]
    private GameObject deadMarker;

    [SerializeField]
    private GameObject winMarker;

    [SerializeField]
    private AudioSource deadAudioSource;

    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private GameObject startPausePanelSelectedObject;

    [SerializeField]
    private GameObject startDeadPanelSelectedObject;

    [SerializeField]
    private WaveSystem waveSystem;

    private void Awake()
    {
        deadMarker.SetActive(false);
        winMarker.SetActive(false);
        finalPanel.SetActive(false);
        pausePanel.SetActive(false);
        GameTime.Pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PausePanelToggle();
    }

    public void ShowFinalPanel(int score, bool dead)
    {
        scoreText.text = score.ToString();
        finalPanel.SetActive(true);
        if(dead)
        {
            deadAudioSource.Play();
            deadMarker.SetActive(true);
            waveSystem?.StopWorkAndDelete();
        }
        else
        {
            winMarker.SetActive(true);
            GameTime.Pause = true;
        }
        SetSelectedUI(startDeadPanelSelectedObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PausePanelToggle()
    {
        if(Input.GetButtonDown("Cancel") && !finalPanel.activeSelf)
        {
            if(pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                musicAudioSource.UnPause();
                GameTime.Pause = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                pausePanel.SetActive(true);
                SetSelectedUI(startPausePanelSelectedObject);
                musicAudioSource.Pause();
                GameTime.Pause = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
