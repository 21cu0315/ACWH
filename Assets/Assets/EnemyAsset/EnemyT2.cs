using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyT2 : EnemyBase
{
    float m_distance = 5f;
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;

    [SerializeField]private float m_Ypos;
    [SerializeField] protected Animator[] m_anims;

    [SerializeField] private bool ToRightSide = true;

     private float timer = 5f;
    [SerializeField] private float floatTimer = 0f;


    
    private void Start()
    {
        pointA = new Vector2(transform.position.x - m_distance, 0 + transform.position.y);
        pointB = new Vector2(transform.position.x + m_distance, 0 + transform.position.y);
        m_Ypos = transform.position.y;
        transform.position = Vector2.Lerp(pointA, pointB,0.5f) ;
    }

    protected override void SubClass()
    {
        foreach (Animator _anim in m_anims) 
        {
            _anim.SetBool("PlayerFound", m_playerFound);
        }
        if (!m_playerFound) 
        {
            timer += Time.fixedDeltaTime;
            floatTimer += Time.fixedDeltaTime;
            if (ToRightSide)
            {
                timer += 0.05f;
                if (timer >= 10)
                {
                    ToRightSide = false;
                }
            }
            else
            {
                timer -= 0.05f;
                if (timer <= 0)
                {
                    ToRightSide = true;
                }
            }
        }
        
        transform.position = new Vector2(Mathf.Lerp(pointA.x, pointB.x, timer/10f), m_Ypos + 0.4f * Mathf.Sin(floatTimer * 2f));
    }


    protected override void OnTriggerStay2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player") )
        {
            m_playerFound = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_playerFound = false;
        }
    }
}
