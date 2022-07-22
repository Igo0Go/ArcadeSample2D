using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    [SerializeField]
    private Transform contextPanel;
    
    private AudioSource source;

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
    }

    void Update()
    {
        if (Input.GetButtonDown("TargetTrackerShot") && currentShotCount > 0)
        {
            
            EventCenter.ContextEvent.Invoke(ContextType.TargetShot);
            SpawnBullet(transform);
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
