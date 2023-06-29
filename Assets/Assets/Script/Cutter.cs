using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    [SerializeField] bool m_charge;
    public int m_timer = 20;
    public int m_dir;
    public GameObject m_target;
    [SerializeField] GameObject m_effectPrefab;
    [SerializeField] Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_anim.SetInteger("timer", m_timer);
    }

    public void SetChargeOn()
    {
        m_charge = true;
    }
    public bool GetCharge()
    {
        return m_charge;
    }
    public void SetChargeOff()
    {
        m_charge = false;
    }
    public void SetOff() 
    {
        m_timer = 20;
       gameObject.SetActive(false);
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") )
        {
            collision.gameObject.TryGetComponent(out EnemyBase tmp_enemyBehave);
            if (tmp_enemyBehave)
            {
                tmp_enemyBehave.Damage(4);
                FindObjectOfType<GameRuler>().DealDamage(0);
                Destroy(Instantiate(m_effectPrefab, collision.transform.position, Quaternion.identity), 2f);
                FindObjectOfType<SoundManager>().PlayAudioOneShotWhileNotPlaying("SE_Cutter_Damage");
            }

        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.TryGetComponent(out EnemyBase tmp_enemyBehave);
            if (tmp_enemyBehave)
            {
                tmp_enemyBehave.Damage(4);
                FindObjectOfType<GameRuler>().DealDamage(0);
                Destroy(Instantiate(m_effectPrefab, collision.transform.position, Quaternion.identity), 2f);
                FindObjectOfType<SoundManager>().PlayAudioOneShotWhileNotPlaying("SE_Cutter_Damage");
            }
        }
    }

}
