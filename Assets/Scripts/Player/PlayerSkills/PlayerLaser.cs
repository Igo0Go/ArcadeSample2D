using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    [SerializeField]
    private Transform contextPanel;

    private RaycastHit2D hit;

    private bool lazerOn = false;

    private void Start()
    {
        laserEnergyValue.maxValue = maxPower;
        laserEnergyValue.value = 0;
    }

    private void Update()
    {
        if (lazerOn && laserEnergyValue.value > 0)
        {
            EventCenter.ContextEvent.Invoke(ContextType.Laser);
            Ray();
        }
        else
        {
            targetPoint.position = transform.position;
        }
    }

    public void OnFire3(InputAction.CallbackContext value)
    {
        lazerOn = value.ReadValueAsButton();
    }


    public void AddPower()
    {
        if (contextPanel != null)
        {
            contextPanel.gameObject.SetActive(true);
        }
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
            laserEnergyValue.value -= GameTime.DeltaTime;
            targetPoint.position = hit.point;
            Instantiate(decal, hit.point, Quaternion.identity);
        }
        else
        {
            targetPoint.position = transform.position + transform.up * 30;
        }
    }
}
