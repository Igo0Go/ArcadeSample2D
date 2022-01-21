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

            if (debug)
            {
                Debug.Log("������� ������ �������� ������: " + moveVector + ". ��� �����: " + moveVector.magnitude);
            }


            moveVector *= speed;
            moveVector *= Time.deltaTime;

            rb2D.position += moveVector;
//            myTransform.up = moveVector;
            myTransform.up = Vector3.Lerp(myTransform.up, moveVector, 0.9f);
        }
    }
    #endregion

    #region ��������������� ��������� ������



    #endregion
}
