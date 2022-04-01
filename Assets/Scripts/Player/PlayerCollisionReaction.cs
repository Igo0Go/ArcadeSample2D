using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerShot))]
[RequireComponent(typeof(PlayerTargetTrackerShot))]
[RequireComponent(typeof(PlayerLaser))]
[RequireComponent(typeof(PlayerShield))]
public class PlayerCollisionReaction : MonoBehaviour
{
    private PlayerShot playerShot;
    private PlayerTargetTrackerShot playerTargetTracker;
    private PlayerLaser playerLaser;
    private PlayerShield playerShield;

    [SerializeField]
    private GameObject explosionDecal;

    private void Start()
    {
        playerShot = GetComponent<PlayerShot>();
        playerTargetTracker = GetComponent<PlayerTargetTrackerShot>();
        playerLaser = GetComponent<PlayerLaser>();
        playerShield = GetComponent<PlayerShield>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bonus"))
        {
            Bonus bonus = collision.GetComponent<Bonus>();
            bonus.TakeBonus();
            switch (bonus.type)
            {
                case BonusType.ScoreOnly:
                    break;
                case BonusType.Shot:
                    playerShot.AddBullets();
                    break;
                case BonusType.TargetTracker:
                    playerTargetTracker.AddBullets();
                    break;
                case BonusType.Laser:
                    playerLaser.AddPower();
                    break;
                case BonusType.Shield:
                    playerShield.ShowShield();
                    break;
                default:
                    break;
            }
        }

        if(collision.CompareTag("Enemies") && !playerShield.active && !playerShield.danger)
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            enemy.FindPlayer();

            Instantiate(explosionDecal, transform.position, Quaternion.identity);

            Destroy(gameObject, Time.fixedDeltaTime);
        }
    }
}
