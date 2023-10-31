using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(AudioSource))]
public class PlayerTargetTrackerShot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private List<AudioClip> shotSounds;

    [SerializeField]
    private float maxShotCount = 5;

    [SerializeField]
    private float currentShotCount = 0;

    [SerializeField]
    private Text bulletCountText;

    private AudioSource source;

    public void AddBullets()
    {
        EventCenter.ShowContextEvent.Invoke(ContextType.TargetShot);
        currentShotCount = maxShotCount;
        bulletCountText.text = currentShotCount.ToString();
    }

    [SerializeField]
    private InputActionAsset starShipInputActionAsset;
    private InputActionMap playerActionMap;
    private InputAction targetTrackerShootAction;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        playerActionMap = starShipInputActionAsset.FindActionMap("Player");

        targetTrackerShootAction = playerActionMap.FindAction("TargetTrackingShoot");
        targetTrackerShootAction.performed += context => SpawnBullet(transform);
    }

    private void OnEnable()
    {
        targetTrackerShootAction.Enable();
    }

    private void OnDisable()
    {
        targetTrackerShootAction.Disable();
    }

    private void SpawnBullet(Transform origin)
    {
        if (currentShotCount > 0)
        {
            if(vibrationRate == 0)
            {
                StartCoroutine(RumbleCoroutine());
            }
            vibrationRate = 0.3f;

            EventCenter.ContextEvent.Invoke(ContextType.TargetShot);
            currentShotCount--;
            bulletCountText.text = currentShotCount.ToString();
            source.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Count)]);
            Instantiate(bulletPrefab, origin.position, origin.rotation);
        }
    }

    private float vibrationRate = 0;
    private IEnumerator RumbleCoroutine()
    {
        vibrationRate = 0.3f;

        while (vibrationRate > 0)
        {
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(vibrationRate, vibrationRate);
            }
            vibrationRate -= Time.deltaTime;
            yield return null;
        }
        vibrationRate = 0;
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(vibrationRate, vibrationRate);
        }

    }
}
