using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public int m_hpMax = 50;
    public int m_hp ;
    //int m_hpMax;
    //int m_beLockLv;
    public SpriteRenderer m_SR;
    public int m_stopCNT = 0;
    public DamageFlash m_damageFlash;
    [SerializeField]protected BoxCollider2D m_col;
    //int m_beLockTimeCNTMax;
    [SerializeField] protected GameObject m_UIPrefab;
    [SerializeField] protected GameObject m_UI;
    [SerializeField] protected Image m_UIBar;
    [SerializeField] protected GameObject m_deathEffectPrefab;
    [SerializeReference] protected bool m_playerFound;
    [SerializeReference] protected Animator m_anim;

    protected virtual void Awake()
    {
        m_hp = m_hpMax;
        m_UI = Instantiate(m_UIPrefab,FindObjectOfType<Canvas>().transform);
        m_UI.GetComponent<UI_FollowCamera>().m_target = transform;
        m_UIBar = m_UI.transform.GetChild(0).GetComponent<Image>() ;
        SubClassStart();
    }
    protected virtual void FixedUpdate()
    {
        Main();

    }
    protected virtual void Main()
    {
        m_UIBar.fillAmount = (float)m_hp / m_hpMax;
        m_UIBar.color = Color.Lerp(Color.red, Color.yellow, m_UIBar.fillAmount);
        if (m_SR.isVisible)
        {
        }
        else
        {
            m_stopCNT--;
            if (m_stopCNT <= 0)
            {
                m_stopCNT = 0;
            }
        }
        SubClass();
        if (m_hp <= 0)
        {
            FindObjectOfType<GameRuler>().DealDamage(10);

            FindObjectOfType<GameRuler>().comboPlus();
          
            Destroy(m_UI);
            Destroy(gameObject, Time.fixedDeltaTime * 5f);
            Destroy(this);
            Destroy(m_col);
            FindObjectOfType<Controller>().setHaptics(0.3f, 15f);
           
                var tmp_death = Instantiate(m_deathEffectPrefab, m_SR.transform.position, m_SR.transform.rotation);
                tmp_death.transform.localScale = m_SR.transform.lossyScale;
                foreach (SpriteRenderer _sr in tmp_death.GetComponentsInChildren<SpriteRenderer>())
                {
                    _sr.sprite = m_SR.sprite;
                    _sr.sortingOrder = m_SR.sortingOrder;
                }
                Destroy(tmp_death, 0.75f);
            FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_Delete");

            
        }
    }

    protected virtual void SubClass() { }
    protected virtual void SubClassStart() { }
    public void Damage(int _damage)
    {
        m_hp -= _damage;
        m_damageFlash.SetDamage(true);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpSkill")) 
        {
            m_hp = 0;
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
    }
    public int GetHp()
    {
        return m_hp;
    }
}
