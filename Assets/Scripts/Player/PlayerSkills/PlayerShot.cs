using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerShot : MonoBehaviour
{


    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private List<AudioClip> shotSounds;

    [SerializeField] private float maxShotCount = 20;

    [SerializeField] [Range(0.25f, 2)] private float shotDelay = 0.4f;

    [SerializeField] private float currentShotCount = 0;

    [SerializeField] private Transform leftShootPoint;
    [SerializeField] private Transform rightShootPoint;
    [SerializeField] private Text bulletCountText;

    private AudioSource source;

    private float leftDelay, rightDelay;

    private bool shootLeft;
    private bool shootRight;

    [SerializeField]
    private InputActionAsset starShipInputActionAsset;
    private InputActionMap playerActionMap;
    private InputAction rightShootAction;
    private InputAction leftShootAction;

    void Awake()
    {
        playerActionMap = starShipInputActionAsset.FindActionMap("Player");

        rightShootAction = playerActionMap.FindAction("ShootRight");
        rightShootAction.started += context => StartShoot(false);
        rightShootAction.canceled += context => EndShoot(false);

        leftShootAction = playerActionMap.FindAction("ShootLeft");
        leftShootAction.started += context => StartShoot(true);
        leftShootAction.canceled += context => EndShoot(true);

        source = GetComponent<AudioSource>();
        leftDelay = rightDelay = 0;
    }

    void Update()
    {
        if (currentShotCount == 0)
        {
            return;
        }

        if (shootLeft)
        {
            if (currentShotCount > 0 && leftDelay == 0)
            {
                EventCenter.ContextEvent.Invoke(ContextType.Shot);
                SpawnBullet(leftShootPoint);
                leftDelay = shotDelay;
            }
        }

        if (shootRight)
        {
            if (currentShotCount > 0 && rightDelay == 0)
            {
                EventCenter.ContextEvent.Invoke(ContextType.Shot);
                SpawnBullet(rightShootPoint);
                rightDelay = shotDelay;
            }
        }

        if (currentShotCount < 1)
        {
            currentShotCount = 0;
        }

        if (leftDelay > 0)
        {
            leftDelay -= GameTime.DeltaTime;
        }
        else
        {
            leftDelay = 0;
        }

        if (rightDelay > 0)
        {
            rightDelay -= GameTime.DeltaTime;
        }
        else
        {
            rightDelay = 0;
        }
    }

    private void OnEnable()
    {
        rightShootAction.Enable();
        leftShootAction.Enable();
    }

    private void OnDisable()
    {
        rightShootAction.Disable();
        leftShootAction.Disable();
    }

    public void AddBullets()
    {
        EventCenter.ShowContextEvent.Invoke(ContextType.Shot);
        currentShotCount = maxShotCount;
        bulletCountText.text = currentShotCount.ToString();
    }

    private void StartShoot(bool left)
    {
        if(left)
        {
            shootLeft = true;
        }
        else
        {
            shootRight = true;
        }
    }

    private void EndShoot(bool left)
    {
        if (left)
        {
            shootLeft = false;
            leftDelay = 0;
        }
        else
        {
            shootRight = false;
            rightDelay = 0;
        }

    }

    private void SpawnBullet(Transform origin)
    {
        currentShotCount--;
        bulletCountText.text = currentShotCount.ToString();
        source.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Count)]);
        Instantiate(bulletPrefab, origin.position, origin.rotation);
    }
}