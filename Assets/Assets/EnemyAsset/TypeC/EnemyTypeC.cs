/*
 * 作成時間：2022/11/01
 * 作成者　：董
 * 処理内容：敵TypeCの処理   使うScript："EnemyShootProjectiles"
 * 
 * 更新内容：2022/12/07　18:30    Playerの取得はマネージャーで処理するようにしました   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeC : Enemy
{
    [Header("[ - 派生クラス 射撃座標 - ]")]
    [SerializeField] private Transform m_gunStartPos;
    [Header("[ - 派生クラス 攻撃の遅延時間 - ]")]
    [SerializeField] private float m_delayTime;

    protected override void UpdateAttackState()
    {
        FixMove();
        base.UpdateAttackState();
    }

    protected override void EnemyShoot()
    {
        StartCoroutine(GetComponent<EnemyShootProjectiles>().Shoot(m_gunStartPos, this, m_delayTime));
    }

}
