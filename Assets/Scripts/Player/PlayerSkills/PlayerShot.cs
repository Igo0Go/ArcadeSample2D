using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerShot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private List<AudioClip> shotSounds;

    [SerializeField]
    private float maxShotCount = 20;

    [SerializeField]
    [Range(0.25f, 2)]
    private float shotDelay = 0.4f;

    [SerializeField]
    private float currentShotCount = 0;

    [SerializeField]
    private Transform leftShootPoint;
    [SerializeField]
    private Transform rightShootPoint;

    [SerializeField]
    private Text bulletCountText;

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
        if(currentShotCount > 0)
        {
            if (Input.GetButton("Fire1") && leftDelay == 0)
            {
                SpawnBullet(leftShootPoint);
                leftDelay = shotDelay;
            }

            if (Input.GetButton("Fire2") && rightDelay == 0)
            {
                SpawnBullet(rightShootPoint);
                rightDelay = shotDelay;
            }
        }

        if(leftDelay > 0)
        {
            leftDelay -= Time.deltaTime;
        }
        else
        {
            leftDelay = 0;
        }

        if (rightDelay > 0)
        {
            rightDelay -= Time.deltaTime;
        }
        else
        {
            rightDelay = 0;
        }
    }

    private IEnumerator ShotCorroutine(Transform ShootPoint)
    {
        while(currentShotCount > 0)
        {
            
            yield return new WaitForSeconds(shotDelay);
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
