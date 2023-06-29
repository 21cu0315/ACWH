/*
 * 作成時間：2022/11/0
 * 作成者　：董
 * 処理内容：敵Bの処理   使うScript："EnemyShootProjectiles"
 * 
 * 更新内容：   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeB : Enemy
{
    private Quaternion m_rotationA;
    private Quaternion m_rotationB;

    [Header("[ - 派生クラス 攻撃装置 - ]")]
    [SerializeField] private Transform m_attackObj;

    [Header("[ - 派生クラス 射撃座標 - ]")]
    [SerializeField] private Transform m_gunStartPos;
    [Header("[ - 派生クラス 攻撃の遅延時間 - ]")]
    [SerializeField] private float m_delayTime;

    private bool m_IsOverlappingPlayer = false;
   
    protected override void Start()
    {
        m_rotationA = Quaternion.Euler(0, 0, 0);
        m_rotationB = Quaternion.Euler(0, 0, -180);

        m_attackObj.localRotation = m_rotationB;
    }

    protected override void FixMove()
    {
        moveTimeCnt += 0.01f;

        if (moveTimeCnt >= 1.0f)        {
            moveTimeCnt = 0.0f;

            canRightMove = !canRightMove;
        }

        if (canRightMove)        {
            m_attackObj.localRotation = Quaternion.Lerp(m_rotationA, m_rotationB, moveTimeCnt / 1.0f);
        }
        else        {
            m_attackObj.localRotation = Quaternion.Lerp(m_rotationB, m_rotationA, moveTimeCnt / 1.0f);
        }
    }

    public override void EnemyOnTriggerEnter(Collider2D collision)
    {
        if (collision.tag == "Player")        {
            m_IsOverlappingPlayer = true;
        }
        else        {
            m_IsOverlappingPlayer = false;
        }
    }

    //死亡アニメションのテスト用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")        {
            m_hp = 0;
        }
    }

    protected override void UpdateWanderState()
    {
        Transform targetPlayer = GetNearbyPlayer();

        if (targetPlayer != null && m_IsOverlappingPlayer != false)        {
            SetState(EnemyState.Attack);
            return;
        }
        FixMove();
    }

    protected override void EnemyShoot()
    {
        if (GetNearbyPlayer().position == null)        {
            return;
        }

        Vector3 tmp_endPos = GetNearbyPlayer().position - m_attackObj.position;
        tmp_endPos = tmp_endPos.normalized;
        tmp_endPos *= 50;

        if (m_gunStartPos != null)        {
            StartCoroutine( GetComponent<EnemyShootProjectiles>().Shoot(m_gunStartPos, tmp_endPos, this, m_delayTime) );
            
        }
        else        {
            StartCoroutine( GetComponent<EnemyShootProjectiles>().Shoot(m_attackObj, tmp_endPos, this, m_delayTime) );
        }
    }
}
