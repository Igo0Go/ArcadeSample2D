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
        if(playerStarShip != null)
        {
            Instantiate(deadWavePrefab, playerStarShip.position, Quaternion.identity);
        }
    }

    public void SpawnConcreteBonus(int numberInList)
    {
        if (playerStarShip != null)
        {
            Bonus bonus = Instantiate(bonusesForSpawn[numberInList].spawnOhbject,
                spawnArea.GetSpawnPointByPlayerPoint(playerStarShip.position),
                Quaternion.identity,
                bonusesContainer).GetComponent<Bonus>();
            bonus.PrepareBonus(scoreHolder, this);
        }
    }

    public void SpawnConcreteEnemy(int numberInList)
    {
        if (playerStarShip != null)
        {
            BaseEnemy enemy = Instantiate(enemiesForSpawn[numberInList].spawnOhbject,
                spawnArea.GetSpawnPointByPlayerPoint(playerStarShip.position),
                Quaternion.identity,
                enemiesContainer).GetComponent<BaseEnemy>();
            enemy.PrepareEnemy(this, scoreHolder);
        }
    }

    public void SpawnNextBonus()
    {
        if (playerStarShip != null)
        {
            Bonus bonus = Instantiate(GetRandomObject(bonusesForSpawn, totalBonusWeight),
            spawnArea.GetSpawnPointByPlayerPoint(playerStarShip.position),
            Quaternion.identity,
            bonusesContainer).GetComponent<Bonus>();
            bonus.PrepareBonus(scoreHolder, this);
        }
    }

    public void SpawnNextEnemy()
    {
        if(playerStarShip != null)
        {
            BaseEnemy enemy = Instantiate(GetRandomObject(enemiesForSpawn, totalEnemyWeight),
                spawnArea.GetSpawnPointByPlayerPoint(playerStarShip.position),
                Quaternion.identity,
                enemiesContainer).GetComponent<BaseEnemy>();
            enemy.PrepareEnemy(this, scoreHolder);
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

    public Vector2 GetSpawnPointByPlayerPoint (Vector3 playerPoint)
    {
        Vector2 resultSpawnPoint;
        do
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


            Rect spawnZone = new Rect(position, size);

            resultSpawnPoint = new Vector2
            {
                x = Random.Range(spawnZone.x, spawnZone.x + spawnZone.width),
                y = Random.Range(spawnZone.y, spawnZone.y + spawnZone.height)
            };
        }
        while (Vector3.Distance(playerPoint, resultSpawnPoint) < 2f);

        return resultSpawnPoint;
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
