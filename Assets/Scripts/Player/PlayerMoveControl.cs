using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveControl : MonoBehaviour
{

    #region ��������� � ��������� ����

    [SerializeField]                                            //���������� � ���������
    [Range(1, 10)]                                               //������� ��������
    [Tooltip("�������� ����������� ��������� ������")]         //������� ����������� ���������
    private float speed = 1;


    [SerializeField]
    [Tooltip("�������� ����� ������� ������� ��� ����� �������. ����� ������������ ��������������� ��������� � ���� �����, � � ������� ����� ���������� ����")]
    private bool debug = false;

    #endregion

    #region ����������� � ��������� ��������� ����

    [HideInInspector]
    public Vector2 moveVector;

    #endregion

    #region ��������� ����

    private Rigidbody2D rb2D;
    private Transform myTransform;
    private float x, y;

    Vector3 debugVector;

    #endregion


    #region ��������� ������� Unity
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        moveVector = Vector2.zero;
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if(x != 0 || y != 0)
        {
            moveVector = new Vector2(x,y);
            moveVector.Normalize();

            debugVector = moveVector;

            if (debug)
            {
                Debug.Log("������� ������ �������� ������: " + moveVector + ". ��� �����: " + moveVector.magnitude);
            }


            moveVector *= speed;
            moveVector *= Time.deltaTime;

            rb2D.position += moveVector;
//            myTransform.up = moveVector;
            myTransform.up = Vector3.Lerp(myTransform.up, moveVector, 0.8f);
        }
        else
        {
            debugVector = Vector3.zero;
        }
    }
    #endregion


    private void OnDrawGizmos()
    {
        if(debug)
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
