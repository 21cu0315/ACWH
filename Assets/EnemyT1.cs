using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT1 : EnemyBase
{
    //敵の行動状態
    [SerializeReference] protected int m_state = 0;
    //行動状態のカウンター
    [SerializeReference] protected int m_stateCNT = 120;
    //武器
    [SerializeField] GameObject m_weapon;
    //武器の位置
    [SerializeField] Transform m_weaponPos;

    protected override void SubClass() 
    {
       //敵が現れた時
        if (m_SR.isVisible)
        {
            //止まる用カウンター
            m_stopCNT = 90;
            //行動状態のカウンターを回す
            m_stateCNT--;
            //カウンターがゼロより小さい場合
            if (m_stateCNT <= 0)
            {
                //行動状態をリセット
                m_state = 0;
                //カウンターをリセット
                m_stateCNT = 120;
            }
        }
        //敵が現れていない時
        else
        {
            //行動状態をリセット
            m_state = 0;
        }
        //アニメーションをリセット
        m_anim.SetInteger("State", m_state);
    }

    //コリジョンが発生した時
     protected override void  OnTriggerStay2D(Collider2D collision)
    {
        //プレイヤーと当たった時
        if (collision.gameObject.CompareTag("Player"))
        {
            //状態を変える
            m_state = 2;
            //プレイヤーを弾く処理
            //方向を決める
            if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }
    //武器を投げる
    void ThrowWeapon()
    {
        //武器を作成する
        GameObject tmp_weapon = Instantiate(m_weapon, m_weaponPos.position, Quaternion.identity);
        
        //SEを流す
        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_T1Weapon");

        //投げる方向を決める
        if (transform.localScale.x < 0)
        {
            tmp_weapon.GetComponent<EnemyT1_Weapon>().m_dir = 1;

        }
        else
        {
            tmp_weapon.GetComponent<EnemyT1_Weapon>().m_dir = -1;

        }
    }
}
