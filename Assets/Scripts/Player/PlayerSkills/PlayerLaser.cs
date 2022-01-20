using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerLaser : MonoBehaviour
{

    [SerializeField]
    private Slider laserEnergyValue;

    [Min(0)]
    [SerializeField]
    private float maxPower = 5;

    [SerializeField]
    private LayerMask ignoreMask;

    [SerializeField]
    private GameObject decal;

    [SerializeField]
    private Transform targetPoint;

    private RaycastHit2D hit;

    private void Start()
    {
        laserEnergyValue.maxValue = maxPower;
        laserEnergyValue.value = 0;
    }

    private void Update()
    {
        if(Input.GetButton("Laser") && laserEnergyValue.value > 0)
        {
            Ray();
        }
        else
        {
            targetPoint.position = transform.position;
        }
    }

    public void AddPower()
    {
        laserEnergyValue.value = laserEnergyValue.maxValue;
    }

    private void Ray()
    {
        hit = Physics2D.Raycast(transform.position, transform.up, 100, ~ignoreMask);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.Kill();
            }
            laserEnergyValue.value -= Time.deltaTime;
            targetPoint.position = hit.point;
            Instantiate(decal, hit.point, Quaternion.identity);
        }
        else
        {
            targetPoint.position = transform.position + transform.up * 30;
        }
    }
}
