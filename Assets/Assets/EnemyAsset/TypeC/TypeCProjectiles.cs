/*
 * �쐬���ԁF2022/11/20
 * �쐬�ҁ@�F��
 * �������e�F���e�̏���   
 * 
 * �X�V���e�F   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeCProjectiles : ProjectilesBase
{
    //���x�E�p�x
    [Header("[ -�h���N���X �����x�E�p�x - ]")]
    [SerializeField] private float m_volocity = 15;
    [SerializeField] private float m_angle = -270f;

    public override void SetUp(Vector3 _shootDir, Enemy _target = null)
    {
        ////���Ŏ��Ԃ�ݒ�
        StartCoroutine(ProjectilesDistory(m_destroyTime));

        //�G�Q�Ƃ��L�^
        m_enemyReference = _target;

        //�������W���L�^����
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
