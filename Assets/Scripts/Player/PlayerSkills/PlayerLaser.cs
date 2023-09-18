using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private bool useLaser;

    [SerializeField]
    private InputActionAsset starShipInputActionAsset;
    private InputActionMap playerActionMap;
    private InputAction laserShootAction;

    void Awake()
    {
        laserEnergyValue.maxValue = maxPower;
        laserEnergyValue.value = 100;

        playerActionMap = starShipInputActionAsset.FindActionMap("Player");

        laserShootAction = playerActionMap.FindAction("Laser");
        laserShootAction.started += context => SetActiveForLaser(true);
        laserShootAction.canceled += context => SetActiveForLaser(false);
    }

    private void OnEnable()
    {
        laserShootAction.Enable();
    }

    private void OnDisable()
    {
        laserShootAction.Disable();
    }

    private void Update()
    {
        if(useLaser && laserEnergyValue.value > 0)
        {
            EventCenter.ContextEvent.Invoke(ContextType.Laser);
            Ray();
        }
        else
        {
            targetPoint.position = transform.position;
        }
    }

    public void AddPower()
    {
        EventCenter.ShowContextEvent.Invoke(ContextType.Laser);
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

    private void SetActiveForLaser(bool value)
    {
        useLaser = value;
    }
}
