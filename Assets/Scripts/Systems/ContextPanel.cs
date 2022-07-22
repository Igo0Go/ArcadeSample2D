using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContextPanel : MonoBehaviour
{
    private Transform myTransform;
    
    
    public Transform PlayerTransform;
    public ContextType Type;
    public float DeathDelay = 1.5f;

    void Start()
    {
        myTransform = transform;
        EventCenter.ContextEvent.AddListener(OnContextEvent);
    }

    void Update()
    {
        if (PlayerTransform != null)
        {
            myTransform.position = PlayerTransform.position;
        }
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
    public static UnityEvent<ContextType> ContextEvent;

    static EventCenter()
    {
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