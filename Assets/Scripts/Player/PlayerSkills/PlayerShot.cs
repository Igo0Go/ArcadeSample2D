using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerShot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private List<AudioClip> shotSounds;

    [SerializeField] private float maxShotCount = 20;

    [SerializeField] [Range(0.01f, 2)] private float shotDelay = 0.4f;

    [SerializeField] private float currentShotCount = 0;

    [SerializeField] private Transform leftShootPoint;
    [SerializeField] private Transform rightShootPoint;
    [SerializeField] private Transform contextPanel;

    [SerializeField] private Text bulletCountText;

    private AudioSource source;

    private float leftDelay, rightDelay;

    

    public void AddBullets()
    {
        if (contextPanel!=null)
        {
            contextPanel.gameObject.SetActive(true);
        }
        currentShotCount = maxShotCount;
        bulletCountText.text = currentShotCount.ToString();
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        leftDelay = rightDelay = 0;
    }

    void Update()
    {
        Fire();
        //if (currentShotCount > 0 && Input.GetButton("LeftShot") && leftDelay == 0)
        //{

        //    EventCenter.ContextEvent.Invoke(ContextType.Shot);
        //    SpawnBullet(leftShootPoint);
        //    leftDelay = shotDelay;
        //}
        //else if (currentShotCount <1)
        //{
        //    currentShotCount = 0;
        //}

        //if (currentShotCount > 0 && Input.GetButton("RightShot") && rightDelay == 0)
        //{

        //    EventCenter.ContextEvent.Invoke(ContextType.Shot);
        //    SpawnBullet(rightShootPoint);
        //    rightDelay = shotDelay;
        //}
        //else if (currentShotCount <1)
        //{
        //    currentShotCount = 0;
        //}

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
    public void Fire()
    {
        if (currentShotCount > 0 && fire && leftDelay == 0)
        {
            if (lfire)
            {
                lfire = false;
                EventCenter.ContextEvent.Invoke(ContextType.Shot);
                SpawnBullet(leftShootPoint);
            }
            else
            {
                lfire = true;
                EventCenter.ContextEvent.Invoke(ContextType.Shot);
                SpawnBullet(rightShootPoint);
            }
            leftDelay = shotDelay;


        }
        else if (currentShotCount < 1)
        {
            currentShotCount = 0;
        }
    }

    private bool fire = false;
    private bool lfire = false;
    public void OnFire1(InputAction.CallbackContext value)
    {
        fire = value.ReadValueAsButton();
    }
    private void SpawnBullet(Transform origin)
    {
        currentShotCount--;
        bulletCountText.text = currentShotCount.ToString();
        source.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Count)]);
        Instantiate(bulletPrefab, origin.position, origin.rotation);
    }
}