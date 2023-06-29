/*
 * �쐬���ԁF2022/11/01
 * �쐬�ҁ@�F��
 * �������e�F�G�̊�{����
 * 
 * �X�V���e�F2022/12/07�@18:30    Player�̎擾�̓}�l�[�W���[�ŏ�������悤�ɂ��܂���   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{


    [Header("[ -�ړ�����- ]")]
    [SerializeField] private float m_moveDistance = 5f;



    [Header("[ -�U��- ]")]
    [SerializeField] protected int m_attackDamge;

    [Header("[ -�A�j���[�^�[- ]")]
    protected Animator enemyAnimator;

    [Header("[ -Player�����͈� - ]")]
    [SerializeField] private float soundRange = 5;          //�v���C���[�ƓG�̋���
    private static GameObject player = null;      //�v���C���[�̎Q��

    [Header("[ -�����p�x �P�ʁFs - ]")]
    [SerializeField] private float sensorInterval;          //�����̕p�x

    private float senseTimer;
    private Transform nearbyPlayer = null;

    [Header("[ -���S�G�t�F�N�g- ]")]
    [SerializeField] private GameObject m_deadAnime;

    [Header("[ -�U���p�x �P�ʁFs - ]")]
    [SerializeField] private float m_attackInterval;
    [Header("[ -�U������- ]")]
    [SerializeField] private AudioClip m_attackAudio;

    private EnemyState m_currentState;
    private float m_attackTimer = 100f;

    private bool isHpLock;

    protected Vector2 posA;                 //A�Ɉړ����邽�߂̍��W
    protected Vector2 posB;                 //B�Ɉړ����邽�߂̍��W
    protected float moveTimeCnt = 0f;       //�ړ������J�E���^�[
    protected bool canRightMove = false;    //�����t���O

    protected bool m_isAttacking = false;   //�U�����t���O

    protected virtual void Start()
    {
        SetRotation(canRightMove);
        posA = new Vector2(transform.position.x - m_moveDistance, transform.position.y);       //�ړ������̍��W��������
        posB = new Vector2(transform.position.x + m_moveDistance, transform.position.y);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //EnemySense();

        //FSMUpdate();
    }


    //-------------------------Shoot���\�b�h--------------------------
    protected virtual void EnemyShoot() { }

    //-------------------------�ړ�����--------------------------
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
    //-------------------------�ړ�����--------------------------

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

        //�A�j���[�^�[�̐ݒ�
        //GetEnemyAnimator().SetBool(, true);      
        if (m_attackTimer > m_attackInterval)        {
            m_attackTimer = 0;

            //SE����
            if (m_attackAudio != null)            {
                AudioSource.PlayClipAtPoint(m_attackAudio, this.transform.position);
            }
            EnemyShoot();
            m_isAttacking = true;

        }

        //�U�����ĂȂ��Ƃ��̂ݕʂ̏�ԂɕύX�ł���
        if (m_isAttacking != true || this.GetNearbyPlayer() == null)        {
            m_currentState = EnemyState.Seek;
            m_attackTimer = m_attackInterval;

            return;
        }
    }

    protected virtual void UpdateDeadState()
    {
        //���S�A�j���V�����𗬂�
        ParticleSystem deadAnime = m_deadAnime.GetComponent<ParticleSystem>();
        if (deadAnime != null)        {
            Instantiate<ParticleSystem>(deadAnime, gameObject.transform.position, Quaternion.identity);
        }
        else        {
            Debug.LogWarning("���S��ParticleSystem���ݒ肳��Ă܂���");
        }

        Destroy(gameObject);
    }

    //--------------------------Enemy Finite State Machine------------------------------------------------

    //--------------------------�v���C���[������------------------------------------------------

    //�v���[���[��T��
    private void SenseNearbyPlayer()
    {
        ////player�̃p�X�͐ݒ肵�Ă邩���`�F�b�N
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

    //--------------------------�v���C���[������------------------------------------------------

    //-------------------------�v���p�e�B�𑀍삷�郁�\�b�h------------------------------------------------
    public void SetPlayer(GameObject _target)    {
        player = _target;
    }

    protected void SetState(EnemyState _state)
    {
        m_currentState = _state;
    }

    //�qClass��OnTriggerEnter
    public virtual void EnemyOnTriggerEnter(Collider2D collision)    { }

    //��������
    protected void SetRotation(bool _canMoveRight)
    {
        transform.localRotation = Quaternion.Euler(0f, _canMoveRight ? 0f : 180f, 0f);
    }

    //�ڕW�Ɍ�������]���\�b�h
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

    //-------------------------�v���p�e�B�𑀍삷�郁�\�b�h------------------------------------------------

}
