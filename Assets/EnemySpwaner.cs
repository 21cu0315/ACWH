using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    //敵の実体
    [SerializeField] GameObject m_enemy;
    
    //アニメーション
    [SerializeField] Animator m_anim;

    //敵を生成する処理
   void SpwaneEnemy() 
    {
        //敵を生成する
        Instantiate(m_enemy, transform.position, Quaternion.identity);

        //スポーン判定を破棄
        Destroy(this);
        Destroy(gameObject, 2f);
        
        //SEを流す
        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_Spwan");
    }
    private void Start()
    {
        //アニメーションを無効化する
        m_anim.enabled = false;
    }

    //敵をスポーンするためのコリジョン判定用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーと当たった時
        if (collision.gameObject.CompareTag("Player")) 
        {
            //アニメーションを有効化する
            m_anim.enabled = true;
            //スポーン判定を破棄
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
