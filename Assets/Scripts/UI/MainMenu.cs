using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private List<Button> mainButtons;
    [SerializeField]
    private List<Button> toolTipButtons;
    [SerializeField]
    private GameObject toolTipPanel;
    [SerializeField]
    private int playScene = 1;

    void Awake()
    {
        mainButtons[0].Select();
        toolTipPanel.SetActive(false);
    }

    public void Play() => SceneManager.LoadScene(playScene);

    public void SetActiveForTipPanel(bool value)
    {
        toolTipPanel.SetActive(value);

        if (value )
        {
            toolTipButtons[(int)SettingsPack.Tooltip].Select();
        }
        else
        {
            mainButtons[0].Select();
        }
    }

    public void Exit() => Application.Quit();

    public void SetToolTipType(int type)
    {
        SettingsPack.Tooltip = (TooltipType)type;
    }
}
