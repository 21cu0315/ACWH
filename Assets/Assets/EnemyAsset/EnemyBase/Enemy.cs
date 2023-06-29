/*
 * 作成時間：2022/11/01
 * 作成者　：董
 * 処理内容：敵の基本処理
 * 
 * 更新内容：2022/12/07　18:30    Playerの取得はマネージャーで処理するようにしました   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{


    [Header("[ -移動距離- ]")]
    [SerializeField] private float m_moveDistance = 5f;



    [Header("[ -攻撃- ]")]
    [SerializeField] protected int m_attackDamge;

    [Header("[ -アニメーター- ]")]
    protected Animator enemyAnimator;

    [Header("[ -Player検索範囲 - ]")]
    [SerializeField] private float soundRange = 5;          //プレイヤーと敵の距離
    private static GameObject player = null;      //プレイヤーの参照

    [Header("[ -検索頻度 単位：s - ]")]
    [SerializeField] private float sensorInterval;          //検索の頻度

    private float senseTimer;
    private Transform nearbyPlayer = null;

    [Header("[ -死亡エフェクト- ]")]
    [SerializeField] private GameObject m_deadAnime;

    [Header("[ -攻撃頻度 単位：s - ]")]
    [SerializeField] private float m_attackInterval;
    [Header("[ -攻撃音声- ]")]
    [SerializeField] private AudioClip m_attackAudio;

    private EnemyState m_currentState;
    private float m_attackTimer = 100f;

    private bool isHpLock;

    protected Vector2 posA;                 //Aに移動するための座標
    protected Vector2 posB;                 //Bに移動するための座標
    protected float moveTimeCnt = 0f;       //移動処理カウンター
    protected bool canRightMove = false;    //向きフラグ

    protected bool m_isAttacking = false;   //攻撃中フラグ

    protected virtual void Start()
    {
        SetRotation(canRightMove);
        posA = new Vector2(transform.position.x - m_moveDistance, transform.position.y);       //移動処理の座標を初期化
        posB = new Vector2(transform.position.x + m_moveDistance, transform.position.y);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //EnemySense();

        //FSMUpdate();
    }


    //-------------------------Shootメソッド--------------------------
    protected virtual void EnemyShoot() { }

    //-------------------------移動処理--------------------------
    protected virtual void FixMove()
    {
        moveTimeCnt += (Time.deltaTime / 0.01666f) / 100;

        if (moveTimeCnt >= 1.0f)        {
            moveTimeCnt = 0.0f;
            canRightMove = !canRightMove;

            SetRotation(canRightMove);
        }

        if (canRightMove)        {
            this.transform.position = Vector2.Lerp(posA, posB, moveTimeCnt / 1.0f);
        }
        else        {
            this.transform.position = Vector2.Lerp(posB, posA, moveTimeCnt / 1.0f);
        }
    }
    //-------------------------移動処理--------------------------

    //--------------------------Enemy Finite State Machine------------------------------------------------
    public enum EnemyState
    {
        Seek,
        Chase,
        Attack,
        Dead
    }

    private void FSMUpdate()
    {
        if (m_currentState != EnemyState.Dead && GetHP() <= 0)
        {
            m_currentState = EnemyState.Dead;
        }

        switch (m_currentState)
        {
            case EnemyState.Seek:
                UpdateWanderState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
            case EnemyState.Dead:
                UpdateDeadState();
                break;
        }

    }

    protected virtual void UpdateWanderState()
    {
        Transform targetPlayer = this.GetNearbyPlayer();

        if (targetPlayer != null)
        {
            m_currentState = EnemyState.Attack;
            return;
        }

        FixMove();
        SetRotation(canRightMove);
    }

    protected virtual void UpdateChaseState()
    {
        Transform m_targetPlayer = this.GetNearbyPlayer();

        if (m_targetPlayer != null)
        {
            m_currentState = EnemyState.Attack;
            return;
        }
    }
    public void SetAttacking(bool _attackFlag)
    {
        m_isAttacking = _attackFlag;
    }

    protected virtual void UpdateAttackState()
    {
        m_attackTimer += Time.deltaTime;

        //アニメーターの設定
        //GetEnemyAnimator().SetBool(, true);      
        if (m_attackTimer > m_attackInterval)        {
            m_attackTimer = 0;

            //SE処理
            if (m_attackAudio != null)            {
                AudioSource.PlayClipAtPoint(m_attackAudio, this.transform.position);
            }
            EnemyShoot();
            m_isAttacking = true;

        }

        //攻撃してないときのみ別の状態に変更できる
        if (m_isAttacking != true || this.GetNearbyPlayer() == null)        {
            m_currentState = EnemyState.Seek;
            m_attackTimer = m_attackInterval;

            return;
        }
    }

    protected virtual void UpdateDeadState()
    {
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

    //--------------------------Enemy Finite State Machine------------------------------------------------

    //--------------------------プレイヤーを検索------------------------------------------------

    //プレーヤーを探す
    private void SenseNearbyPlayer()
    {
        ////playerのパスは設定してるかをチェック
        //if (player != null)       {
        //    float tmp_dist = Vector2.Distance(player.transform.position, this.transform.position);

        //    if (tmp_dist < soundRange)            {
        //        nearbyPlayer = player.transform;
        //    }
        //    else            {
        //        nearbyPlayer = null;
        //    }

        //}
        //else        {
        // //   Debug.LogError("enemy.cs/GameObject palyer is null");
        //}
    }

    private void EnemySense()
    {
        if (senseTimer >= sensorInterval)        {
            senseTimer = 0;
            SenseNearbyPlayer();
        }
        senseTimer += Time.deltaTime;
    }

    public Transform GetNearbyPlayer()
    {
        return nearbyPlayer;
    }

    //--------------------------プレイヤーを検索------------------------------------------------

    //-------------------------プロパティを操作するメソッド------------------------------------------------
    public void SetPlayer(GameObject _target)    {
        player = _target;
    }

    protected void SetState(EnemyState _state)
    {
        m_currentState = _state;
    }

    //子ClassのOnTriggerEnter
    public virtual void EnemyOnTriggerEnter(Collider2D collision)    { }

    //向き処理
    protected void SetRotation(bool _canMoveRight)
    {
        transform.localRotation = Quaternion.Euler(0f, _canMoveRight ? 0f : 180f, 0f);
    }

    //目標に向きを回転メソッド
    protected void EnemyLookAt(Transform _targetObject)
    {
        if (_targetObject == null)
        {
            return;
        }

        Vector3 tmp_distancePosition = this.transform.position - _targetObject.position;
        transform.localRotation = Quaternion.Euler(0, tmp_distancePosition.x <= 0.0f ? 0f : 180f, 0);
    }


    public int GetHP()
    {
        return m_hp;
    }
    protected Animator GetEnemyAnimator()
    {
        return enemyAnimator;
    }

    public void TakeDamage(int _damage)
    {
        if (isHpLock == true)
            return;

        m_hp -= _damage;
        if (m_hp <= 0)        {
            m_hp = 0;
            Destroy(gameObject);
        }
    }

    //-------------------------プロパティを操作するメソッド------------------------------------------------

}
