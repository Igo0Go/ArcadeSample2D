using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject deadWavePrefab;
    [SerializeField]
    private List<SpawnItem> bonusesForSpawn;
    [SerializeField]
    private List<SpawnItem> enemiesForSpawn;

    public Transform playerStarShip;

    [SerializeField]
    private ScoreHolder scoreHolder;

    [SerializeField]
    private SpawnArea spawnArea;

    [HideInInspector]
    public AudioSource source;

    [SerializeField]
    private Transform bonusesContainer;

    [SerializeField]
    private Transform enemiesContainer;

    [SerializeField]
    private bool debug;

    private int totalBonusWeight;
    private int totalEnemyWeight;

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        spawnArea.Prepare();

        foreach (var item in bonusesForSpawn)
        {
            totalBonusWeight += item.weight;
        }
        foreach (var item in enemiesForSpawn)
        {
            totalEnemyWeight += item.weight;
        }
    }

    public void SpawnDeadWave()
    {
        StopAllCoroutines();
        if(playerStarShip != null)
        {
            Instantiate(deadWavePrefab, playerStarShip.position, Quaternion.identity);
        }
    }

    public void SpawnConcreteBonus(int numberInList)
    {
        if (playerStarShip != null)
        {
            StartCoroutine(SpawnObjectCorroutine(playerStarShip.position, bonusesForSpawn[numberInList].spawnOhbject,
                bonusesContainer));
        }
    }

    public void SpawnConcreteEnemy(int numberInList)
    {
        if (playerStarShip != null)
        {
            StartCoroutine(SpawnObjectCorroutine(playerStarShip.position, enemiesForSpawn[numberInList].spawnOhbject,
                bonusesContainer));
        }
    }

    public void SpawnNextBonus()
    {
        if (playerStarShip != null)
        {
            StartCoroutine(SpawnObjectCorroutine(playerStarShip.position, GetRandomObject(bonusesForSpawn, totalBonusWeight),
                bonusesContainer));
        }
    }

    public void SpawnNextEnemy()
    {
        if (playerStarShip != null)
        {
            StartCoroutine(SpawnObjectCorroutine(playerStarShip.position, GetRandomObject(enemiesForSpawn, totalEnemyWeight),
                bonusesContainer));
        }
    }

    public void UnBlockBonus(int numberInList)
    {
        bonusesForSpawn[numberInList].blocked = false;
    }
    public void UnBlockEnemy(int numberInList)
    {
        enemiesForSpawn[numberInList].blocked = false;
    }

    [ContextMenu("Тесттовый спавн бонусов")]
    public void TestSpawn()
    {
        StartCoroutine(TestSpawnCoroutine());
    }

    public GameObject GetRandomObject(List<SpawnItem> items, int totalweight)
    {
        int bufer = Random.Range(0, totalweight);
        int currentThreshold = 0;

        foreach (var item in items)
        {
            currentThreshold += item.weight;
            if (currentThreshold > bufer)
            {
                if(debug)
                {
                    Debug.Log(item.spawnOhbject);
                }
                if(item.blocked)
                {
                    break;
                }
                return item.spawnOhbject;
            }
        }
        if (debug)
        {
            Debug.Log(items[0].spawnOhbject);
        }
        return items[0].spawnOhbject;

    }

    private IEnumerator TestSpawnCoroutine()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnNextBonus();
            yield return null;
        }
    }

    private IEnumerator SpawnObjectCorroutine(Vector3 playerPoint, GameObject spawnObject, Transform container)
    {
        bool isCorrectPointForSpawn;
        float time = 1f;
        Vector2 currentSpawnPoint;
        Rect spawnZone = spawnArea.GetSpawnZone(playerPoint);
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();

        do
        {
            time -= GameTime.DeltaTime;
            currentSpawnPoint = new Vector2
            {
                x = Random.Range(spawnZone.x, spawnZone.x + spawnZone.width),
                y = Random.Range(spawnZone.y, spawnZone.y + spawnZone.height)
            };

            if(time <= 0)
            {
                break;
            }

            isCorrectPointForSpawn = true;

            try
            {
                if (Vector3.Distance(playerPoint, currentSpawnPoint) < 2f)
                {
                    isCorrectPointForSpawn = false;
                }

                for (int i = 0; i < enemies.Length; i++)
                {
                    if (Vector3.Distance(playerPoint, enemies[i].transform.position) < 2f)
                    {
                        isCorrectPointForSpawn = false;
                    }
                }
            }
            catch (System.NullReferenceException)
            {
                break;
            }
            catch(MissingReferenceException)
            {
                break;
            }
            yield return null;
        }
        while (!isCorrectPointForSpawn);

        Instantiate(spawnObject, currentSpawnPoint, Quaternion.identity, container).
            GetComponent<ISpawnObject>().Prepare(this, scoreHolder);
    }
}

[System.Serializable]
public class SpawnArea
{
    public Transform leftTop;
    public Transform leftDown;
    public Transform RightTop;
    public Transform RightDown;

    private Vector3 centerPoint;
    private float height;
    private float width;

    public void Prepare()
    {
        height = Mathf.Abs(RightTop.position.y - RightDown.position.y);
        width = Mathf.Abs(RightTop.position.x - leftTop.position.x);

        centerPoint.x = leftTop.position.x + width / 2;
        centerPoint.y = leftDown.position.y + height / 2;
    }

    public Rect GetSpawnZone(Vector3 playerPoint)
    {
        Vector2 position = leftDown.position;
        Vector2 size = new Vector2(width / 2, height / 2);

        if (playerPoint.x < centerPoint.x)
        {
            position.x = centerPoint.x;
        }

        if (playerPoint.y < centerPoint.y)
        {
            position.y = centerPoint.y;
        }
        return new Rect(position, size);
    }
}

[System.Serializable]
public class SpawnItem
{
    [Min(1)]
    public int weight = 1;

    public bool blocked = true;

    public GameObject spawnOhbject;
}
