/*
 * 作成時間：2022/11/01
 * 作成者　：董
 * 処理内容：発射物の基本処理
 * 
 * 更新内容：   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesBase : MonoBehaviour
{
    protected Enemy m_enemyReference;
    protected Vector3 m_vShootDir;
    protected Vector3 m_startPos;
    protected Vector3 m_endPos;


    [Header("[ -破壊時間- ]")]
    [SerializeField] protected float m_destroyTime = 4f;

    [Header("[ -爆発エフェクト- ]")]
    [SerializeField] protected GameObject m_deadAnime;

    //XとY方向の座標
    protected float m_volocityY = 0;
    protected float m_volocityX = 0;
    protected float m_accumulateTime = 0;

    //移動フラグ
    private bool m_canMove = true;

    protected virtual void FixedUpdate()
    {
        if (m_canMove)        {
            move();
        }
    }

    protected virtual void move() { }

    //発射物を消滅
    public IEnumerator ProjectilesDistory(float _delayTime = 0f)
    {
        yield return new WaitForSeconds(_delayTime);

        //死亡アニメションを流す
        ParticleSystem deadAnime = m_deadAnime.GetComponent<ParticleSystem>();
        if (deadAnime != null)        {
            Instantiate<ParticleSystem>(deadAnime, gameObject.transform.position, Quaternion.identity);
        }
        else        {
            Debug.LogWarning("死亡のParticleSystemが設定されてません");
        }

        Destroy(gameObject);
    }

    //角度変換関数
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    //プロパティの設定
    public void SetMoveFlag(bool _MoveFlag)
    {
        m_canMove = _MoveFlag;
    }
    public virtual void SetUp(Vector3 _shootDir,Enemy _target = null) { }
    public virtual void SetUp(Vector3 _shootDir,Vector3 _endPos, Enemy _target = null) { }


   

}
