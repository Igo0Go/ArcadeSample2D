using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMoveControl : MonoBehaviour
{
    #region Доступные в редакторе поля

    [SerializeField]
    private InputActionAsset starShipInputActionAsset;
    private InputActionMap playerActionMap;
    private InputAction changeMoveAction;

    [SerializeField]
    [Range(1, 10)]
    [Tooltip("Скорость перемещения звездолёта игрока")]
    private float speed = 1;

    [Range(0,1)]
    [SerializeField]
    private float rotateDelta = 0.8f;

    [SerializeField]
    private bool useInertion = false;

    [SerializeField]
    [Tooltip("Включить режим отладки скрипта для этого объекта. " +
        "Будет отображаться вспомогательная отрисовка в окне сцены, а в консоль будут добаляться логи")]
    private bool debug = false;

    [SerializeField]
    private Transform xDebugPoint;
    [SerializeField]
    private Transform yDebugPoint;
    [SerializeField]
    private Transform normolizeDebugPoint;
    [SerializeField]
    private Transform debugOrigin;

    #endregion

    #region недоступные в редакторе публичные поля

    [HideInInspector]
    public Vector2 moveVector;

    public UnityEvent<float> rbVelocityChanged = new UnityEvent<float>();

    #endregion

    #region приватные поля

    private Rigidbody2D rb2D;
    private Transform myTransform;
    private float x, y;
    private const float inertionMultiplicator = 50;
    #endregion

    #region Обработка событий Unity
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        moveVector = Vector2.zero;
        useInertion = true;

        playerActionMap = starShipInputActionAsset.FindActionMap("Player");
        changeMoveAction = playerActionMap.FindAction("ChangeMove");
        changeMoveAction.performed += ChangeControl;
    }

    private void OnDestroy()
    {
        changeMoveAction.performed -= ChangeControl;
    }

    private void OnEnable()
    {
        changeMoveAction.Enable();
    }

    private void OnDisable()
    {
        changeMoveAction.Disable();
    }

    void Update()
    {
        Move();
    }
    #endregion

    private void OnMove(InputValue inputValue)
    {
        Vector2 inputVector = inputValue.Get<Vector2>();
        x = inputVector.x;
        y = inputVector.y;
    }

    private void Move()
    {
        if (x != 0 || y != 0)
        {
            EventCenter.ContextEvent.Invoke(ContextType.Movement);

            if(SettingsPack.useDebug)
            {
                x = Input.GetAxis("Horizontal");
                y = Input.GetAxis("Vertical");
            }

            moveVector = new Vector2(x, y);

            if(SettingsPack.useNormolize)
            {
                moveVector.Normalize();
            }

            DrawDebug();

            moveVector *= speed;
            moveVector *= GameTime.DeltaTime;

            if (useInertion)
            {
                rb2D.AddForce(moveVector * inertionMultiplicator);
                rbVelocityChanged?.Invoke(rb2D.velocity.magnitude);
            }
            else
            {
                rb2D.position += moveVector;
            }

            myTransform.up = Vector3.Lerp(myTransform.up, moveVector, rotateDelta);
        }
        else
        {
            DrawDebug();
        }
    }
    private void ChangeControl(InputAction.CallbackContext context)
    {
        useInertion = !useInertion;
        EventCenter.ContextEvent.Invoke(ContextType.MovingStyle);
        if (useInertion)
        {
            rb2D.velocity = moveVector;
        }
        else
        {
            rb2D.velocity = Vector2.zero;
            rbVelocityChanged?.Invoke(0);
        }
    }

    private void DrawDebug()
    {
        if (SettingsPack.useDebug)
        {
            debugOrigin.rotation = Quaternion.identity;
            xDebugPoint.localPosition = Vector3.right * moveVector.x;
            yDebugPoint.localPosition = Vector3.up * moveVector.y;
            normolizeDebugPoint.localPosition = moveVector;
        }
    }
}
