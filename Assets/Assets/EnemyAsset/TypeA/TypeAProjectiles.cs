/*
 * �쐬���ԁF2022/11/10
 * �쐬�ҁ@�F��
 * �������e�F���[�P�b�g�e�̏���  
 * 
 * �X�V���e�F   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAProjectiles : ProjectilesBase
{

    //���x�E�p�x
    [Header("[ -�h���N���X �����x�E�p�x - ]")]
    [SerializeField] private float m_volocity = 15;
    [SerializeField] private float m_angle = 30f;

    public override void SetUp(Vector3 _shootDir, Enemy _target = null)
    {
        m_enemyReference = _target;

        //�������W���L�^����
        m_startPos = transform.position;

        //�p�x�ݒ�
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(_shootDir));

        Destroy(gameObject, m_destroyTime);

        //
        m_volocityY = Mathf.Sin(Mathf.Deg2Rad * m_angle) * m_volocity;
        m_volocityX = Mathf.Cos(Mathf.Deg2Rad * m_angle) * m_volocity * _shootDir.x;//
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if(collision.gameObject.tag == "Player")        {
            Destroy(gameObject,0.2f);
        }
    }

    protected override void move()
    {
        m_accumulateTime += Time.deltaTime;
        //
        float x = m_volocityX * m_accumulateTime;

        //
        float y = m_volocityY * m_accumulateTime - 9.8f * 0.5f * Mathf.Pow(m_accumulateTime, 2);

        Vector3 pos = new Vector3(x, y, 0);

        transform.position = pos + m_startPos;
    }

     ~TypeAProjectiles()
    {
        //�U���t���O��߂�
        m_enemyReference.SetAttacking(false);
    }
}
