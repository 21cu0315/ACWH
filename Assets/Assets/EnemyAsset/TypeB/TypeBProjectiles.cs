/*
 * �쐬���ԁF2022/11/20
 * �쐬�ҁ@�F��
 * �������e�F���[�U�[�̏���   �g��Script�F"LineRenderer"
 * 
 * �X�V���e�F   
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

        //�������W���L�^����
        m_startPos = transform.position;
        m_endPos = _endPos;

        Destroy(gameObject, m_destroyTime);

        m_laser = GetComponent<LineRenderer>();
        if (m_laser == null) {
            Debug.LogError("TypeBProjectiles�@�́@LineRenderer�R���|�[�l���g���K�v�ł��I");
            return;
        }

        //���[�U�[�̍��W��ݒ�
        m_laser.SetPosition(0, m_startPos);
        m_laser.SetPosition(1, m_endPos);

        //�����蔻�背�[�U�[�쐬
        RaycastHit2D hit2D = Physics2D.Raycast(m_startPos, m_endPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);         

        if (collision.gameObject.tag == "Player")
        {
            //�v���C���[�ɍU��
        }
    }

    ~TypeBProjectiles()
    {
        //�U���t���O��߂�
        m_enemyReference.SetAttacking(false);
    }
}
