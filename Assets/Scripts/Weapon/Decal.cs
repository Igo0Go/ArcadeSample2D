using UnityEngine;

public class Decal : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private float timeForDestroy = 1;

    void Start()
    {
        Destroy(gameObject, timeForDestroy);
    }
}
