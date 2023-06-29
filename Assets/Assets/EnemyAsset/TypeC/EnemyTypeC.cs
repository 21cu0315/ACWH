/*
 * �쐬���ԁF2022/11/01
 * �쐬�ҁ@�F��
 * �������e�F�GTypeC�̏���   �g��Script�F"EnemyShootProjectiles"
 * 
 * �X�V���e�F2022/12/07�@18:30    Player�̎擾�̓}�l�[�W���[�ŏ�������悤�ɂ��܂���   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeC : Enemy
{
    [Header("[ - �h���N���X �ˌ����W - ]")]
    [SerializeField] private Transform m_gunStartPos;
    [Header("[ - �h���N���X �U���̒x������ - ]")]
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
