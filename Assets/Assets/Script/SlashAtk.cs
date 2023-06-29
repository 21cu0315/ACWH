using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAtk : MonoBehaviour
{

    [SerializeField] GameObject m_effPrefab;
    int m_damage = 20;
    public void SetDamage( int _damage) 
    {
        m_damage = _damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBase>().Damage(m_damage);
            FindObjectOfType<GameRuler>().DealDamage(2);
            Destroy(Instantiate(m_effPrefab, collision.gameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))), 0.5f);
            Destroy(this);
        }

    }
}
