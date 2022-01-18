using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{

    #region Доступные в редакторе поля

    [SerializeField]                                            //Отрисовать в редакторе
    [Range(1, 10)]                                               //Сделать ползунок
    [Tooltip("Скорость перемещения звездолёта игрока")]         //Сделать всплывающую подсказку
    private float speed = 1;


    [SerializeField]
    [Tooltip("Включить режим отладки скрипта для этого объекта. Будет отображаться вспомогательная отрисовка в окне сцены, а в консоль будут добаляться логи")]
    private bool debug = false;

    #endregion

    #region недоступные в редакторе публичные поля

    [HideInInspector]
    public Vector3 moveVector;

    #endregion

    #region приватные поля

    private Transform myTransform;
    private float x, y;

    #endregion


    #region Обработка событий Unity
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
                Debug.Log("Текуший вектор движения игрока: " + moveVector + ". Его длина: " + moveVector.magnitude);
            }


            moveVector *= speed;
            moveVector *= Time.deltaTime;

            myTransform.position += moveVector ;
            myTransform.up = moveVector;
        }
    }
    #endregion

    #region Вспомогательные приватные методы



    #endregion
}
