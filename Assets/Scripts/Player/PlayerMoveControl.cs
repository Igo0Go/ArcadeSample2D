using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveControl : MonoBehaviour
{
    #region ��������� � ��������� ����

    [SerializeField]                                            //���������� � ���������
    [Range(1, 10)]                                               //������� ��������
    [Tooltip("�������� ����������� ��������� ������")]         //������� ����������� ���������
    private float speed = 1;

    [Range(0, 1)]
    [SerializeField]
    private float rotateDelta = 0.8f;

    [SerializeField]
    private bool useInertion = false;

    [SerializeField]
    [Tooltip("�������� ����� ������� ������� ��� ����� �������. ����� ������������ ��������������� ��������� � ���� �����, � � ������� ����� ���������� ����")]
    private bool debug = false;

    #endregion

    #region ����������� � ��������� ��������� ����

    [HideInInspector]
    private Vector2 moveVector;

    public UnityEvent<float> rbVelocityChanged = new UnityEvent<float>();

    #endregion

    #region ��������� ����

    private Rigidbody2D rb2D;
    private PlayerInput playerInput; 
    private Transform myTransform;
    private float x, y;
    private const float inertionMultiplicator = 50;

    Vector3 debugVector;

    #endregion

    #region ��������� ������� Unity
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        myTransform = transform;
        moveVector = Vector2.zero;
        useInertion = true;
    }

    void Update()
    {
        Move();
        //ChangeControl();
    }
    #endregion

    private void Move()
    {
        //x = Input.GetAxis("Horizontal");
        //y = Input.GetAxis("Vertical");
       // moveVector = playerInput.actions.actionMaps[0]. .ReadValue<Vector2>();

        //if (x != 0 || y != 0)
        if (moveVector.magnitude > 0)
        {
            EventCenter.ContextEvent.Invoke(ContextType.Movement);
            //moveVector = new Vector2(x, y);
            //moveVector.Normalize();

            if (debug)
            {
                debugVector = moveVector;
                Debug.Log("������� ������ �������� ������: " + debugVector +
                    ". ��� �����: " + debugVector.magnitude);
            }

            //moveVector *= speed;
            //moveVector *= GameTime.DeltaTime;

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
            debugVector = Vector3.zero;
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        moveVector *= speed;
        moveVector *= GameTime.DeltaTime;
    }
    public void OnSwitch(InputAction.CallbackContext value)
    {

        if (value.ReadValueAsButton())
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
    }
    private void ChangeControl()
    {
        if (Input.GetButtonDown("ChangeControl"))
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
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 1);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector3.up * debugVector.y);
            Gizmos.DrawSphere(transform.position + Vector3.up * debugVector.y, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.right * debugVector.x);
            Gizmos.DrawSphere(transform.position + Vector3.right * debugVector.x, 0.1f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, moveVector);
            Gizmos.DrawSphere(transform.position + debugVector, 0.1f);
        }
    }
}
