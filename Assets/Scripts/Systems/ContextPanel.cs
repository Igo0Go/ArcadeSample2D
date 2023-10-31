using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContextPanel : MonoBehaviour
{
    public Transform PlayerTransform;
    public ContextType Type;
    public float DeathDelay = 1.5f;

    [SerializeField]
    private List<GameObject> tooltipObjects;

    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
        EventCenter.ShowContextEvent.AddListener(OnShowContextEvent);
        EventCenter.ContextEvent.AddListener(OnContextEvent);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (PlayerTransform != null)
        {
            myTransform.position = PlayerTransform.position;
        }
    }

    public void OnShowContextEvent(ContextType contextType)
    {
        if (contextType == Type)
        {
            ShowContext();
        }
    }

    public void ShowContext()
    {
        gameObject.SetActive(true);
        tooltipObjects[(int)SettingsPack.Tooltip].SetActive(true);
    }

    public void OnContextEvent(ContextType contextType)
    {
        if (contextType == Type)
        {
            Destroy(gameObject, DeathDelay);
        }
    }
}

public static class EventCenter
{
    public static UnityEvent<ContextType> ShowContextEvent;
    public static UnityEvent<ContextType> ContextEvent;

    static EventCenter()
    {
        ShowContextEvent = new UnityEvent<ContextType>();
        ContextEvent = new UnityEvent<ContextType>();
    }
}

public enum ContextType
{
    MovingStyle,
    Movement,
    Laser,
    Shot,
    TargetShot,
}

public enum TooltipType
{
    PS = 0,
    XBOX = 1,
    Keyboard = 2
}