using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Min(0)] [SerializeField] private float shieldTime = 3;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private SpriteRenderer littleShield;

    [HideInInspector] public bool active;

    [HideInInspector] public bool danger;

    public Color shieldColor;

    [SerializeField] [Range(1, 4)] private float rbVelocityLenghtForShieldActivation = 2.5f;

    private float currentShield;

    private void Start()
    {
        shieldColor = spriteRenderer.color;
        spriteRenderer.gameObject.SetActive(false);
        GetComponent<PlayerMoveControl>().rbVelocityChanged.AddListener(CheckVelocityValue);
    }

    public void ShowShield()
    {
        StopAllCoroutines();
        spriteRenderer.color = shieldColor;
        spriteRenderer.gameObject.SetActive(true);
        currentShield = shieldTime;
        StartCoroutine(ShieldCoroutine());
        active = true;
    }

    private IEnumerator ShieldCoroutine()
    {
        float t = 1;
        while (currentShield > 0)
        {
            spriteRenderer.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, Mathf.PingPong(t, 1));
            t += Time.deltaTime;
            currentShield -= Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = shieldColor;
        yield return new WaitForSeconds(1);
        spriteRenderer.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, 0);
        yield return new WaitForSeconds(1);
        spriteRenderer.color = shieldColor;
        yield return new WaitForSeconds(1);
        active = false;
        spriteRenderer.gameObject.SetActive(false);
    }

    private void CheckVelocityValue(float value)
    {
        danger = value > rbVelocityLenghtForShieldActivation;
        littleShield.gameObject.SetActive(danger);

        if (danger)
        {
            float alpha = Mathf.Clamp01((value - rbVelocityLenghtForShieldActivation) /
                                        (rbVelocityLenghtForShieldActivation));
            littleShield.color = new Color(littleShield.color.r, littleShield.color.g, littleShield.color.b, alpha);
        }
    }
}