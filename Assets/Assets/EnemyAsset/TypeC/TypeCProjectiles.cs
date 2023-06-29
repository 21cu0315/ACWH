/*
 * 作成時間：2022/11/20
 * 作成者　：董
 * 処理内容：爆弾の処理   
 * 
 * 更新内容：   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeCProjectiles : ProjectilesBase
{
    //速度・角度
    [Header("[ -派生クラス 加速度・角度 - ]")]
    [SerializeField] private float m_volocity = 15;
    [SerializeField] private float m_angle = -270f;

    public override void SetUp(Vector3 _shootDir, Enemy _target = null)
    {
        ////消滅時間を設定
        StartCoroutine(ProjectilesDistory(m_destroyTime));

        //敵参照を記録
        m_enemyReference = _target;

        //初期座標を記録する
        m_startPos = transform.position;

        m_volocityY = Mathf.Sin(Mathf.Deg2Rad * m_angle) * m_volocity;
        m_volocityX = Mathf.Cos(Mathf.Deg2Rad * m_angle) * m_volocity * _shootDir.x;//

    }

    protected override void move()
    {
        m_accumulateTime += Time.deltaTime;
        //
        float x = m_volocityX * m_accumulateTime;
        float y = m_volocityY * m_accumulateTime - 9.8f * 0.5f * Mathf.Pow(m_accumulateTime, 2);

        Vector3 pos = new Vector3(x, y, 0);

        transform.position = pos + m_startPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            StartCoroutine(ProjectilesDistory());
        }
        else if(collision.tag == "Ground")
        {
            SetMoveFlag(false);
            StartCoroutine(ProjectilesDistory());
        }
    }

    private void OnDisable()
    {
        
    }
}
