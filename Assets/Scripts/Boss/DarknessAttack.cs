using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessAttack : MonoBehaviour
{
    public float lifetime = 1.15f;
    public LayerMask obstacleLayer;
    public float damage = 0.5f;
    public string ignoreTag; // 無視するタグ

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage dealDamage = other.GetComponent<DealDamage>();
            if (dealDamage != null)
            {
                dealDamage.Damage(damage);
            }
            Destroy(gameObject);
        }
        else if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag(ignoreTag)) // 無視するタグの場合は何もしない
        {
            return;
        }
    }
}
