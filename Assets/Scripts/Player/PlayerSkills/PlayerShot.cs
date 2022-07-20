using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void AddBullets()
    {
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
        if (currentShotCount > 0 && Input.GetButton("LeftShot") && leftDelay == 0)
        {
            SpawnBullet(leftShootPoint);
            leftDelay = shotDelay;
        }
        else if (currentShotCount <1)
        {
            currentShotCount = 0;
        }
        
        if (currentShotCount > 0 && Input.GetButton("RightShot") && rightDelay == 0)
        {
            SpawnBullet(rightShootPoint);
            leftDelay = shotDelay;
        }
        else if (currentShotCount <1)
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

    private void SpawnBullet(Transform origin)
    {
        currentShotCount--;
        bulletCountText.text = currentShotCount.ToString();
        source.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Count)]);
        Instantiate(bulletPrefab, origin.position, origin.rotation);
    }
}