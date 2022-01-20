using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Min(0)]
    [SerializeField]
    private float shieldTime = 3;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [HideInInspector]
    public bool active;

    private float currentShield;

    private void Start()
    {
        spriteRenderer.gameObject.SetActive(false);
    }

    public void ShowShield()
    {
        if(!active)
        {
            spriteRenderer.gameObject.SetActive(true);
            currentShield = shieldTime;
            StartCoroutine(ShieldCoroutine());
            active = true;
        }
        else
        {
            currentShield = shieldTime;
        }
    }

    private IEnumerator ShieldCoroutine()
    {
        Color shieldColor = spriteRenderer.color;

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
}
