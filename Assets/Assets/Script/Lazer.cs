using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Vector2 m_startPos;
    public Vector2 m_center1Pos;
    public Vector2 m_center2Pos;
    public Vector2 m_EndPos;
    public GameObject m_End;

    public float m_timer = 0;
    [SerializeField] GameObject m_effPrefab;
    [SerializeField] BoxCollider2D m_col;

    // Update is called once per frame
    void FixedUpdate()
    {
       
            if (m_col.size != Vector2.one*0.1f)
            {
                m_col.size = Vector2.one * 0.1f;
            }
            m_timer += Time.fixedDeltaTime*5f;
            if (m_End)
            { 
            m_EndPos = m_End.transform.position;
               
            }
            else 
            {
                
            }
        if (m_timer >= 5f)
        {
            Destroy(gameObject);
        }
        transform.position = CubicLerp(m_startPos, m_center1Pos, m_center2Pos, m_EndPos, m_timer);
        
       
        
    }
    private Vector3 QuadraticLerp(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.LerpUnclamped(ab, bc, t);
    }

    private Vector2 CubicLerp(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 ab_bc = QuadraticLerp(a, b, c, t);
        Vector2 bc_cd = QuadraticLerp(b, c, d, t);


        return Vector3.LerpUnclamped(ab_bc, bc_cd, t);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject == m_End)
            {
                collision.gameObject.GetComponent<EnemyBase>().Damage(10);
                Destroy(Instantiate(m_effPrefab, collision.gameObject.transform.position, Quaternion.identity), 2f);
                FindObjectOfType<GameRuler>().DealDamage(0);
                Destroy(this);

            }

        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            if (collision.gameObject == m_End)
            {
                collision.gameObject.GetComponent<EnemyBase>().Damage(10);
                Destroy(Instantiate(m_effPrefab, collision.gameObject.transform.position, Quaternion.identity), 2f);
                FindObjectOfType<GameRuler>().DealDamage(2);
                Destroy(this);

            }

        }

    }
}
