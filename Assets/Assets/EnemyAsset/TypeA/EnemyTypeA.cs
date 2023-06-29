/*
 * 作成時間：2022/11/0
 * 作成者　：董
 * 処理内容：敵Aの処理   使うScript："EnemyShootProjectiles"
 * 
 * 更新内容：   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeA : Enemy
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        EnemyLookAt(GetNearbyPlayer());
    }

    [Header("[ -派生クラス 射撃座標 - ]")]
    [SerializeField] protected Transform m_gunStartPos;
    [SerializeField] protected Transform m_gunEndPos;

    protected override void EnemyShoot()
    {
        if (m_gunStartPos != null && m_gunEndPos != null)
        {
            GetComponent<EnemyShootProjectiles>().Shoot(m_gunStartPos, m_gunEndPos);
        }
        else
        {
            Debug.LogError("firePos is null");
        }
    }

    //死亡アニメションのテスト用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_hp = 0;
        }
    }
}




