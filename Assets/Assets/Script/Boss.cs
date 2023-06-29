using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    [SerializeField] GameObject m_SpSkill;
    [SerializeField] GameObject m_SpSkillPrefab;
    [SerializeField] GameObject m_atkX;
    [SerializeField] GameObject m_atkX2;
    [SerializeField] GameObject m_atkNova;
    [SerializeField] Transform[] m_atkPos;
    [SerializeField] GameObject m_atkXRot;

    void CreateAtkX()
    {
        Instantiate(m_atkX, FindObjectOfType<Player>().transform.position, Quaternion.identity);
    }
    void CreateAtk()
    {
        Instantiate(m_atkX, FindObjectOfType<Player>().transform.position, Quaternion.Euler(0,0,45f));
    }
    void CreateAtkX2()
    {
        Instantiate(m_atkX2, FindObjectOfType<Player>().transform.position, Quaternion.identity);
    }
    void CreateAtk2()
    {
        Instantiate(m_atkX2, FindObjectOfType<Player>().transform.position, Quaternion.Euler(0, 0, 45f));
    }
    void CreateAtkXRot()
    {
        Instantiate(m_atkXRot, transform.position, Quaternion.identity);
    }
    void CreateAtkNova(int _posIndex)
    {
        Instantiate(m_atkNova, m_atkPos[_posIndex].position, m_atkPos[_posIndex].rotation);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_hp = m_hpMax;

    }
   
    // Update is called once per frame
    void Update()
    {
        m_anim.SetInteger("Hp", m_hp);
    }
   override protected void FixedUpdate()
    {
        //m_anim.SetInteger("Hp", m_hp);
        m_UIBar.fillAmount = (float)m_hp / m_hpMax;
        m_UIBar.color = Color.Lerp(Color.red, Color.yellow, m_UIBar.fillAmount);
    }
    override protected  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpSkill"))
        {
            m_hp -= 250;
            FindObjectOfType<GameRuler>().DealDamage(250);
        }
    }
    void SPSkill() 
    {
       Instantiate(m_SpSkillPrefab, FindObjectOfType<Canvas>().transform);
        FindObjectOfType<GameRuler>().m_player.m_SPSkillOn = true;

        Time.timeScale = 0;
    }

}
