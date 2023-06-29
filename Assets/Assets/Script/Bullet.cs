using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]protected Rigidbody2D m_rb2D;
    protected Vector2 m_dir;
    protected float m_speed;
    [SerializeField] protected Player m_player;
    [SerializeField] protected SpriteRenderer m_sr;
    [SerializeField] protected GameObject m_effectPrefab;
    [SerializeField] protected Transform m_effectPos;

    protected void FixedUpdate()
    {
        m_rb2D.MovePosition(m_rb2D.position + m_dir * m_speed * Time.fixedDeltaTime);
        if (!m_sr.isVisible) 
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent(out EnemyBase tmp_enemyBehave);
            if (tmp_enemyBehave) 
            {
                tmp_enemyBehave.Damage(4);
                FindObjectOfType<GameRuler>().DealDamage(0);
                Destroy(Instantiate(m_effectPrefab, m_effectPos.position, Quaternion.identity), 0.1f);
                Destroy(gameObject);
            }
           
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.TryGetComponent(out EnemyBase tmp_enemyBehave);
            if (tmp_enemyBehave)
            {
                tmp_enemyBehave.Damage(4);
                FindObjectOfType<GameRuler>().DealDamage(1);
                Destroy(Instantiate(m_effectPrefab, m_effectPos.position, Quaternion.identity), 0.1f);
                Destroy(gameObject);
            }

        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(Instantiate(m_effectPrefab, m_effectPos.position, Quaternion.identity), 0.1f);
            Destroy(gameObject);
        }
    }
    public void  setPlayer(Player _player) 
    {
        m_player = _player;
    }
    public void setDirection(Vector2 _dir)
    {
        m_dir = _dir;
    }
    public void setSpeed(float _speed)
    {
        m_speed = _speed;
    }
}
