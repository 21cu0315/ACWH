using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT1_Weapon : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_sr;
    public int m_dir = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_sr.isVisible) 
        {
            Destroy(gameObject);
        }
        transform.position = new Vector3(transform.position.x + m_dir*0.2f, transform.position.y);
        if (m_dir > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1) * 3;
        }
        else 
        {
            transform.localScale = Vector3.one* 3;
        }
    }
}
