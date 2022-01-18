using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 moveVector;

    #endregion

    #region ��������� ����

    private Transform myTransform;
    private float x, y;

    #endregion


    #region ��������� ������� Unity
    void Start()
    {
        myTransform = transform;
        moveVector = Vector3.zero;
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if(x != 0 || y != 0)
        {
            //moveVector = Vector2.up * y + Vector2.right * x;

            moveVector = new Vector3(x,y,0);
            //moveVector.Normalize();

            if (debug)
            {
                Debug.Log("������� ������ �������� ������: " + moveVector + ". ��� �����: " + moveVector.magnitude);
            }


            moveVector *= speed;
            moveVector *= Time.deltaTime;

            myTransform.position += moveVector ;
            myTransform.up = moveVector;
        }
    }
    #endregion

    #region ��������������� ��������� ������



    #endregion
}
