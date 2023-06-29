/*
 * 作成時間：2022/11/20
 * 作成者　：董
 * 処理内容：レーザーの処理   使うScript："LineRenderer"
 * 
 * 更新内容：   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeBProjectiles : ProjectilesBase
{
    private LineRenderer m_laser;

    public override void SetUp(Vector3 _shootDir, Vector3 _endPos, Enemy _target = null)
    {
        m_enemyReference = _target;

        //初期座標を記録する
        m_startPos = transform.position;
        m_endPos = _endPos;

        Destroy(gameObject, m_destroyTime);

        m_laser = GetComponent<LineRenderer>();
        if (m_laser == null) {
            Debug.LogError("TypeBProjectiles　は　LineRendererコンポーネントが必要です！");
            return;
        }

        //レーザーの座標を設定
        m_laser.SetPosition(0, m_startPos);
        m_laser.SetPosition(1, m_endPos);

        //当たり判定レーザー作成
        RaycastHit2D hit2D = Physics2D.Raycast(m_startPos, m_endPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);         

        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーに攻撃
        }
    }

    ~TypeBProjectiles()
    {
        //攻撃フラグを閉める
        m_enemyReference.SetAttacking(false);
    }
}
