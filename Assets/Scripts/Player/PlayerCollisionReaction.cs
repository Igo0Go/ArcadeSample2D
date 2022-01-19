using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCollisionReaction : MonoBehaviour
{
    [SerializeField]
    private PlayerShot playerShot;

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
                case BonusType.Laser:
                    break;
                case BonusType.Bomb:
                    break;
                case BonusType.Shield:
                    break;
                default:
                    break;
            }

        }
    }
}
