using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private List<WaveItem> waves;

    void Start()
    {
        StartCoroutine(WaveCorroutine());
    }

    private IEnumerator WaveCorroutine()
    {
        float time = 0;
        foreach (var wave in waves)
        {
            while(time < wave.timeThreshold)
            {
                time += GameTime.DeltaTime;
                yield return null;
            }
            wave.unityEvent?.Invoke();
        }
    }
}

[System.Serializable]
public class WaveItem
{
    [Min(0)]
    public float timeThreshold;
    public UnityEvent unityEvent;
}
