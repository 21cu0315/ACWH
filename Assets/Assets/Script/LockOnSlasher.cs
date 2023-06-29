using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSlasher : MonoBehaviour
{
    Rigidbody2D m_rb2D;
    Vector2 m_dir;
    float m_speed;
    Player m_player;
    SpriteRenderer m_sr;

    void Start()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (m_rb2D)
        {
            m_rb2D.velocity = m_dir * m_speed;
        }
        if (!m_sr.isVisible)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LockCol") && m_sr.isVisible)
        {
           // m_player.getWeapon().addObj(collision.transform.parent.gameObject);
                transform.position = collision.transform.parent.position;
         
            Destroy(m_rb2D);
            Destroy(gameObject,0.25f);
            Destroy(GetComponent<BoxCollider2D>());

        }
    }
    public void setPlayer(Player _player)
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
