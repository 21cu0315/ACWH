using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBase>().Damage(4);
            FindObjectOfType<GameRuler>().DealDamage(1);
            Destroy(Instantiate(m_effectPrefab, m_effectPos.position, Quaternion.identity), 2f);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<Boss>().Damage(4);
            FindObjectOfType<GameRuler>().DealDamage(1);
            Destroy(Instantiate(m_effectPrefab, m_effectPos.position, Quaternion.identity), 2f);
        }
    
    }
}
