using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameControll : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameObject pausePanel; 
    
    [SerializeField]
    private GameObject nighTalesButton;

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

    [SerializeField]
    private InputActionAsset starShipInputActionAsset;
    private InputActionMap playerActionMap;
    private InputAction menuToggleAction;

    [SerializeField]
    private GameObject debugPanel;
    [SerializeField]
    private GameObject playerDebug;

    [SerializeField]
    private Toggle debugToggle;

    [SerializeField]
    private Toggle normolizeToggle;

    void Awake()
    {
        deadMarker.SetActive(false);
        winMarker.SetActive(false);
        finalPanel.SetActive(false);
        pausePanel.SetActive(false);
        nighTalesButton.SetActive(false);
        GameTime.Pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        debugToggle.isOn = SettingsPack.useDebug;
        playerDebug.SetActive(SettingsPack.useDebug);
        debugPanel.SetActive(SettingsPack.useDebug);
        normolizeToggle.isOn = SettingsPack.useNormolize;


        playerActionMap = starShipInputActionAsset.FindActionMap("Player");

        menuToggleAction = playerActionMap.FindAction("Menu");
        menuToggleAction.performed += PausePanelToggle;
    }

    private void OnEnable()
    {
        menuToggleAction.Enable();
    }

    private void OnDisable()
    {
        menuToggleAction.Disable();
    }

    private void OnDestroy()
    {
        menuToggleAction.performed -= PausePanelToggle;
    }

    public void ShowFinalPanel(int score, bool dead)
    {
        scoreText.text = score.ToString();
        finalPanel.SetActive(true);
        nighTalesButton.SetActive(true);
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PausePanelToggle(InputAction.CallbackContext context)
    {
        if(!finalPanel.activeSelf)
        {
            if(pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                nighTalesButton.SetActive(false);
                musicAudioSource.UnPause();
                GameTime.Pause = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                pausePanel.SetActive(true);
                nighTalesButton.SetActive(true);
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
        SceneManager.LoadScene(0);
    }

    private void SetSelectedUI(GameObject UIgameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(UIgameObject);
    }

    public void OnChangeUseDebug()
    {
        SettingsPack.useDebug = debugToggle.isOn;
        playerDebug.SetActive(debugToggle.isOn);
        debugPanel.SetActive(debugToggle.isOn);
    }

    public void OnChangeUseNormolize()
    {
        SettingsPack.useNormolize = normolizeToggle.isOn;
    }
}

public static class SettingsPack
{
    public static TooltipType Tooltip = TooltipType.Keyboard;

    public static bool useDebug = false;

    public static bool useNormolize = false;
}
