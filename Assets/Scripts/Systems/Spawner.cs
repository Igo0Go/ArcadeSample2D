using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> bonusesForSpawn;
    [SerializeField]
    private List<GameObject> enemiesForSpawn;

    public Transform playerStarShip;

    [SerializeField]
    private ScoreHolder scoreHolder;

    [SerializeField]
    private SpawnArea spawnArea;

    [HideInInspector]
    public AudioSource source;

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        spawnArea.Prepare();
        SpawnNextBonus();
        SpawnNextEnemy();
    }

    public void SpawnNextBonus()
    {
        Bonus bonus = Instantiate(bonusesForSpawn[Random.Range(0, bonusesForSpawn.Count)], spawnArea.GetSpawnPointByPlayerPoint(playerStarShip.position), Quaternion.identity).GetComponent<Bonus>();
        bonus.PrepareBonus(scoreHolder, this);
    }

    public void SpawnNextEnemy()
    {
        BaseEnemy enemy = Instantiate(enemiesForSpawn[Random.Range(0, enemiesForSpawn.Count)], spawnArea.GetSpawnPointByPlayerPoint(playerStarShip.position), Quaternion.identity)
            .GetComponent<BaseEnemy>();
        enemy.PrepareEnemy(this, scoreHolder);
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
